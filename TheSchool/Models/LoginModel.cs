using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheSchool.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Name Required !")]
        [Display(Name = "User Name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Pasword")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Display(Name = "Designation")]
        [DataType(DataType.Text)]
        [Required]
        public string Designation { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        [Required]
        public string Email { get; set; }
        public int isActive { get; set; }
        public int isAdmin { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Name Required !")]
        [Display(Name = "User Name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Pasword")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int isActive { get; set; }
        public int isAdmin { get; set; }
    }

    public class ChangePwdModel
    {
        [Required(ErrorMessage = "Name Required")]
        [Display(Name = "User Name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Pasword")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "New Pasword")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }

    public class ForgetPassword
    {
        [Required(ErrorMessage = "Name Required")]
        [Display(Name = "User Name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

    }

    public class MailViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}