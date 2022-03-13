using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsProject.Models
{
    public class ListAdminAddressModel
    {
        public List<AdminAddressModel> listReceived { get; set; }
        public List<AdminAddressModel> listCancelled { get; set; }
        public List<AdminAddressModel> listOnTheWay { get; set; }
        public List<AdminAddressModel> listDelivered { get; set; }
    }
}