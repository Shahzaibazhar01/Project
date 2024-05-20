using Newtonsoft.Json;
using Project.BusinessLogic;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User model)
        {
            var res = BL_Login.UserLogin(model);
            if (res!=null)
            {
                var temp = (BusinessObject.User)res;
                BaseController.WDMSUSER = temp;
                return Redirect("/Home/Index");
            }
            if(!string.IsNullOrEmpty(model.UPassword) && !string.IsNullOrEmpty(model.UUserName))
                ViewBag.Failed = "Invalil Credentials";
            return View("Index");
        }
        public ActionResult Logout(string redirecturl)
        {
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            BaseController.WDMSUSER = null;
            string url = "/Login/Index";
            return Redirect(url);
        }

        public ActionResult GetSessionExpiredCode()
        {
            Response.StatusCode = 403;
            return Json(new { Status = "SessionExpired" }, JsonRequestBehavior.AllowGet);
        }
        public string GetUserType()
        {
            string result = JsonConvert.SerializeObject(BaseController.WDMSUSER, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None });
            return result;
        }
        public ActionResult Signup()
        {
            ViewBag.Role = BL_Login.GetRoles();
            return View();
        }
        [HttpPost]
        public ActionResult Signup(User model)
        {
            if (model.URole == null)
            {
                ViewBag.Role = BL_Login.GetRoles();
                ViewBag.RoleError = "Select Role First";
                return View("Signup");
            }
            var res = BL_Login.CreateAccount(model);
            if (res.Equals("Success"))
                return Redirect("/Login/Index");
            else
                ViewBag.Failed = res;
            return View("Signup");
        }
    }
}