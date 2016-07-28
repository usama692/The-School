using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TheSchool.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TheSchool.Controllers
{
    public class LoginController : Controller
    {
        TheSchoolEntities2 db = new TheSchoolEntities2();

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", true);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel login_obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var users = db.Sp_Login(login_obj.UserName, login_obj.Password).ToList();
                    Session["UserName"] = users.FirstOrDefault().name;
                    Session["Designation"] = users.FirstOrDefault().designation;

                    int? isadmin = users.FirstOrDefault().isAdmin;

                    if (users.Count > 0 && isadmin == 1)
                    {
                        return RedirectToAction("VWRedirectAdmin", "Login");
                    }
                    else if (users.Count > 0 && isadmin == 0)
                    {
                        return RedirectToAction("VWRedirect", "Login");
                    }
                }
                else
                {
                    return View(login_obj);
                }

                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Msg = login_obj.UserName.ToUpper() + " is NOT Active User".ToString();
                return View();
            }
        }

        [HttpGet, OutputCache(NoStore = true, Duration = 1)]
        public ActionResult VWRedirect()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public ActionResult VWRedirectAdmin()
        {
            return View();
        }

        [HttpGet]
        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel login_obj)
        {
            try
            {

                HttpPostedFileBase photo = Request.Files["productavatar"];
                if (photo.FileName != "")
                {
                    photo.SaveAs(Server.MapPath("~/Assets/Images/Uploads/" + login_obj.UserName + ".jpg"));
                }

                db.Sp_Reg_Insert(login_obj.Name, login_obj.Email, login_obj.UserName, login_obj.Password, login_obj.Designation, photo.FileName);
                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(login_obj);
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgetPassword login_obj)
        {
            var users = db.Sp_RecoverPassword(login_obj.UserName, login_obj.Email).ToList();
            if (users.Count == 0)
            {
                //ViewBag.Msg = "USER IS NOT ACTIVE";
                return View();
            }
            Session["Username"] = users.FirstOrDefault().username;
            Session["Email"] = users.FirstOrDefault().email;
            Session["Password"] = users.FirstOrDefault().password;
            return RedirectToAction("RecoverEmail");
        }

        [HttpGet]
        public ActionResult RecoverEmail()
        {
            var user = Session["UserName"];

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var finduser = Session["Username"];
            var findemail = Session["Email"];
            var findpwd = Session["Password"];

            if (finduser == null)
            {
                return RedirectToAction("Login");
            }

            MailViewModel mm = new MailViewModel();
            mm.From = "theschoolgrw@gmail.com";
            mm.To = findemail.ToString();
            mm.Subject = "Password Recovery";
            mm.Body = "Your Password is : " + findpwd + " .... It is recommended that Change your Password after getting yourself  LOGGED IN !!";

            MailMessage mail = new MailMessage();
            mail.To.Add(mm.To);
            mail.From = new MailAddress(mm.From);
            mail.Subject = mm.Subject;
            string Body = mm.Body;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            ("theschoolgrw", "Pakistan@123");// Enter seders User name and password
            smtp.EnableSsl = true;
            smtp.Send(mail);
            ViewBag.mailmsg = "Your Password has been sent to your email account";
            return View("Login");
        }
        [HttpGet]
        public ActionResult ChangePwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePwd(ChangePwdModel login_obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Sp_ChangePwd(login_obj.UserName, login_obj.Password, login_obj.NewPassword);
                    ViewBag.Success = "Password Successfully Changed";
                    return RedirectToAction("Login");
                }

                return View(login_obj);
            }
            catch (Exception ex)
            {
                ViewBag.msg = "INVALID USER";
                return RedirectToAction("ChangePwd");
            }
        }
    }
}