using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsProject.Models
{
    public class AccountDetailsAdminModel
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string avatar { get; set; }
        public string status { get; set; }
        public List<String> listAddressBook { get; set; }

        public string selectStatus { get; set; }

    }
}