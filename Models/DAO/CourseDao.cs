using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DAO
{
    public class CourseDao
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        public Course GetByCourseTitle (string courseTitle)
        {
            return db.Courses.SingleOrDefault(x => x.Titles.Contains(courseTitle));
        }

        public IEnumerable<Course> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Course> model = db.Courses;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Titles.Contains(searchString));
            }
            return model.OrderByDescending(x => x.Date).ToPagedList(page, pageSize);
        }
        public Course ViewDetail (int id)
        {
            return db.Courses.Find(id);
        }
        public bool Insert(Course entity)
        {
            try
            {
                if ((entity.Titles == null) ||
                    (entity.Category.CategoryName == null) ||
                    (entity.Description == null) ||
                    (entity.Images == null) ||
                    (entity.Teacher.FullName == null))
                { return false; }
                db.Courses.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var KhoaHoc = db.Courses.Find(id);
                db.Courses.Remove(KhoaHoc);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Course entity)
        {
            var courseUpdate = db.Courses.Find(entity.ID);
            if (courseUpdate != null)
            {
                courseUpdate.Titles = courseUpdate.Titles;
                courseUpdate.Images = courseUpdate.Images;
                courseUpdate.Category.ID = courseUpdate.Category.ID;
                courseUpdate.Description = entity.Description;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Review> GetReviews(int id)
        {
            List<Review> query = db.Reviews.Where(x =>x.IDCourse== id).OrderByDescending(x => x.Date).ToList();
            return query;
        }

        public bool InsertReview(Review entity)
        {
            try
            {
                var DanhGia = new Review();
                DanhGia.ID = entity.ID;
                DanhGia.Description = entity.Description;
                DanhGia.Rate = entity.Rate;
                DanhGia.Date = DateTime.Now;
                DanhGia.IDUser= entity.IDUser;
                DanhGia.IDCourse= entity.IDCourse;
                db.Reviews.Add(DanhGia);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public bool InsertJoin(long id, long courseID)
        {
            if(db.Joins.Count(x => x.IDUser ==id && x.IDCourse ==courseID) ==0)
            {
                var entity = new Join();
                entity.IDCourse = courseID;
                entity.IDUser = id;
                entity.Date = DateTime.Now;
                db.Joins.Add(entity);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        //Get link 
        public string GetLink(string str)
        {
            string link = "";
            int dem = 1;
            for (var i = str.Length - 1; i >= 0; i--)
            {
                if (dem <= 11)
                {
                    link += str[i];
                    dem++;
                }
                else
                {
                    break;
                }
            }
            char[] arr = link.ToCharArray(); // chuỗi thành mảng ký tự
            Array.Reverse(arr); // đảo ngược mảng
            return new string(arr); // trả về chuỗi mới sau khi đảo mảng
        }
        public List<Course> DanhSach()
        {
            return db.Courses.ToList();
        }
        public Course ChiTiet(long ID)
        {
            return db.Courses.Find(ID);
        }
    }
}