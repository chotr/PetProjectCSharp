using PetsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PetsProject.Utils
{
    public class GenerateInfo
    {
        static ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
        public static String getInfoBill(bill bill)
        {
            string result = "YOUR INFO BILL@";
            account acc = db.accounts.Find(bill.username);
            address_Book ab = db.address_Book.Find(bill.idaddress);
            List<bill_Details> listBillItem = db.bill_Details.Where(x => x.idbill == bill.id).ToList();
            result+= "ID bill: " + bill.id + "@Customer name: " +acc.fullname+"@Date: "+bill.time.ToString("HH:mm dd-MM-yyyy")+"@"+"Payment: "+bill.payment+"@Note: "+bill.note+"@Total to pay: "+bill.totalcost.ToString("#,## VNĐ") + "@@YOUR BILL DETAIL:"+"@";
            foreach(var item in listBillItem)
            {
                pet pet = db.pets.Find(item.idproduct);
                result += pet.name + "   x" + item.quantity + "   --> " + item.totalcost.ToString("#,## VNĐ") + "@";
            }
            result = result.Replace("@", "<br/>");
            System.Diagnostics.Debug.WriteLine(result);
            return result;
        }
    }
}