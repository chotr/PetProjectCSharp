using PetsProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PetsProject.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
        Security.Security security = new Security.Security();
        static string urlParam = "/";
        // GET: Login
        [HttpGet]
        public ActionResult SignIn()
        {
            urlParam = Request.QueryString["ReturnUrl"];
            if (urlParam == "" || urlParam==null)
                urlParam = "/";
            Debug.WriteLine("URL dang luu: " + urlParam);
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(string USER_NAME, string ENCRYTED_PASSWORD)
        {
            // Mật khẩu truyền vào đã được mã hoá theo MD5
            System.Diagnostics.Debug.WriteLine("Kiem tra dang nhap voi username la: "+USER_NAME);
            var userDAO = new Models.DAO.userDao();
            var user = new Models.APP_USER();
            user.USER_NAME = USER_NAME;
            user.ENCRYTED_PASSWORD = ENCRYTED_PASSWORD;
             if(userDAO.sign(user) == true)
            {
                account acc = db.accounts.Find(USER_NAME);

                // Nguoi dung bat dau dang nhap...
                Session["acc"] = acc;
                FormsAuthentication.SetAuthCookie(USER_NAME, true);

                setCartWhenSignIn();
                if (security.IsUserInRole(USER_NAME, "ADMIN")&&urlParam=="/")
                    return RedirectToAction("Index", "Admin_Product");
                else if (security.IsUserInRole(USER_NAME, "USER") && urlParam == "/")
                    return RedirectToAction("Index", "Home");
                else if(urlParam != "/")
                {
                    return RedirectToLocal(urlParam);
                }
            }
            else
            {
                ViewBag.errorSignin = "The username or password is in correct!";
                return View();
            }

            return View();

        }

        public ActionResult SignOut()
        {
            Session["acc"] = null;
            FormsAuthentication.SignOut();
            clearSessionCart();
            return RedirectToAction("SignIn", "Login");
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public void setCartWhenSignIn()
        {
            List<cart> listCart = Session["cart"] as List<cart>;
            account acc = Session["acc"] as account;
            if (listCart == null)
            {
                listCart = new List<cart>();
                Session["cart"] = listCart;
            }
            if(listCart.Count() == 0)
            {
                listCart.AddRange(db.carts.Where(x => x.username == acc.username).ToList());
            }
            else
            {
                List<cart> listCartInDB= db.carts.Where(x => x.username == acc.username).ToList();
                foreach(var item in listCartInDB)
                {
                    db.carts.Remove(item);
                }
                foreach(var item in listCart)
                {
                    cart cart = new cart(item.id, item.idproduct, item.quantity, acc.username);
                    db.carts.Add(cart);
                }
                db.SaveChanges();

            }

        }
        public void clearSessionCart()
        {
            List<cart> listCart = Session["cart"] as List<cart>;
            if (listCart != null)
                listCart.Clear();
        }
    }
}