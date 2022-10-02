using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u19009209_HW05.Models
{
    public class Student
    {
        public int studentId { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        public Nullable<DateTime> birthdate { get; set; }

        public string gender { get; set; }

        public string Class { get; set; }

        public Nullable<int> point { get; set; }
    }
}