using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;


namespace WebApplication1.Models.DAO
{
    public class UserDao
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        public bool Insert (User entity)
        {
            try
            {
                var user = new User();
                user.Email = entity.Email;
                user.Password = entity.Password;
                user.FullName = entity.FullName;
                user.Birth = entity.Birth;
                user.Phone = entity.Phone;
                user.Addess = entity.Addess;
                user.Date = DateTime.Today;
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update (User entity) 
        {
            try 
            {
                var user = db.Users.Find(entity.ID);
                user.FullName= entity.FullName;
                if(!string.IsNullOrEmpty(user.Password)) 
                {
                    user.Password = entity.Password;
                }
                user.Email = entity.Email;
                user.Phone= entity.Phone;
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public bool Delete (int id) 
        {
            try
            {
                var user =db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public User GetByUserName(string userName)
        {
            return db.Users.SingleOrDefault(x => x.Email == userName);
        }
        public IEnumerable<User> ListAllPaging (string searchString, int page, int pageSize)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.FullName.Contains(searchString));
            }
            return model.OrderByDescending(x => x.FullName).ToPagedList(page, pageSize);
        }
        public User ViewDetail(int ID)
        {
                return db.Users.Find(ID);
        }

        public int Login (string Email, string Password )
        {
            var Result = db.Users.SingleOrDefault(x => x.Email== Email);
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

        public bool checkEmail (string Email)
        {
            if(db.Users.Count(x => x.Email == Email) > 0) 
            {
            return true;
            }
            else return false;
        }
    }
}