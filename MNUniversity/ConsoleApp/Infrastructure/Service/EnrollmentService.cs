using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.DataAccess.Context;
using ConsoleApp.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Infrastructure.Service
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollment>> GetList();
        void Insert(Enrollment enrollment);
        void Update(Enrollment enrollment);
        void Delete(int id);
    }

    public class EnrollmentService : IEnrollmentService
    {
        protected readonly SchoolContext _context;

        public EnrollmentService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Enrollment>> GetList()
        {
            var list = await _context.Enrollments.AsNoTracking().ToListAsync();
            return list;
        }

        public void Insert(Enrollment enrollment)
        {
            if (enrollment == null) return;

            try
            {
                _context.Enrollments.Add(enrollment);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Update(Enrollment enrollment)
        {
            if(enrollment == null) return;
            try
            {
                _context.Enrollments.Update(enrollment);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Delete(int id)
        {
            if(id == 0) return;

            var enrollment = _context.Enrollments.FirstOrDefault(x => x.EnrollmentID == id);
            if(enrollment == null) return;
            try
            {
                _context.Enrollments.Remove(enrollment);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
