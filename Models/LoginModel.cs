using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Mời bạn nhập email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mời bạn nhập mật khẩu")]
        public string Password { get; set; }
    }
}