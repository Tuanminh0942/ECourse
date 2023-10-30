using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.GiangVien.Controllers
{
    public class HomeController : Controller
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        // GET: GiangVien/Home
        public ActionResult Index()
        {
            var teacher = Session["Teacher"] as Teacher;
            //Đếm số khóa học
            ViewBag.Count_Course = db.Courses.Count(x => x.IDTeacher == teacher.ID);

            //Số bài giảng
            ViewBag.Count_CourseDetail = db.Sessions.Count(x =>x.Course.IDTeacher == teacher.ID);

            //Số học viên
            ViewBag.Count_User = db.Joins.Count(x => x.Course.IDTeacher == teacher.ID);

            //Số giảng viên
            ViewBag.Count_Essays = db.Essays.Count(x => x.Session.Course.IDTeacher == teacher.ID);

            //Thống kê đánh giá hôm nay
            ViewBag.Review_today = db.Reviews.Where(x => x.Course.IDTeacher ==teacher.ID &&x.Date == DateTime.Today).Count();

            //Thống kê tổng đánh giá
            ViewBag.Review_Total = db.Reviews.Count(x => x.Course.IDTeacher == teacher.ID);

            //Học viên mới đăng ký
            ViewBag.New_User = db.Joins.Where(x => x.Course.IDTeacher == teacher.ID&& x.User.Date==DateTime.Today).Count();

            //Danh sách khóa học
            var KhoaHoc = db.Courses.Where(x=> x.IDTeacher == teacher.ID).OrderByDescending(x => x.Date).ToList();
            ViewBag.KhoaHoc = KhoaHoc;
            ViewBag.Title = "Trang chủ";
            return View();
        }
        public ActionResult Logout() 
        {
            Session["Teacher"] = null;
            return Redirect("/Home/LOGIN"); 
        }
    }
}