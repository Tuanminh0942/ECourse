using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.Admin.Model;
using WebApplication1.Common;
using WebApplication1.Models.DAO;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new AdminDao();
                var result = dao.Login(model.Email,model.Password);
                if (result == 1)
                {
                    var Admin = dao.GetByAdminName(model.Email);
                    var usersession = new AdminLogin();
                    usersession.AdminName = Admin.Email;
                    usersession.AdminID = Admin.ID;
                    Session.Add(CommonConstants.USER_SESSION, usersession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (result == 2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
            }
            return View("Index");
        }
    }
}