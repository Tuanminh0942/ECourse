using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DAO;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class EssayController : Controller
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        // GET: Admin/Essay
        public ActionResult Index()
        {
            var model = db.Essays.OrderByDescending(x => x.DateCmt).ToList();
            return View(model);
        }
        public ActionResult Detail(int id)
        {
            var Baitap = db.Essays.Find(id);
            return View(Baitap);
        }
    }
}