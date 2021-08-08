using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace my_boot.Models
{
    public class Cart
    {
        public int productid { get; set; }
        public float price { get; set; }
        public int qty { get; set; }
        public float bill { get; set; }
        public string prductname { get; set; }


    }
}