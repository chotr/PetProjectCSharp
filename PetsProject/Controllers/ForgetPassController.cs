using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net.Mail;
using System.Net;

namespace PetsProject.Controllers
{
    [AllowAnonymous]
    public class ForgetPassController : Controller
    {
        // GET: ForgetPass
        public ActionResult Forget()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forget(string username, string email)
        {
            var userDao = new Models.DAO.userDao();

            if(userDao.IsValidEmail(email))
            {
              if(userDao.checkExistAccount(username,email))
                {

                    long otpCode = userDao.otpCode();
                    Session.Add("otp", otpCode);
                    Session.Add("username", username);
                    MailMessage mail = new MailMessage();
                    mail.To.Add(email);
                    mail.From = new MailAddress("gallmount1899@gmail.com");
                    mail.Subject = "OTP code";
                    string body = "This is your OTP: " + otpCode;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("gallmount1899@gmail.com", "nhs191118");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    return RedirectToAction("OTPValidation", "OTPConfirm");
                }else
                {
                    ViewBag.errorEmailFormat = "Username or email incorrect";
                }
           
            }
            else
            {
                ViewBag.errorEmailFormat = "Inccorect email format";
              
            }
            return View();

        }

    }
}