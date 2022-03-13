using PetsProject.Models;
using PetsProject.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetsProject.Controllers
{
    [Authorize(Roles = "ADMIN")]
    // Trang web quan ly tat ca don hang
    public class Admin_AddressController : Controller
    {
            ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
        // GET: Admin_Address
        public ActionResult Index()
        {
            List<bill> listBill = db.bills.ToList();
            ListAdminAddressModel listBillModel = new ListAdminAddressModel();
            if(listBill.Count>0)
                foreach(bill b in listBill)
                {
                    if (listBillModel.listCancelled == null)
                        listBillModel.listCancelled = new List<AdminAddressModel>();
                    if (listBillModel.listReceived == null)
                        listBillModel.listReceived = new List<AdminAddressModel>();
                    if (listBillModel.listOnTheWay == null)
                        listBillModel.listOnTheWay = new List<AdminAddressModel>();
                    if (listBillModel.listDelivered == null)
                        listBillModel.listDelivered = new List<AdminAddressModel>();

                    AdminAddressModel aam = new AdminAddressModel();
                    aam.id = b.id;
                    String fullname = db.accounts.Find(b.username).fullname;
                    aam.name = fullname;
                    aam.payment = b.payment;
                    aam.status = b.status;
                    aam.totalcost = (int)b.totalcost;
                    String address = db.address_Book.Find(b.idaddress).address;
                    aam.address = address;
                    aam.datetime = b.time.ToString("HH:mm MMMM dd, yyyy");

                    if (aam.status == "RECEIVED ORDER")
                        listBillModel.listReceived.Add(aam);
                    else if (aam.status == "ON THE WAY")
                        listBillModel.listOnTheWay.Add(aam);
                    else if (aam.status == "CANCELLED")
                        listBillModel.listCancelled.Add(aam);
                    else if (aam.status == "DELIVERED")
                        listBillModel.listDelivered.Add(aam);
                }
            return View(listBillModel);
        }

        public ActionResult billDetails(String id)
        {
            bill billDB = db.bills.Find(id);
            List<bill_Details> billDetailsDB = db.bill_Details.Where(x => x.idbill == id).ToList();
            address_Book ab = db.address_Book.Find(billDB.idaddress);
            BillIem bi = new BillIem();
            bi.id = id;
            bi.fullDateTime = billDB.time.ToString("HH:mm dd-MM-yyyy"); ;
            bi.address = ab.address;
            bi.note = billDB.note;
            bi.payment = billDB.payment;
            bi.status = billDB.status;
            bi.totalCost = billDB.totalcost.ToString("#,## VNĐ");
            List<BillDetails> billDetails = new List<BillDetails>();
            foreach(var item in billDetailsDB)
            {
                BillDetails bd = new BillDetails();
                pet pet = db.pets.Find(item.idproduct);
                bd.product = pet.name;
                bd.price = pet.price.ToString("#,## VNĐ");
                bd.quantity = item.quantity;
                billDetails.Add(bd);
            }
            bi.listBillDetails = billDetails;

            ViewBag.bill = bi;
            return View();
        }

        [HttpPost]
        public JsonResult saveStatusBill(AJAXRequest req)
        {
            ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
            bill bill = db.bills.Find(req.id);
            bill.status = req.status;
            db.Entry(bill).State = EntityState.Modified;
            db.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Da luu thanh cong idBill "+req.id+" voi trang thai la "+req.status);
            return Json("");
        }

        [HttpPost]
        public JsonResult sendMail(AJAXRequest req)
        {
            ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
            bill bill = db.bills.Find(req.id);
            string status = req.status;
            account acc = db.accounts.Find(bill.username);
            System.Diagnostics.Debug.WriteLine("Chuan bi gui email den "+acc.email);
            SendMail.sendMail(acc.email, GenerateInfo.getInfoBill(bill), status);
            return Json("");
        }
    }
}