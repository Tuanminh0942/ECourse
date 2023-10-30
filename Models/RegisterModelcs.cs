using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class RegisterModelcs
    {
        [Required(ErrorMessage = "Mời bạn nhập Họ và tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Mời bạn nhập số điện thoại")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Mời bạn nhập email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mời bạn nhập mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Mời bạn nhập địa chỉ")]
        public string Addess { get; set; }
        [Required(ErrorMessage = "Mời bạn nhập ngày sinh")]
        public DateTime Birth { get; set; }
    }
}