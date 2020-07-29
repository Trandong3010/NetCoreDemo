using System;
using AutoMapper;
using DataAccess.Entity;

namespace Infrastructure.Models.Students
{
    public class StudentModel
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string FullName { get; set; }
    }
}
