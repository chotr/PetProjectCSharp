using PetsProject.Models;
using PetsProject.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PetsProject.Controllers
{
    [Authorize(Roles = "USER,ADMIN")]
    public class ManageAccountController : Controller
    {
        ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
        // GET: ManageAccount
        public ActionResult Index()
        {
            ManageAccountModel manageAccountModel = new ManageAccountModel();
            showInfoAccount(manageAccountModel);
            return View(manageAccountModel);
        }

        [HttpPost]
        public JsonResult UploadAvatar(HttpPostedFileBase file)
        {
            // Cach 1 - dung vong for chay Request.Files (ko co tham so dau vao)
            /*String username = "minhquang";
             for (int i = 0; i < Request.Files.Count; i++)
             {
                 HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                             //Use the following properties to get file's name, size and MIMEType
                 int fileSize = file.ContentLength;
                 string fileName = file.FileName;
                 string mimeType = file.ContentType;
                 System.IO.Stream fileContent = file.InputStream;
                 //To save file, use SaveAs method
                 string path = Path.Combine(Server.MapPath("~/img/account"), Path.GetFileName(username) + ".jpg");
                 file.SaveAs(path);
             }

             System.Diagnostics.Debug.WriteLine("Upload hinh thanh cong!");
             return Json("Uploaded " + Request.Files.Count + " files"); */

            // Cach 2 - Dung tham so dau vao HttpPostedFileBase file
            account acc = (account)Session["acc"];
           
            // Save image avatar
            if (file != null)
            {
                string path = Path.Combine(Server.MapPath("~/img/account"), Path.GetFileName(acc.username) + ".jpg");
                file.SaveAs(path);
                System.Diagnostics.Debug.WriteLine("Upload hinh thanh cong!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Khong lay duoc hinh anh!");
            }

            return Json("Upload avatar success!"); 
        }

        public ActionResult SaveAvatar(HttpPostedFileBase file)
        {
            account acc = (account)Session["acc"];
            // Save image avatar
            if (file != null)
            {
                string path = Path.Combine(Server.MapPath("~/img/account"), Path.GetFileName(acc.username) + ".jpg");
                file.SaveAs(path);
                System.Diagnostics.Debug.WriteLine("Upload hinh thanh cong!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Khong lay duoc hinh anh!");
            }
            // Save to db
            db.Entry(acc).State = EntityState.Modified;
            db.SaveChanges();

            ManageAccountModel manageAccountModel = new ManageAccountModel();
            showInfoAccount(manageAccountModel);
            return View(manageAccountModel);
        }

        [HttpPost]
        public JsonResult EditBasicInfo(AJAXRequest req)
        {
           
            System.Diagnostics.Debug.WriteLine("Address is: "+req.address);
            account account = (account)Session["acc"];
            account acc = db.accounts.Find(account.username);
            System.Diagnostics.Debug.WriteLine("Fullname moi la: " + req.fullname);
            acc.fullname = req.fullname;
            acc.email = req.email;
            acc.phone = req.phone;
            acc.address = req.address;
            Session["acc"] = acc; // Cap nhat vo session account

            var res = new AJAXRespone();
            res.fullname = acc.fullname;
            res.email = acc.email;
            res.phone = acc.phone;
            res.address = res.address;

            db.Entry(acc).State = EntityState.Modified;
            db.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Da luu thanh cong thong tin nguoi dung " + req.username);

            return Json(res);
        }

        public JsonResult SaveNewPassword(AJAXRequest req)
        {
            account acc = (account)Session["acc"];
            String username = acc.username;
            APP_USER au = db.APP_USER.SingleOrDefault(x => x.USER_NAME == username);
            String OldPassDB = au.ENCRYTED_PASSWORD;
            String oldPassword = CreateMD5.generateToMD5(req.oldPassword);
            System.Diagnostics.Debug.WriteLine("Old password md5: " + oldPassword);
            if (OldPassDB != oldPassword)
            {
                return Json("The old password is incorrect!");
            }
            else
            {
                String newPass = req.newPassword;
                // Encrypt before saving...
                String newPassEcrypted = CreateMD5.generateToMD5(newPass);
                APP_USER appuser=db.APP_USER.SingleOrDefault(x => x.USER_NAME == username);
                appuser.ENCRYTED_PASSWORD = newPassEcrypted;

                db.Entry(appuser).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Saving password successfully!");
            }

        }

        public JsonResult CheckPasswordToDeleteAccount(AJAXRequest req)
        {
            account acc = (account)Session["acc"];
            String username = acc.username;
            APP_USER au = db.APP_USER.SingleOrDefault(x => x.USER_NAME == username);
            String OldPassDB = au.ENCRYTED_PASSWORD;
            String password = CreateMD5.generateToMD5(req.password);
            if (OldPassDB != password)
            {
                return Json("incorrect");
            }
            else
            {
                return Json("correct");
            }
        }

        public JsonResult SendRequestToDeleteAccount()
        {
            account acc = Session["acc"] as account;
            SendMail.sendGoogleMail("ngominhquang9999@gmail.com", "Request for remove account","Dear Admin, We received a request to remove the account with username: "+acc.username+", email: "+acc.email);
            return Json("");

        }

        public JsonResult AddAddressBook(AJAXRequest req)
        {
            account acc = (account)Session["acc"];

            CreateRandomID randomID = new CreateRandomID();
            address_Book ab = new address_Book();
            ab.id = randomID.newIDAddressBook();
            ab.username = acc.username;
            ab.address = req.address;
            ab.status = "";
            System.Diagnostics.Debug.WriteLine("ID Address la: " + ab.id);

            db.address_Book.Add(ab);
            db.SaveChanges();

            return Json(ab.id);
        }

        public JsonResult EditDefaultAddress(AJAXRequest req)
        {

            address_Book abOld = db.address_Book.Find(req.idAddressOld);
            address_Book abNew = db.address_Book.Find(req.idAddressNew);
            abNew.status = "DEFAULT";
            abOld.status = "";

            AJAXRespone res = new AJAXRespone();
            res.idAddressOld = req.idAddressOld;
            res.idAddressNew = req.idAddressNew;

            // Save to db
            db.Entry(abOld).State = EntityState.Modified;
            db.Entry(abNew).State = EntityState.Modified;
            db.SaveChanges();

            return Json(res);
        }

        public void showInfoAccount(ManageAccountModel manageAccountModel)
        {
            account acc = (account)Session["acc"];

            ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
            List<address_Book> listAB = db.address_Book.Where(x => x.username == acc.username).ToList();
            List<bill> listBill = db.bills.Where(x => x.username == acc.username).ToList();
            List<BillIem> listReceivedBill = new List<BillIem>();
            List<BillIem> listOnTheWayBill = new List<BillIem>();
            List<BillIem> listDeliveredBill = new List<BillIem>();
            List<BillIem> listCancelledBill = new List<BillIem>();
            manageAccountModel.fullname = acc.fullname;
            manageAccountModel.email = acc.email;
            manageAccountModel.avatar = acc.avatar;
            manageAccountModel.phone = acc.phone;
            manageAccountModel.address = acc.address;
            manageAccountModel.username = acc.username;
            foreach (var item in listAB)
            {
                if (item.status == "DEFAULT")
                {
                    manageAccountModel.defaultAddress = item.address;
                    break;
                }
            }
            manageAccountModel.listAddress = listAB;

            foreach (var item in listBill)
            {
                BillIem bi = new BillIem();
                bi.totalCost = string.Format("{0:##,#} VND", item.totalcost);
                bi.fullDateTime = item.time.ToString("HH:mm dd-MM-yyyy");
                bi.payment = item.payment;
                bi.note = item.note;
                bi.status = item.status;

                address_Book ab = db.address_Book.Find(item.idaddress);
                bi.address = ab.address;

                List<BillDetails> listBD = new List<BillDetails>();
                List<bill_Details> listBDFromDB = db.bill_Details.Where(x => x.idbill == item.id).ToList();
                foreach (var itemBD in listBDFromDB)
                {
                    BillDetails billDetails = new BillDetails();
                    pet pet = db.pets.Find(itemBD.idproduct);
                    billDetails.product = pet.name;
                    billDetails.quantity = (int)itemBD.quantity;
                    billDetails.price = string.Format("{0:##,#} VND", itemBD.price);
                    listBD.Add(billDetails);
                }
                bi.listBillDetails = listBD;

                if (bi.status == "DELIVERED")
                {
                    listDeliveredBill.Add(bi);
                }
                else if (bi.status == "ON THE WAY")
                {
                    listOnTheWayBill.Add(bi);
                }
                else if (bi.status == "RECEIVED ORDER")
                {
                    listReceivedBill.Add(bi);
                }
                else if (bi.status == "CANCELLED")
                {
                    listCancelledBill.Add(bi);
                }

            }
            manageAccountModel.listReceivedBill = listReceivedBill;
            manageAccountModel.listOnTheWayBill = listOnTheWayBill;
            manageAccountModel.listDeliveredBill = listDeliveredBill;
            manageAccountModel.listCancelledBill = listCancelledBill;
        }

        public JsonResult DeleteDeliveredBill()
        {
            account acc = (account)Session["acc"];

            List<bill> listBill=db.bills.Where(x => x.username == acc.username).ToList();
            foreach(var item in listBill)
            {
                if (item.status == "DELIVERED")
                {
                    List<bill_Details> listBillDetails = db.bill_Details.Where(x => x.idbill == item.id).ToList();
                    foreach (var itemBD in listBillDetails)
                    {
                        db.bill_Details.Remove(itemBD);
                    }
                    db.bills.Remove(item);
                }
            }
            db.SaveChanges();
            return Json("");
        }

        public JsonResult DeleteCancelledBill()
        {
            account acc = (account)Session["acc"];

            List<bill> listBill = db.bills.Where(x => x.username == acc.username).ToList();
            foreach (var item in listBill)
            {
                if (item.status == "CANCELLED")
                {
                    List<bill_Details> listBillDetails = db.bill_Details.Where(x => x.idbill == item.id).ToList();
                    foreach (var itemBD in listBillDetails)
                    {
                        db.bill_Details.Remove(itemBD);
                    }
                    db.bills.Remove(item);
                }
            }
            db.SaveChanges();
            return Json("");
        }
    }

}