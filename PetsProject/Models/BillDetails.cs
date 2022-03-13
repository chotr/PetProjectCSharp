using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsProject.Models
{
    public class BillDetails
    {
        public String product { get; set; }
        public int quantity { get; set; }

        public String price { get; set; }
    }
}