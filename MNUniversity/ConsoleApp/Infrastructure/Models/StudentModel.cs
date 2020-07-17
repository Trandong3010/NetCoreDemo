using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp.DataAccess.Entity;

namespace ConsoleApp.Infrastructure.Models
{
    public class StudentModel
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public Grade? Grade { get; set; }
        public string Title { get; set; }
    }
}
