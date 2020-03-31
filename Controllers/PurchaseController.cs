//using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

using ShoppingWebApp.DB;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    public class PurchaseController : Controller
    {
        public ActionResult Index()
        {
            if (Session["User"] == null || (string)Session["User"] == "guest")
                return RedirectToAction("Index", "Home");

            Customer customer = (Customer)Session["Customer"];
            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                var grpResult = from C in db.Customer
                                from P in db.Product
                                from O in db.Order
                                from OD in db.OrderDetail
                                from ODE in db.OrderDetailExtended
                                where (C.ID == customer.ID && O.CustomerID == C.ID && OD.OrderID == O.ID && OD.ProductID == P.ID && ODE.OrderDetailID == OD.ID)
                                group ODE.ActivationCode by new { P.Name, P.Description, O.Date, OD.Quantity } into grp
                                select (new Purchase()
                                {
                                    Name = grp.Key.Name,
                                    Description = grp.Key.Description,
                                    Date = grp.Key.Date,
                                    Quantity = grp.Key.Quantity,
                                    ActivationCodes = grp.ToList()
                                });

                var orderResult = from r in grpResult
                              orderby r.Date descending, r.Name
                              select r;

                var orderResultList = orderResult.ToList();
                /*
                foreach (var grp in orderResultList)
                {
                    Debug.WriteLine("{0}, {1}, {2}, {3}", grp.Name, grp.Description, grp.Date, grp.Quantity);
                    foreach (var ac in grp.ActivationCodes)
                        Debug.WriteLine(ac);
                }
                */
                return View(orderResultList);
            }
        }
    }
}