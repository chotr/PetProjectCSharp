using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsProject.Models
{
    public class AJAXRequest
    {
        public String username { get; set; }
        public String fullname { get; set; }
        public String phone { get; set; }
        public String email { get; set; }
        public String address { get; set; }
        public String idAddressOld { get; set; }
        public String idAddressNew { get; set; }
        public string newPassword { get; set; }
        public string oldPassword { get; set; }

        public string password { get; set; }

        public String id { get; set; }
        public String status { get; set; }

        public String text { get; set; }

    }
}