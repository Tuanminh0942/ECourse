using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DAO;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LOGIN()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LOGIN(User entity, bool Teacher = false)
        {
            if(Teacher == false) 
            {
                if(ModelState.IsValid)
                {
                    var dao = new UserDao();
                    var result = dao.Login(entity.Email, entity.Password);
                    if (result == 1)
                    {
                        var user = dao.GetByUserName(entity.Email);
                        Session["User"] = user;
                        return RedirectToAction("Index", "Home");
                    }
                    else if (result == 0)
                    {
                        ModelState.AddModelError("", "Tài khoản không tồn tại");
                    }    
                    else if (result == -1) 
                    {
                        ModelState.AddModelError("", "Mật khẩu không đúng");
                    }
                }
                return View("LOGIN");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var dao = new TeacherDao();
                    var result = dao.Login(entity.Email, entity.Password);
                    if (result == 1)
                    {
                        var teacher = dao.GetByTeacherName(entity.Email);
                        Session["Teacher"] = teacher;
                        return Redirect("/GiangVien/Home/Index");
                    }
                    else if (result == 0)
                    {
                        ModelState.AddModelError("", "Tài khoản không tồn tại");
                    }
                    else if (result == -1)
                    {
                        ModelState.AddModelError("", "Mật khẩu không đúng");
                    }
                }

                return View("LOGIN");
            }
        }

        [HttpPost]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.checkEmail(entity.Email);
                if (result == true)
                {
                    ModelState.AddModelError("", "Email đã được dùng để đăng ký tài khoản");
                }
                else
                {
                    new UserDao().Insert(entity);
                    Session["error_login"] = "Đăng ký thành công. Vui lòng đăng nhập để tiếp tục.";
                    return RedirectToAction("Index", "Home");
                }    
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult CourseLst()
        {
            CodeFunEntities1 db= new CodeFunEntities1();
            var user = Session["User"] as User;
            var LstCourse = db.Joins.Where(x => x.IDUser == user.ID).ToList();
            return View(LstCourse);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            CodeFunEntities1 db = new CodeFunEntities1();
            var user = Session["User"] as User;
            var KhoaHoc = db.Joins.Where(x => x.IDCourse ==id && x.IDUser == user.ID).SingleOrDefault();
            var lstEssay = db.Essays.Where(x => x.IDUser == id).ToList();
            foreach (var item in lstEssay)
            {
                if (item.File != null)
                {
                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Assets/files"), item.File));
                }
                db.Essays.Remove(item);
            }
            db.Joins.Remove(KhoaHoc);
            db.SaveChanges();
            return RedirectToAction("CourseLst");
        }
        public ActionResult Info() 
        {
            CodeFunEntities1 db = new CodeFunEntities1();
            var user = Session["User"] as User;
            var userInfo = db.Users.Find(user.ID);
            return View(userInfo);
        }
        [HttpPut]
        public ActionResult Info (User entity)
        {
            CodeFunEntities1 db = new CodeFunEntities1();
            var user = db.Users.Find(entity.ID);
            user.FullName = entity.FullName;
            user.Addess = entity.Addess;
            user.Email = entity.Email;
            user.Password = entity.Password;
            user.Phone = entity.Phone;
            db.SaveChanges();
            Session["User"] = user;
            return RedirectToAction("Index", "Home");
        }
    }
}