using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DAO;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User

        CodeFunEntities1 db = new CodeFunEntities1();
        // GET: Admin/Course
        //Thông tin học viên
        public ActionResult Index(string SearchString, int page = 1, int PageSize = 200)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(SearchString, page, PageSize);
            return View(model);
        }
        //Xóa học viên
        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            var LstJoin = db.Joins.Where(a => a.IDUser == id).ToList();
            foreach(var item in LstJoin)
            {
                db.Joins.Remove(item);
            }    
            var lstEssay = db.Essays.Where(x => x.IDUser == id).ToList();
            foreach (var item in lstEssay)
            {
                if (item.File != null)
                {
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/files"), item.File));
                }
                db.Essays.Remove(item);
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Danh sách học viên mới
        public ActionResult NewUser()
        {
            var NewUser = db.Users.Where(x => DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(DateTime.Today));
            return View(NewUser);
        }
    }
}