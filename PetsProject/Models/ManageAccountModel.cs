using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsProject.Models
{
    public class ManageAccountModel
    {
        public String fullname { get; set; }
        public String email { get; set; }
        public String avatar { get; set; }
        public String phone { get; set; }
        public String address { get; set; }
        public String username { get; set; }
        public String defaultAddress { get; set; }
        public List<address_Book> listAddress { get; set; }
        public List<BillIem> listReceivedBill { get; set; }
        public List<BillIem> listOnTheWayBill { get; set; }
        public List<BillIem> listDeliveredBill { get; set; }
        public List<BillIem> listCancelledBill { get; set; }
    }
}