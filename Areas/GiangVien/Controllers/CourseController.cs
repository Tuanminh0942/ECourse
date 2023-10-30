using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.GiangVien.Controllers
{
    public class CourseController : Controller
    { 
        CodeFunEntities1 db = new CodeFunEntities1();
        // GET: GiangVien/Course
        public ActionResult Index()
        {
            var teacher = Session["Teacher"] as Teacher;
            var model = db.Courses.Where(x => x.IDTeacher == teacher.ID).OrderByDescending(x => x.Date).ToList();
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var KhoaHoc = db.Courses.Find(id);
            var DanhGia = db.Reviews.Where(c => c.IDCourse ==id).ToList();
            foreach (var d in DanhGia)
            {
                db.Reviews.Remove(d);
            }    
            var LstJoin = db.Joins.Where(a => a.IDCourse == id).ToList();
            foreach (var ThamGia in LstJoin)
            {
                db.Joins.Remove(ThamGia);
            }
            var LstSession = db.Sessions.Where(x => x.courseID == id).ToList();
            foreach (var entity in LstSession)
            {
                if (entity.File != null)
                {
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/files"), entity.File));
                    db.Sessions.Remove(entity);
                }
            }
            if (KhoaHoc.Images != null)
            { 
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/img/Course"), KhoaHoc.Images)); 
            }
            db.Courses.Remove(KhoaHoc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCourse(Course entity, HttpPostedFileBase Images)
        {
            var teacher = Session["teacher"] as WebApplication1.Models.Teacher;
            var course = new Course();
            course.Titles = entity.Titles;
            course.Description = entity.Description;
            course.IDTeacher = teacher.ID;
            course.Date = DateTime.Now;
            course.IDCategory = entity.IDCategory;

            //Thêm hình ảnh
                if (Images != null)
            {
                string filename = Images.FileName;
                var path = Path.Combine(Server.MapPath("~/Assets/img/Course/"), filename);
                Images.SaveAs(path);
                course.Images = filename;
            }

            db.Courses.Add(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Update(Course entity)
        {
            var course = db.Courses.Find(entity.ID);
            return View(course);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Course entity, HttpPostedFileBase Images)
        {
            try
            {
                var course = db.Courses.Find(entity.ID);
                course.Titles = entity.Titles;
                course.Description = entity.Description;
                course.IDCategory = entity.IDCategory;

                if (Images != null && course.Images != Images.FileName)
                {
                    //Xóa file cũ
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/img/Course"), course.Images));
                    //Thêm hình ảnh
                    string filename = Images.FileName;
                    var path = Path.Combine(Server.MapPath("~/Assets/img/Course/"), filename);
                    Images.SaveAs(path);
                    course.Images = filename;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }
        public ActionResult HocVien(int id)
        {
            var teacher = Session["Teacher"] as Teacher;
            var model = db.Joins.
                Where(x => x.Course.Teacher.ID == teacher.ID && x.IDCourse == id).ToList();
            ViewBag.HocVien = model;
            return View();
        }
    }
}