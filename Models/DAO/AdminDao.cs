using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.DAO
{
    public class AdminDao
    {
        CodeFunEntities1 db = new CodeFunEntities1();
        public Admin GetByAdminName (string email)
        {
            return db.Admins.SingleOrDefault(x => x.Email== email);
        }
        public int Login(string email, string password)
        {
            var result = db.Admins.SingleOrDefault(x => x.Email == email);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Password.Trim() == password)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }
    }
}