using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.GiangVien.Controllers
{
    public class EssayController : Controller
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        // GET: GiangVien/Essay
        public ActionResult Index()
        {
            var teacher = Session["Teacher"] as Teacher;
            var model = db.Essays.Where(x => x.Session.Course.IDTeacher == teacher.ID).OrderByDescending(x => x.DateCmt).ToList();
            return View(model);
        }
        public ActionResult Update( int ID)
        {
            var BaiTap = db.Essays.Find(ID);
            return View(BaiTap);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update (Essay entity)
        {
            var Baitap = db.Essays.Find(entity.ID );
            Baitap.RepDate = DateTime.Now;
            Baitap.Comment= entity.Comment;
            Baitap.Score= entity.Score;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}