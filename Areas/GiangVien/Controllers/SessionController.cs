using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DAO;

namespace WebApplication1.Areas.GiangVien.Controllers
{
    public class SessionController : Controller
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        // GET: GiangVien/Session
        public ActionResult Index(int id)
        {
            var teacher = Session["Teacher"] as Teacher;
            var model = db.Sessions.
                Where(x => x.Course.IDTeacher == teacher.ID && x.courseID == id).ToList();
            var Ma = id;
            return View(model);
        }

        public ActionResult Detail(int id) 
        {
            var BaiGiang = db.Sessions.Find(id);
            return View(BaiGiang);
        }
        public ActionResult Delete (int id)
        {
            var BaiGiang = db.Sessions.Find(id);
            if (BaiGiang.File != null)
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/files"), BaiGiang.File));
            }
            var lstEssay = db.Essays.Where(x => x.IDSession == id).ToList();
            foreach (var item in lstEssay)
            {
                if (item.File != null)
                {
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/files"), item.File));
                }
                db.Essays.Remove(item);
            }
            db.Sessions.Remove(BaiGiang);
            db.SaveChanges();
            return RedirectToAction("/GiangVien/Session/Index/" + BaiGiang.courseID);
        }
        public ActionResult AddBG()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddBG(int ma,Session entity, HttpPostedFileBase File) 
        {
            entity.Link = "https://www.youtube.com/embed/" + new CourseDao().GetLink(entity.Link).Trim();
            if(File != null)
            {
                string filename = File.FileName;
                var path = Path.Combine(Server.MapPath("~/Assets/files/"), filename);
                File.SaveAs(path);
                entity.File = filename;
            }    
            db.Sessions.Add(entity);
            db.SaveChanges();
            return Redirect("/GiangVien/Session/Index/"+ entity.courseID);
        }
        public ActionResult Update(int ID) 
        {
            var BaiGiang = db.Sessions.Find(ID);
            return View(BaiGiang);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Session entity, HttpPostedFileBase File)
        {
            var BaiGiang = db.Sessions.Find(entity.ID);
            BaiGiang.Titles = entity.Titles;
            BaiGiang.Description = entity.Description;
            BaiGiang.Link= "https://www.youtube.com/embed/" + new CourseDao().GetLink(entity.Link).Trim();
            if(File != null)
            {
                //Xóa file cũ
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/files/"), BaiGiang.File));
                //Thêm file mới
                string filename = File.FileName;
                var path = Path.Combine(Server.MapPath("~/Assets/files/"), filename);
                File.SaveAs(path);
                BaiGiang.File = filename;
            }
            db.SaveChanges();
            return Redirect("/GiangVien/Session/Index/" + entity.courseID);
        }
    }
}