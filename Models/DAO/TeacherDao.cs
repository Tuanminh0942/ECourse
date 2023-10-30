using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DAO
{
    public class TeacherDao
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        public string ThongBao = "";
        public bool Insert(Teacher entity)
        { 
            try
        {
                if((entity.FullName ==null)||
                    (entity.Email == null)||
                    (entity.Address == null)||
                    (entity.Password == null)||
                    (entity.Phone == null))
                {
                    ThongBao = "Bạn chưa điền đủ thông tin";
                    return false;
                }    
                db.Teachers.Add(entity);
                db.SaveChanges();
                return true;
        }
            catch (Exception)
                {
                return false;
                }
        }


        public bool Update(Teacher entity)
        {
            var teacherUpdate = db.Teachers.Find(entity.ID);
                if (teacherUpdate != null) 
                {
                    teacherUpdate.FullName = entity.FullName;
                    teacherUpdate.Password = entity.Password;
                    teacherUpdate.Email = entity.Email;
                    teacherUpdate.Phone = entity.Phone;
                    teacherUpdate.Address = entity.Address;
                    db.SaveChanges();
                    return true;
                }
                return false;
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Teachers.Find(id);
                db.Teachers.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Teacher GetByTeacherName(string email)
        {
            return db.Teachers.SingleOrDefault(x => x.Email == email);
        }
        public IEnumerable<Teacher> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Teacher> model = db.Teachers;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.FullName.Contains(searchString));
            }
            return model.OrderByDescending(x => x.FullName).ToPagedList(page, pageSize);
        }
        public Teacher ViewDetail(int ID)
        {
            return db.Teachers.Find(ID);
        }

        public int Login(string Email, string Password)
        {
            var Result = db.Teachers.SingleOrDefault(x => x.Email == Email);
            if (Result == null)
            {
                return 0;
            }
            else
            {
                if (Result.Password.Trim() == Password)
                {
                    return 1;
                }
                else
                    return -1;
            }
        }

    }
}