using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19009209_HW05.Models.ViewModels
{
    public class BorrowedBook
    {
        public int bookId { get; set; }

        public int studentId { get; set; }

        public BorrowedBook(int bookId, int studentId)
        {
            this.bookId = bookId;
            this.studentId = studentId;
        }
    }
}