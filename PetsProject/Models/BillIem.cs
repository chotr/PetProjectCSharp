using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsProject.Models
{
    public class BillIem
    {
        public String id { get; set; }
        public String totalCost { get; set; }
        public String fullDateTime { get; set; }
        public String payment { get; set; }
        public String note { get; set; }
        public String status { get; set; }
        public String address { get; set; }
        public List<BillDetails> listBillDetails { get; set; }

    }
}