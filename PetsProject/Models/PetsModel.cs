using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetsProject.Models
{
    public class PetsModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string weight { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public string origin { get; set; }
        public double price { get; set; }
        public string imageslink { get; set; }
        public double pricebefore { get; set; }
        public string checkquality { get; set; }
        public string description { get; set; }
        public int rate { get; set; }
        public string status { get; set; }


        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string file { get; set; }
    }
}