using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19009209_HW05.Models.ViewModels
{
    public class BorrowsHybrid
    {
        public string studentName { get; set; }

        public Nullable<int> borrowId { get; set; }

        public Nullable<DateTime> takenDate { get; set; }

        public Nullable<DateTime> broughtDate { get; set; }
    }
}