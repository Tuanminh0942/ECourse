using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.GiangVien.Controllers
{
    public class UserController : Controller
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        // GET: GiangVien/User
        public ActionResult Index()
        {
            var teacher = Session["Teacher"] as Teacher;
            var model = db.Joins.
                Where(x => x.Course.IDTeacher == teacher.ID).ToList();
            return View(model);
        }
        public ActionResult NewHV() 
        {
            var teacher = Session["Teacher"] as Teacher;
            var model = db.Joins.
                Where(x => x.Course.Teacher.ID == teacher.ID&& x.Date== DateTime.Today).ToList();
            return View(model);
        }
    }
}