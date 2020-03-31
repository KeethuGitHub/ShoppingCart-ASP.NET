using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Windows;
using ShoppingWebApp.DB;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                Session["User"] = "guest";
                Session["Cart"] = new Dictionary<int, int>();
                Session["CartQty"] = 0;
            }

            List<Product> productList = new List<Product>();
            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                productList = db.Product.ToList();
            }
            ViewBag.ProductList = productList;

            return View();
        }

        public ActionResult Register()
        {
            if (Session["User"] == null || (string)Session["User"] == "member")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer newCustomer)
        {
            if (newCustomer.Password != newCustomer.ConfirmPassword)
            {
                MessageBox.Show("Password and confirm password do not match!");
                return View();
            }

            if (newCustomer.Dateofbirth.Value > DateTime.Now)
            {
                MessageBox.Show("Invalid Date of Birth!");
                return View();
            }

            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                Customer customer = db.Customer.Where(x => x.Username == newCustomer.Username).FirstOrDefault();
                if (customer != null)
                {
                    MessageBox.Show("Username already exists! Please choose another one.");
                    return View();
                }
                string password = newCustomer.Password;
                newCustomer.Password = Functions.ComputeSHA256(password);
                newCustomer.ConfirmPassword = null;
                newCustomer.JoinDate = DateTime.Now;
                db.Customer.Add(newCustomer);
                db.SaveChanges();
            }
            MessageBox.Show("Account successfully registered! You may now login.");

            return RedirectToAction("Login", "Home");
        }

        public ActionResult Login()
        {
            if (Session["User"] == null || (string)Session["User"] == "member")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Login(Customer newCustomer)
        {
            Customer customer = null;
            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                customer = db.Customer.Where(x => x.Username == newCustomer.Username).FirstOrDefault();
                if (customer == null || customer.Password != Functions.ComputeSHA256(newCustomer.Password))
                {
                    MessageBox.Show("Invalid username and/or password! Please try again.");
                    return View();
                }
            }
            Session["User"] = "member";
            Session["Customer"] = customer;

            if ((string)Session["LoginType"] == "CartLogin")
                return RedirectToAction("ProcessCheckout", "Cart");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Search(string searchStr)
        {
            List<Product> productList = new List<Product>();
            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                if (string.IsNullOrWhiteSpace(searchStr))
                    productList = db.Product.ToList();
                else
                    productList = db.Product.Where(p => p.Name.Contains(searchStr) || p.Description.Contains(searchStr) || p.Price.ToString().Contains(searchStr)
                    || db.Category.Any(c => c.ID == p.CategoryID && c.Name.Contains(searchStr))).ToList();
            }
            ViewBag.SearchStr = searchStr;
            ViewBag.ProductList = productList;

            return PartialView("_CataloguePartialView");
        }

        public ActionResult ForgotPassword()
        {
            if (Session["User"] == null || (string)Session["User"] == "member")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(FormCollection form)
        {
            string username = form["Username"];
            string email = form["Email"];

            string message = "";
            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                Customer account = db.Customer.Where(a => a.Email == email && a.Username == username).FirstOrDefault();
                if (account != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetCode = resetCode;
                    db.SaveChanges();
                    message = "Reset password link has been sent to your email address!";
                }
                else
                    message = "Invalid username and/or email! Please try again.";
            }
            MessageBox.Show(message);

            return View();
        }

        [HttpPost]
        public void SendVerificationLinkEmail(string email, string resetCode, string emailFor = "ResetPassword")
        {
            var verifyUrl = "/Home/" + emailFor + "?id=" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            string subject = "";
            string body = "";

            var fromEmail = new MailAddress("Team10bca2019@gmail.com", "Team 10b");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "q1w2e3@--";

            if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "<p>Hi,</p>" +
                    "<p>We have received a request to reset your account password. Please click on the link below to reset your password.</p>" +
                    "<p><a href=" + link + ">Reset Password</a></p>" +
                    "<p>If you did not request for a password reset, then your account may have been compromised. Please login immediately and secure your account.</p>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        public ActionResult ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return HttpNotFound();

            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                Customer user = db.Customer.Where(a => a.ResetCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPassword model = new ResetPassword();
                    model.ResetCode = id;
                    model.Username = user.Username;
                    return View(model);
                }
                else
                    return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(FormCollection form)
        {
            string username = form["Username"];
            string newPassword = form["NewPassword"];

            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                Customer user = db.Customer.Where(a => a.Username == username).FirstOrDefault();
                user.Password = Functions.ComputeSHA256(newPassword);
                user.ConfirmPassword = Functions.ComputeSHA256(newPassword);
                db.SaveChanges();
            }
            MessageBox.Show("Password successfully reset! You may now login.");
            Session["User"] = "guest";
            Session["Cart"] = new Dictionary<int, int>();
            Session["CartQty"] = 0;

            return RedirectToAction("Login", "Home");
        }       
    }
}