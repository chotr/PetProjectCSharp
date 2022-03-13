using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetsProject.Models;
using System.Net;
/*Thư viện phân trang*/
using PagedList;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PetsProject.Controllers
{
    [AllowAnonymous]
    public class SanPhamController : Controller
    {
        // GET: SanPham
        ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
        //Xây dựng trang danh sách sản phẩm
        public ActionResult SanPham(int?page)
        {
            //Load danh sách sản phẩm
            var lstSP = db.pets;
            if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }

            //Thực hiện chức năng phân trang
            //tạo biến số sản phẩm trên trang
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int PageSize = 12;
            //tại biến thứ 2 số trang hiện tại
            int PageNumber = (page ?? 1);
           

            return View(lstSP.OrderBy(n => n.id).ToPagedList(PageNumber, PageSize));
        }

        [HttpGet]
        public ActionResult SearchProduct()
        {
            var totalCount = db.pets.Count();
            List<pet> list = db.pets.ToList();
            var text = "";
            // Lay ra ten nhung san pham co san - ho tro autocomplete
            foreach(var item in list)
            {
                text +=  item.name + ",";
            }

            Text t = new Text();
            t.text = text;
            ViewBag.TotalPage = totalCount / 6;
            System.Diagnostics.Debug.WriteLine("Tong so lan phai doc toi da: " + (totalCount / 6));
            return View(t);
        }

        [HttpPost]
        public JsonResult SearchProduct(AJAXRequest req)
        {
            List<pet> listSearched = db.pets.Where(x => x.name.ToLower().Contains(req.text.ToLower())).ToList();
            return Json(listSearched);
        }

        //Xây dựng trang chi tiết sản phẩm
        public ActionResult ChiTietSanPham(string id, string tensp)
        {
            //kiểm  tra tham số truyền vào có rỗng hay không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            //Nếu không thì truy xuất cơ sở dữ liệu lấy sp tương ứng
            pet sp = db.pets.SingleOrDefault(n => n.id == id);
            if (sp == null)
            {
                //thông báo nếu như không có sản phẩm đó
                return HttpNotFound();
            }
            return View(sp);
        }

        [HttpPost]
        public JsonResult AutoCompleteSearch(string Prefix)
        {
            List<pet> ObjList = db.pets.ToList();
            
            //Searching records from list using LINQ query  
            var Name = (from N in ObjList
                        where N.name.ToLower().Contains(Prefix.ToLower())
                        select new { N.name });
            return Json(Name, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadMoreProduct(int pageNo)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Dem so lan doc lai: " + pageNo);
                var totalCount = db.pets.Count();
         
                var positionStart = pageNo * 6;
                List<pet> listPets = db.pets.ToList();
                List<pet> result = new List<pet>();
                for (Int32 i = positionStart; i < positionStart + 6; i++)
                {
                    if (positionStart >= totalCount)
                        break;
                    result.Add(listPets.ElementAt(i));
                }
                //result = (List<spetho>)result.OrderBy(x => x.price);
                
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Co loi xay ra khi load more");
                return Json(new { success = false, ex = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}