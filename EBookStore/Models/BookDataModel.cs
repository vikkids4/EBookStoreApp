using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Models
{
    public class BookDataModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string isbn { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public string publisher { get; set; }
        public double price { get; set; }
        public int stock { get; set; }

        public BookDataModel() { }
    }
}