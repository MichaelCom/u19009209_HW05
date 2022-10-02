using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19009209_HW05.Models
{
    public class Borrow
    {
        public int borrowId { get; set; }

        public Nullable<int> studentId { get; set; }

        public Nullable<int> bookId { get; set; }

        public Nullable<DateTime> takenDate { get; set; }

        public Nullable<DateTime> broughtDate { get; set; }
    }
}