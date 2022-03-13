using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetsProject.Controllers
{
    [AllowAnonymous]
    public class ChangePassController : Controller
    {
        // GET: ChangePass
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string newpassword, string renewpassword)
        {
            var userDao = new Models.DAO.userDao();
            if(newpassword == renewpassword)
            {
                var username = Session["username"].ToString();
                if(userDao.changNewPass(username,newpassword)) {
                    return RedirectToAction("SignIn", "Login");
                }else
                {
                    ViewBag.error = "Error. Please try again!";

                }

            }else
            {
                ViewBag.error = "Password and newpassword need same";
            }
            return View();

        }
    }
}