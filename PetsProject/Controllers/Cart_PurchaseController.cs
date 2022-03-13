using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetsProject.Models;
using PetsProject.Controllers;
using System.Data;
using System.Data.Entity;
using System.Text;
using PetsProject.Utils;
using PayPal.Api;
using PayPalSandbox.Config;

namespace ShoeProject.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
        private PayPal.Api.Payment payment;
        public ActionResult Index()
        {
            return View();
        }
        //Phương thức lấy giỏ hàng
        public List<cart> LayGioHang()
        {
            //Giỏ hàng  đã tồn tại

            List<cart> lstGioHang = Session["cart"] as List<cart>;
            if (lstGioHang == null)
            {
                // Nếu Session giỏ hàng chưa tồn tại thì khởi tạo list giỏ hàng

                lstGioHang = new List<cart>();
                Session["cart"] = lstGioHang;
            }
           
            return lstGioHang;
        }
        //Phương thức tính tổng số lượng
        public int TinhTongSoLuong()
        {
            List<cart> lstGioHang = Session["cart"] as List<cart>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Count();

        }
        //Phương thức tính tổng tiền
        public double TinhTongTien()
        {
            List<cart> lstGioHang = Session["cart"] as List<cart>;
            if (lstGioHang == null)
            {
                return 0;
            }
            if (lstGioHang.Count()==0)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Gio hang da bi xoa!");
                return 0;
            }
            double total = 0;
            foreach(var item in lstGioHang)
            {
                total += item.totalcost;
            }
            return total;
        }
        public ActionResult XemGioHang()
        {
            List<cart> lstGioHang = LayGioHang();
            ViewBag.listCart = lstGioHang;
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            System.Diagnostics.Debug.WriteLine("Đi đến giỏ hàng!");
            return View();
        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        //Thêm giỏ hàng load thông thường có load lại trang
        //Phương thức thêm giỏ hàng thông thường(LOAD LẠI TRANG)
        public ActionResult ThemGioHang(string id)
        {
            String MaSP = id;
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            pet sp = db.pets.SingleOrDefault(n => n.id == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<cart> lstGioHang = LayGioHang();
            //Trường hợp 1 nếu sản phẩm đã tồn tại trong giỏ hàng
            cart spcheck = lstGioHang.SingleOrDefault(n => n.idproduct == MaSP);
            if (spcheck != null)
            {
                //Kiểm tra số lượng tồn trước khi cho khách hàng mua hàng
               
                spcheck.quantity++;
                spcheck.totalcost = spcheck.price * spcheck.quantity;
                // Nếu người dùng đã đăng nhập - update vô db
                if (Session["acc"] != null)
                {
                    account acc = (account)Session["acc"];
                    cart cart = db.carts.SingleOrDefault(x => x.username == acc.username && x.idproduct == spcheck.idproduct);
                    cart.quantity = spcheck.quantity;
                    cart.totalcost = spcheck.totalcost;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("XemGioHang");
            }
            // Nếu sản phẩm chưa tồn tại trong giỏ hàng
            cart itemGH = new cart(MaSP);
            lstGioHang.Add(itemGH);
            // Nếu người dùng đã đăng nhập - thêm vô db
            if (Session["acc"] != null)
            {
                account acc = (account)Session["acc"];
                itemGH.username = acc.username;
                db.carts.Add(itemGH);
                db.SaveChanges();
            }
            else
            {
                itemGH.pet = db.pets.Find(MaSP);
            }
            return RedirectToAction("XemGioHang");
        }
        
        /*AJAX GIỎ HÀNG*/
        public ActionResult ThemGioHangAjax(string MaSP, string quantity)
        {
            System.Diagnostics.Debug.WriteLine("So luong da nhap: "+quantity);
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            pet sp = db.pets.SingleOrDefault(n => n.id == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            //Lấy giỏ hàng
            List<cart> lstGioHang = LayGioHang();
            //Trường hợp 1 nếu sản phẩm đã tồn tại trong giỏ hàng
            cart spcheck = lstGioHang.SingleOrDefault(n => n.idproduct == MaSP);
            if (spcheck != null)
            {
                
                spcheck.quantity++;
                spcheck.totalcost = spcheck.quantity * spcheck.price;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                // Nếu người dùng đã đăng nhập - update vô db
                if (Session["acc"] != null)
                {
                    account acc = (account)Session["acc"];
                    cart cart = db.carts.SingleOrDefault(x => x.username == acc.username && x.idproduct == spcheck.idproduct);
                    cart.quantity = spcheck.quantity;
                    cart.totalcost = spcheck.totalcost;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return PartialView("GioHangPartial");
            }

            cart itemGH = new cart(MaSP);
            System.Diagnostics.Debug.WriteLine("ID gio hang nay la: " + itemGH.id);
            lstGioHang.Add(itemGH);
            //System.Diagnostics.Debug.WriteLine("Them vao gio hang qua AJAX thanh cong voi san pham: " + itemGH.pet.name);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            // Nếu người dùng đã đăng nhập - thêm vô db
            if (Session["acc"] != null)
            {
                account acc = (account)Session["acc"];
                itemGH.username = acc.username;
                db.carts.Add(itemGH);
                db.SaveChanges();
            }
            else
            {
                itemGH.pet = db.pets.Find(MaSP);
            }
            System.Diagnostics.Debug.WriteLine("Them san pham moi vao gio hang...");
            return PartialView("GioHangPartial");
        }
        [HttpPost]
        //Xóa sản phẩm trong giỏ hàng Ajax
        public ActionResult XoaGioHangAjax(string MaSP)
        {
            System.Diagnostics.Debug.WriteLine("Chuẩn bị xoá sản phẩm: "+MaSP);
            //Kiểm tra Session Giỏ hàng tồn tại chưa
            if (Session["cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            pet sp = db.pets.SingleOrDefault(n => n.id == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list Giỏ Hàng từ Session
            List<cart> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm đó có tồn tại trong giỏ hàng hay không
            cart spCheck = lstGioHang.SingleOrDefault(n => n.idproduct == MaSP);
            if (spCheck == null)
            {
               return RedirectToAction("Index", "Home");

            }
            //Xóa item trong giỏ hàng
            lstGioHang.Remove(spCheck);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            // Nếu người dùng đã đăng nhập - xoá trong db
            if (Session["acc"] != null)
            {
                account acc = (account)Session["acc"];
                cart ca = db.carts.SingleOrDefault(x => x.idproduct == spCheck.idproduct && x.username == acc.username);
                db.carts.Remove(ca);
                db.SaveChanges();
            }
            return PartialView("GioHangPartial");
        }
        [HttpGet]
        public ActionResult SuaGioHang(string MaSP)
        {
            //Kiểm tra Session Giỏ hàng tồn tại chưa
            if (Session["cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra sản phẩm có tồn tại trong CSDL hay không
            pet sp = db.pets.SingleOrDefault(n => n.id == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list Giỏ Hàng từ Session
            List<cart> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm đó có tồn tại trong giỏ hàng hay không
            cart spCheck = lstGioHang.SingleOrDefault(n => n.idproduct == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");

            }
            //Lấy list Giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGioHang;
            //Nếu tồn tại rồi
            return View(spCheck);
        }
        //Xử lý cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGiohang(cart itemGH)
        {
           
            //Cập nhật số lượng trong Session Giỏ hàng
            //B1:Lấy List<GioHang> từ Session["GioHang"]
            List<cart> lstGH = LayGioHang();
            //B2:Lấy Sp cần cập nhật từ trong list giỏ hàng
            cart itemGHUpdate = lstGH.Find(n => n.idproduct == itemGH.idproduct);
            //B3: Cập nhật lại số lượng và thành tiền
            itemGHUpdate.quantity = itemGH.quantity;
            itemGHUpdate.totalcost = itemGHUpdate.price * itemGHUpdate.quantity;
            // Nếu người dùng đã đăng nhập - update vô db
            if (Session["acc"] != null)
            {
                account acc = (account)Session["acc"];
                cart ca = db.carts.SingleOrDefault(x => x.idproduct == itemGH.idproduct && x.username == acc.username);
                ca.quantity = itemGHUpdate.quantity;
                ca.totalcost = itemGHUpdate.totalcost;
                db.Entry(ca).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("XemGioHang");
           
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,USER")]
        public ActionResult ThanhToan()
        {
            account act = (account)Session["acc"];
            if (Session["cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<cart> lstGioHang = LayGioHang();
            List<address_Book> listAB = db.address_Book.Where(x => x.username == act.username).ToList();
            string addressDefault = db.address_Book.SingleOrDefault(x => x.username == act.username && x.status == "DEFAULT").address;
            act.address = addressDefault;
            ViewBag.acc = act;
            ViewBag.listAB = listAB;
            ViewBag.TongTien = TinhTongTien();
            ViewBag.payment = new SelectList(LoadPayment());
            ViewBag.DHStatus = "RECEIVED ORDER";   
            
            return View(lstGioHang);
        }
        /*[ValidateInput(false)]*/
        [HttpPost]
        [Authorize(Roles = "ADMIN,USER")]
        public ActionResult ThanhToan(FormCollection f)
        {
            Data.payment = f["payment"];
            Data.address = f["address"];
            Data.note = f["note"];
            Data.status = f["status"];
            //Kiểm tra Session Giỏ hàng tồn tại chưa
            if (Session["cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (f["payment"] == "CASH")
            {
                return RedirectToAction("purchaseProcess");
            } else if (f["payment"] == "PAYPAL")
            {
                return RedirectToAction("PaymentWithPaypal");
            }
            return RedirectToAction("errorPage");
        }

        // Thanh toán qua PayPay - Sandbox
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfig.GetAPIContext();
            
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
            if (string.IsNullOrEmpty(payerId))
            {
                //this section will be executed first because PayerID doesn't exist  
                //it is returned by the create function call of the payment class  
                // Creating a payment  
                // baseURL is the url on which paypal sendsback the data.  
                string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                //here we are generating guid for storing the paymentID received in session  
                //which will be used in the payment execution  
                var guid = Convert.ToString((new Random()).Next(100000));
                //CreatePayment function gives us the payment approval url  
                //on which payer is redirected for paypal account payment  
                var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                //get links returned from paypal in response to Create function call  
                var links = createdPayment.links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    Links lnk = links.Current;
                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.href;
                    }
                }
                // saving the paymentID in the key guid  
                Session.Add(guid, createdPayment.id);
                return Redirect(paypalRedirectUrl);
            }
            else
            {
                // This function exectues after receving all parameters for the payment  
                var guid = Request.Params["guid"];
                try {
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will petw payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("errorPayPalPayment");
                    }
                }catch(Exception e)
                {
                    return RedirectToAction("purchaseProcess");
                }
                }
            
            //on successful payment, petw success page to user.  
            return RedirectToAction("purchaseProcess");
        }
       
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "your generated invoice number", //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
        public ActionResult purchaseProcess()
        {
            account act = Session["acc"] as account;
            //Thêm đơn hàng
            var generator = new GioHangController();
            bill b = new bill();
            b.id = "BI" + generator.RandomString(2);
            b.username = act.username;
            b.time = DateTime.Now;
            b.note = Data.note;
            b.status = Data.status;
            b.payment = Data.payment;
            string addressForm = Data.address.ToLower().Trim();
            address_Book ab = db.address_Book.SingleOrDefault(x => x.username == act.username && x.address.ToLower() == addressForm);
            // Tu them 1 dia chi nguoi dung moi (da ghi trong form) khi khong tim thay trong db AB
            if (ab == null)
            {
                CreateRandomID cr = new CreateRandomID();
                address_Book newAB = new address_Book();
                newAB.id = cr.newIDAddressBook();
                newAB.username = act.username;
                newAB.address = Data.address;
                newAB.status = "";
                db.address_Book.Add(newAB);
                b.idaddress = newAB.id;
            }
            else
            {
                b.idaddress = ab.id;
            }
            b.totalcost = (int)(int?)TinhTongTien();
            db.bills.Add(b);
            db.SaveChanges();/*Phát sinh mã DDH sau khi lưu*/
            //Thêm chi tiết đơn hàng
            List<cart> lstGioHang = LayGioHang();
            foreach (var item in lstGioHang)
            {
                bill_Details ctdh = new bill_Details();
                ctdh.id = "BD" + generator.RandomString(2);
                ctdh.idbill = b.id;
                ctdh.idproduct = item.idproduct;
                ctdh.price = (int)(int?)item.price;
                ctdh.quantity = item.quantity;
                ctdh.totalcost = (int)(int?)item.totalcost;
                db.bill_Details.Add(ctdh);

            }

            // Gửi email thông báo đến người mua
            // Xoá giỏ hàng trong db
            List<cart> listCart = db.carts.Where(x => x.username == act.username).ToList();
            foreach (var item in listCart)
            {
                db.carts.Remove(item);
            }
            db.SaveChanges();
            Session["cart"] = null;
            SendMail.sendMail(act.email, GenerateInfo.getInfoBill(b), "RECEIVED ORDER");
            return RedirectToAction("ThanhToanThanhCong", new { idBill = b.id });
        }
        public ActionResult ThanhToanThanhCong(String idBill)
        {
            bill item = db.bills.Find(idBill);
            BillIem bi = new BillIem();
            bi.id = idBill;
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
            ViewBag.bill = bi;

            return View();
        }

       
        public ActionResult errorPayPalPayment()
        {
            return View();
        }

        public List<string> LoadPayment()
        {
            List<string> lstPayment = new List<string>();
            lstPayment.Add("CASH");
            lstPayment.Add("PAYPAL");
            return lstPayment;
        }
        private readonly Random _random = new Random();
        // Generates a random string with a given size.
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):   
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);              //4 letter
            }
            builder.Append(RandomNumber(100, 999));//3 number

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

    }
}