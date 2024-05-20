using Project.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class BaseController : Controller
    {
        public static User WDMSUSER { get; set; }
        protected string RequestGUID { get; private set; } = "";

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (controllerName == "Login" && actionName == "Index" && WDMSUSER==null)
                return;

            if (WDMSUSER == null)
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.Redirect(Url.Action("Index", "Login"));
                filterContext.HttpContext.Response.End();

                return;
            }
        }
    }
}