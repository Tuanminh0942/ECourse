using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication1.Common;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = (AdminLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "login", action = "Index", area = "Admin" }));
            }
            base.OnActionExecuted(filterContext);
        }
        protected void SetAlert (string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type== "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            if (type == "Error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}