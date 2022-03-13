using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetsProject.Models;

namespace PetsProject.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();



        public ActionResult SignUp()
        {
            return View();
        }
    


        [HttpGet]
        public JsonResult checkUserName(string username)
        {
            var userDAO = new Models.DAO.userDao();
            bool existUser = userDAO.checkExistUserName(username);
            string respone = "";
            if(existUser == true)
            {
                respone = "exist";

            }else
            {
                respone = "valid";
            }
            return  Json(respone,JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(string fullName,DateTime birthday, string phone, string email, string address, string username, string password, string repassword)
        {
                var userDAO = new Models.DAO.userDao();
                bool existUser = userDAO.checkExistUserName(username);
                if (existUser == true)
                {
                    ViewBag.existUser = "UserName đã tồn tại!";
                }
                if(!password.Equals(repassword) ){

                ViewBag.errorPass = "Mật khẩu nhập lại không giống nhau!";
            }else
            {
                if(userDAO.signUp(fullName,birthday,address,email,phone,username,password))
                {
                    return RedirectToAction("SignIn", "Login");
                }else
                {
                    return View();
                }
            }
           
              
            
         

            return View();
        }
    }

}