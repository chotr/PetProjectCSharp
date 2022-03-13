using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsProject.Models
{
    public class AdminAddressModel
    {
        public String id { get; set; }

        public String name { get; set; }

        public String address { get; set; }

        public String payment { get; set; }

        public int totalcost { get; set; }

        public String datetime { get; set; }

        public String  status { get; set; }

    }
}