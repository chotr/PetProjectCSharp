using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetsProject.Controllers
{
    [AllowAnonymous]
    public class OTPConfirmController : Controller
    {
        // GET: OTPConfirm
        public ActionResult OTPValidation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OTPValidation(int otpcode)

        {
            int otp = Convert.ToInt32(Session["otp"]);
           
           if(otp == otpcode)
            {
                return RedirectToAction("ChangePassword", "ChangePass");
           }else
            {
                ViewBag.errorOTP = "OTP incorrect";
            }

            return View();
        }
    }
}