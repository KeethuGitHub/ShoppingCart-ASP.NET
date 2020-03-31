using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ShoppingWebApp.DB;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Index", "Home");

            Session["LoginType"] = null;

            Dictionary<int, int> cart = (Dictionary<int, int>)Session["Cart"];
            List<Product> productList = new List<Product>();
            var keyList = cart.Keys;
            foreach (int key in keyList.ToList())
            {
                if (cart[key] == 0)
                    cart.Remove(key);
            }
            Session["Cart"] = cart;

            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                productList = db.Product.Where(p => keyList.Any(k => k == p.ID)).ToList();
            }

            double cartSubTotal = 0;
            foreach (int key in keyList.ToList())
            {
                Product p = productList.Where(m => m.ID == key).FirstOrDefault();
                cartSubTotal += (cart[key] * p.Price);
            }
            double discountPercent = 0;
            double cartTotal = cartSubTotal * (1 - discountPercent / 100);

            ViewBag.Cart = cart;
            ViewBag.ProductList = productList;
            ViewBag.CartSubTotal = cartSubTotal;
            ViewBag.DiscountPercent = discountPercent;
            ViewBag.CartTotal = cartTotal;

            return View();
        }

        public ActionResult UpdateSessionCart(string action, int productID)
        {
            Dictionary<int, int> cart = (Dictionary<int, int>)Session["Cart"];
            int sessionCartQty = (int)Session["CartQty"];
            if (action == "add" || action == "plus")
            {
                if (cart.ContainsKey(productID))
                    cart[productID] += 1;
                else
                    cart[productID] = 1;

                sessionCartQty += 1;
            }
            else if (action == "minus")
            {
                int cartQty = cart[productID];
                if (cartQty > 0)
                {
                    cart[productID] -= 1;
                    sessionCartQty -= 1;
                }
            }
            else
            {
                sessionCartQty -= cart[productID];
                cart.Remove(productID);
            }
            Session["Cart"] = cart;
            Session["CartQty"] = sessionCartQty;

            if (action == "add")
                return PartialView("~/views/Home/_CartPartialView.cshtml");
            else if (action == "plus" || action == "minus")
            {
                ViewDataDictionary viewData = new ViewDataDictionary() { { "ID", productID }, { "Qty", cart[productID] } };
                var partialView = PartialView("_QtyPartialView");
                partialView.ViewData = viewData;

                return partialView;
            }
            else
                return Json(true);
        }

        public ActionResult Checkout()
        {
            Dictionary<int, int> cart = (Dictionary<int, int>)Session["Cart"];
            foreach (int k in cart.Keys.ToList())
            {
                if (cart[k] == 0)
                    cart.Remove(k);
            }
            Session["Cart"] = cart;

            if (cart.Count == 0)
                return RedirectToAction("Index", "Cart");

            if ((string)Session["User"] == "guest")
            {
                Session["LoginType"] = "CartLogin";
                return RedirectToAction("Login", "Home");
            }

            return RedirectToAction("ProcessCheckout", "Cart");
        }

        public ActionResult ProcessCheckout()
        {
            Customer customer = (Customer)Session["Customer"];
            Dictionary<int, int> cart = (Dictionary<int, int>)Session["Cart"];
            var keyList = cart.Keys;
            List<Product> productList = new List<Product>();

            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                productList = db.Product.Where(p => keyList.Any(k => k == p.ID)).ToList();
                double totalPrice = 0.0;
                foreach (Product p in productList)
                    totalPrice += (p.Price * cart[p.ID]);

                Order order = new Order() { Date = DateTime.Now, Price = totalPrice, Products = new List<Product>() };
                order.Customer = db.Customer.Where(c => c.ID == customer.ID).FirstOrDefault();
                foreach (Product p in productList)
                    order.Products.Add(p);
                db.Order.Add(order);

                List<OrderDetail> odList = new List<OrderDetail>();
                foreach (Product p in productList)
                {
                    OrderDetail od = new OrderDetail() { Quantity = cart[p.ID], Order = order, Product = p };
                    db.OrderDetail.Add(od);
                    odList.Add(od);
                }

                foreach (OrderDetail od in odList)
                {
                    for (int i = 0; i < od.Quantity; i++)
                    {
                        OrderDetailExtended ode = new OrderDetailExtended() { ActivationCode = Guid.NewGuid() };
                        ode.OrderDetail = od;
                        db.OrderDetailExtended.Add(ode);
                    }
                }

                db.SaveChanges();
            }
            Session["Cart"] = new Dictionary<int, int>();
            Session["CartQty"] = 0;

            return RedirectToAction("Index", "Purchase");
        }
    }
}