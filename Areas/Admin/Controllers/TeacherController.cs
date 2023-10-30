using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DAO;

namespace WebApplication1.Areas.Admin.Controllers
{
    
    public class TeacherController : BaseController
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        // GET: Admin/Teacher
        public ActionResult Index(string SearchString, int page =1, int PageSize = 200)
        {
            var dao = new TeacherDao();
            var model = dao.ListAllPaging(SearchString, page, PageSize);
            ViewBag.SearchString = SearchString;
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var user = db.Teachers.Find(id);
            var LstCourse = db.Courses.Where(x => x.IDTeacher == id).ToList();
            foreach(var item in LstCourse)
            {
                var DanhGia = db.Reviews.Where(c => c.IDCourse == id).ToList();
                foreach (var d in DanhGia)
                {
                    db.Reviews.Remove(d);
                }
                var LstJoin = db.Joins.Where(a => a.IDCourse == item.ID).ToList();
                foreach(var ThamGia in LstJoin)
                {
                    db.Joins.Remove(ThamGia);
                }
                var LstSession =db.Sessions.Where(x => x.courseID== item.ID).ToList();
                foreach(var entity in LstSession)
                {
                    var LstEssay = db.Essays.Where(b => b.IDSession == entity.ID).ToList() ;
                    foreach(var BaiTap in LstEssay)
                    {
                        if(BaiTap.File != null)
                        {
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/files"), BaiTap.File));
                            db.Essays.Remove(BaiTap);
                        }    
                    }    
                    if(entity.File != null)
                    {
                        System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/files"), entity.File));
                        db.Sessions.Remove(entity);
                    }
                }
                if (item.Images != null)
                {
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/img/Course"), item.Images));
                }
                db.Courses.Remove(item);
            }   
            db.Teachers.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");   
        }
        public ActionResult Edit(int id)
        {
            var edit = new TeacherDao();
            var teacherEdit = edit.ViewDetail(id);
            return View(teacherEdit);
        }
        [HttpPut]
        public ActionResult Edit(Teacher model)
        {
            TeacherDao GiangVien = new TeacherDao();
            if (GiangVien.Update(model) == true) 
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("",GiangVien.ThongBao.ToString());
                return View(model);
            }    
        }
        public ActionResult AddGV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGV(Teacher model)
        {
            TeacherDao GiangVien = new TeacherDao();
            if(GiangVien.Insert(model)==true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", GiangVien.ThongBao);
                return View();
            }
            
        }
    }
}