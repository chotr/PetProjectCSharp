using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsProject.Utils
{
    public class CreateRandomID
    {
        public String newIDProduct()
        {
            return "PRO"+Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper();
        }

        public String newIDAddressBook()
        {
            return "AB"+Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper();
        }
        public String newIDCart()
        {
            return "CR" + Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper();
        }
    }
}