using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Common
{
    [Serializable]
    public class AdminLogin
    {
        public long AdminID { get; set; }
        public string AdminName { get; set; }
        public string GroupID { get; set; }
    }
}