using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DAO
{
    public class CategoryDao
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        public List<Category> DanhSach()
        {
            return db.Categories.ToList();
        }
        public Category ChiTiet(long ID)
        {
            return db.Categories.Find(ID);
        }
    }
}