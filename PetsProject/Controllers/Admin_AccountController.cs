using PetsProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PetsProject.Controllers
{
    [Authorize(Roles ="ADMIN")]
    // Trang web quan ly tai khoan cua toan bo he thong
    public class Admin_AccountController : Controller
    {
        // GET: Admin_Account
        public ActionResult Index()
        {
            
            ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
            List<AccountAdminModel> listAcAdMo = new List<AccountAdminModel>();
            List<APP_USER> list = db.APP_USER.ToList();
            foreach(APP_USER app in list)
            {
                AccountAdminModel acc = new AccountAdminModel();
                acc.username = app.USER_NAME;
                acc.fullname = app.account.fullname;
                acc.role = getRoles(app.USER_NAME);
                if (app.ENABLED == true)
                    acc.status = "ACTIVE";
                else
                    acc.status = "DEACTIVE";
                listAcAdMo.Add(acc);
            }

            return View(listAcAdMo);
        }

        public ActionResult ViewInfo(String id)
        {
            ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
            account acc = db.accounts.Find(id);
            System.Diagnostics.Debug.WriteLine("Username dang doc la: "+id);
            AccountDetailsAdminModel accDetails = new AccountDetailsAdminModel();
            accDetails.username = acc.username;
            accDetails.fullname = acc.fullname;
            accDetails.phone = acc.phone;
            accDetails.email = acc.email;
            accDetails.address = acc.address;
            accDetails.avatar = acc.avatar;
            APP_USER app = db.APP_USER.SingleOrDefault(x => x.USER_NAME == id);
            if (app.ENABLED)
                accDetails.status = "ACTIVE";
            else
                accDetails.status = "DEACTIVE";
            List<address_Book> listAB=db.address_Book.Where(x => x.username == id).ToList();
            accDetails.listAddressBook = new List<string>();
            foreach(var item in listAB)
            {
                accDetails.listAddressBook.Add(item.address);
            }
            return View(accDetails);
        }

        public ActionResult SaveInfo(AccountDetailsAdminModel accDetails)
        {
            ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
            APP_USER app = db.APP_USER.SingleOrDefault(x => x.USER_NAME == accDetails.username);
            if (accDetails.selectStatus == "ACTIVE")
                app.ENABLED = true;
            else if (accDetails.selectStatus == "DEACTIVE")
                app.ENABLED = false;

            // Save to db
            db.Entry(app).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewInfo/" + accDetails.username);
        }

        public string getRoles(String username)
        {
            string result = "";
            ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
            APP_USER acc = db.APP_USER.SingleOrDefault(x => x.USER_NAME == username);
            List<USER_ROLE> listRole = db.USER_ROLE.Where(x => x.USER_ID == acc.USER_ID).ToList();
            foreach(USER_ROLE user in listRole)
            {
                result += user.APP_ROLE.ROLE_NAME + " ";
            }
            return result;
        }
    }
}