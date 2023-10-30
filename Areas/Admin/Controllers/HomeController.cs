using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebApplication1.Models;
using WebApplication1.Models.DAO;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            CodeFunEntities1 db = new CodeFunEntities1();
            //Đếm số khóa học
            ViewBag.Count_Course = db.Courses.Count();

            //Số bài giảng
            ViewBag.Count_CourseDetail = db.Sessions.Count();

            //Số học viên
            ViewBag.Count_User = db.Users.Count();

            //Số giảng viên
            ViewBag.Count_Teacher = db.Teachers.Count();

            //Thống kê đánh giá hôm nay
            ViewBag.Review_today = db.Reviews.Where(x=> DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(DateTime.Today)).Count();

            //Thống kê tổng đánh giá
            ViewBag.Review_Total = db.Reviews.Count();

            //Học viên mới đăng ký
            ViewBag.New_User = db.Users.Where(x => DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(DateTime.Today)).Count();

            //Khóa học mới tạo
            ViewBag.New_Course =db.Courses.Where(x => DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(DateTime.Today)).Count();

            //Danh sách khóa học
            var KhoaHoc = db.Courses.OrderByDescending(x => x.Date).ToList();
            ViewBag.KhoaHoc = KhoaHoc;
            ViewBag.Title = "Trang chủ";
            return View();
        }
    }
}