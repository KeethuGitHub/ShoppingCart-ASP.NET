using System.Web.Mvc;

using ShoppingWebApp.Models;

namespace ShoppingWebApp.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Index()
        {
            if (Session["User"] == null || (string)Session["User"] == "guest")
                return RedirectToAction("Index", "Home");

            Customer customer = (Customer)Session["Customer"];

            return View(customer);
        }
    }
}