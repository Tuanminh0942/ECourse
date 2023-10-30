using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DAO;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class CourseController : BaseController
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        // GET: Admin/Course
        public ActionResult Index(string SearchString, int page = 1, int PageSize = 200)
        {
            var dao = new CourseDao();
            var model = dao.ListAllPaging(SearchString, page, PageSize);
            return View(model);
        }
        public ActionResult ViewChiTiet(int id)
        {
            var edit = new CourseDao();
            var CourseDetail = edit.ViewDetail(id);
            return View(CourseDetail);
        }
        public ActionResult SessionDetail(int id)
        {
            //Lấy bài giảng
            var SessionDetail = db.Sessions.Find(id);
            //var ChiTietBaiGiang = db.Sessions.Where(i => i.ID == id).FirstOrDefault();
            //ViewBag.ChiTietBaiGiang = ChiTietBaiGiang;
            return View(SessionDetail);
        }

        public ActionResult NewCourse()
        {
            var NewCourse = db.Courses.Where(x => DbFunctions.TruncateTime(x.Date)== DbFunctions.TruncateTime(DateTime.Today));
            return View(NewCourse);
        }
    }
}