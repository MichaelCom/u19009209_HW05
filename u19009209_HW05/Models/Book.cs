using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19009209_HW05.Models
{
    public class Book
    {
        public int bookId { get; set; }
        public string name { get; set; }
        public Nullable<int> pagecount { get; set; }
        public Nullable<int> point { get; set; }
        public Nullable<int> authorId { get; set; }
        public Nullable<int> typeId { get; set; }
    }
}