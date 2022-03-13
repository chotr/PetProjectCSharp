using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetsProject.Models;
using System.Collections;
using System.Threading;

namespace PetsProject.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
        public ActionResult Index()
        {
            // Lay ra 1 tai khoan trong db dua tren username
            /*ProjectWebBanThuCungEntities db = new ProjectWebBanThuCungEntities();
            account acc = db.accounts.SingleOrDefault(x => x.username == "bedang");
            return View(acc);*/


            // Lay ra danh sach san pham tu db
            var lstSP = db.pets.ToList();
            //gán vao ViewBag
            ViewBag.ListSP = lstSP;

            return View();
        }

        public ActionResult MenuPartial()
        {
            var lstSanPham = db.pets;

            return PartialView(lstSanPham);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}