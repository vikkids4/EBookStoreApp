using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBookStore.Models
{
    public class BookModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string isbn { get; set; }
        public int authorId { get; set; }
        public int genreId { get; set; }
        public int publisherId { get; set; }
        public double price { get; set; }
        public int stock { get; set; }

        public BookModel() { }
    }
}