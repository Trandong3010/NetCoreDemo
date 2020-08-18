using System;
using System.Collections.Generic;

namespace WebAppEntryBlank.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? EnrollmentDate { get; set; }
    }
}
