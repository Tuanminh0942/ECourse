using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebApplication1.Models;
using WebApplication1.Models.DAO;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        CodeFunEntities1 dbCourse = new CodeFunEntities1();
        // GET: Course
        public ActionResult Index(int? CategoryID, string searchstring = "" , int Page = 1)
        {
            int CoursePerPage = 6;
            //Lấy danh sách//
            var categories = dbCourse.Categories.ToList();
            ViewBag.Categories = categories ;
            List<Course> danhsachCourse ;
            if (searchstring != "")
            {
                danhsachCourse = dbCourse.Courses.Where(x => x.Titles.Contains(searchstring)).ToList();
            }
            else
            danhsachCourse = dbCourse.Courses.ToList();
            var KhoaHoc = danhsachCourse;
            
            if(CategoryID !=null)
            {
                KhoaHoc = danhsachCourse.Where(n => n.IDCategory == CategoryID).ToList();
            };
            //Đánh số trang
            int TotalPage = KhoaHoc.Count()/ CoursePerPage;
            if (TotalPage * CoursePerPage < KhoaHoc.Count())
                TotalPage++;
            ViewBag.TotalPage = TotalPage;
            //Lấy số khóa học mỗi trang
            KhoaHoc = KhoaHoc.Skip((Page - 1) * CoursePerPage).Take(CoursePerPage).ToList();
            var DanhsachKH = KhoaHoc.Select(i => new List<string>
            {
                i.Titles,
                i.Images,
                i.Date.Value.ToString("dd-MM-yyyy"),
                dbCourse.Teachers.Find(i.IDTeacher).FullName,
                i.ID.ToString(),
                i.IDCategory.ToString(),
                i.IDTeacher.ToString(),
            }).ToList();
            ViewBag.KhoaHoc = DanhsachKH;
            return View();
        }

        public ActionResult Details(int ID)
        {
            CodeFunEntities1 db = new CodeFunEntities1();
            //Lấy chi tiết khóa học//
            var ChiTietKhoaHoc = dbCourse.Courses.Where(i => i.ID == ID).Select(i => new List<string>
            {
                i.Titles,
                i.Images,
                i.Date.ToString(),
                i.Teacher.FullName,
                i.ID.ToString(),
                i.IDCategory.ToString(),
                i.Description,
                i.Teacher.Email,
                i.Teacher.Phone,
            }).FirstOrDefault();

            //Lấy tên danh sách bài giảng//
            var DanhSachBaiHoc = dbCourse.Sessions.Where(n => n.courseID == ID).ToList();
            //Truyền chi tiết khóa học//
            ViewBag.ChiTietKhoaHoc = ChiTietKhoaHoc;
            //Truyền danh sách bài giảng//
            ViewBag.DanhSachBaiHoc = DanhSachBaiHoc;
            //Lấy danh sách đánh giá//
            var LstReview = new CourseDao().GetReviews(ID);
            //Truyền danh sách đánh giá//
            ViewBag.LstReview = LstReview;

            var user = Session["User"] as User;
            bool checkJoin = false;
            if (user != null&& db.Joins.Count(x => x.IDCourse == ID && x.IDUser == user.ID) != 0)
                checkJoin= true;
            ViewBag.checkJoin = checkJoin;
            return View();
        }

        public ActionResult BaiGiang (int? ID)
        {
            var user = Session["User"] as User;
            //Lấy bài giảng
            var ChiTietBaiGiang = dbCourse.Sessions.Where(i => i.ID == ID).FirstOrDefault();
            ViewBag.ChiTietBaiGiang = ChiTietBaiGiang;
            var checkJoin = new CourseDao().InsertJoin(user.ID, ChiTietBaiGiang.courseID);
            var GiangVien = dbCourse.Teachers.Where(c => c.ID == dbCourse.Courses.Where(a => a.ID == ChiTietBaiGiang.courseID).Select(a => a.IDTeacher).FirstOrDefault()).FirstOrDefault();
            ViewBag.GiangVien = GiangVien;
            var KhoaHoc = dbCourse.Courses.Where(d => d.ID == ChiTietBaiGiang.courseID).FirstOrDefault();
            ViewBag.KhoaHoc = KhoaHoc;
            //Lấy bình luận bài tập về nhà//
            var Homework = dbCourse.Essays.Where(x=> x.IDSession== ID).OrderByDescending(x=> x.DateCmt).ToList();
            ViewBag.Homework = Homework;
            return View();
        }
        [HttpPost]
        public ActionResult AddHomework(Essay entity)
        {
            HttpPostedFileBase File = Request.Files[0];
            var Baitap = new Essay();
            var newname = Guid.NewGuid();
            if (File != null)
            {
                string FileName = File.FileName;
                string path = Path.Combine(Server.MapPath("~/Assets/files/"), FileName);
                File.SaveAs(path);
                entity.File = FileName;
            }
            Baitap.DateCmt = DateTime.Now;
            Baitap.File= entity.File;
            Baitap.IDUser= entity.IDUser;
            Baitap.IDSession= entity.IDSession;
            Baitap.Score= entity.Score;
            Baitap.RepDate= entity.RepDate;
            Baitap.Detail= entity.Detail;
            Baitap.Comment= entity.Comment;
            dbCourse.Essays.Add(Baitap);
            dbCourse.SaveChanges();
            return Redirect("/course/BaiGiang/" + entity.IDSession);
        }
        [HttpPost]
        public ActionResult AddReview(Review model) 
        {
            CourseDao DanhGia = new CourseDao();
            if (DanhGia.InsertReview(model) == true)
            {
                return Redirect("/course/Details/" + model.IDCourse); ;
            }
            else return View(); 
        }
        public ActionResult DownloadFile(string file)
        {
            string fullName = Server.MapPath("~/Assets/files/" + file);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }
        public ActionResult DownloadDoc(string file)
        {
            string fullName = Server.MapPath("~/Assets/doc/" + file);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
    }
}