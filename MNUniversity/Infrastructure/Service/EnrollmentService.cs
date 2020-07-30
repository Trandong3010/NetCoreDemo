using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Context;
using DataAccess.Entity;
using Infrastructure.Models.Enrollment;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentModel>> GetList();
        void Insert(EnrollmentModel model);
        void Update(EnrollmentModel model);
        void Delete(int id);
        EnrollmentModel GetById(int id);
    }

    public class EnrollmentService : IEnrollmentService
    {
        protected readonly SchoolContext _context;
        protected readonly IMapper _mapper;

        public EnrollmentService(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EnrollmentModel>> GetList()
        {
	        return _mapper.Map<IEnumerable<EnrollmentModel>>(await _context.Enrollments.AsNoTracking().ToListAsync());
        }

        public void Insert(EnrollmentModel model)
        {
            if (model == null) return;
            var entity = _mapper.Map<EnrollmentModel, Enrollment>(model);
            _context.Enrollments.Add(entity);
            _context.SaveChanges();
        }

        public void Update(EnrollmentModel model)
        {
            if(model == null) return;
            var entity = _mapper.Map<EnrollmentModel, Enrollment>(model);
            _context.Enrollments.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            if(id == 0) return;

            var enrollment = _context.Enrollments.FirstOrDefault(x => x.EnrollmentID == id);
            if(enrollment == null) return;
            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();
        }

        public EnrollmentModel GetById(int id)
        {
	        var enrollment = _mapper.Map<EnrollmentModel>
	        (_context.Enrollments
		        .Include(x => x.Course)
		        .Include(x => x.Student)
		        .AsNoTracking().FirstOrDefault(x => x.EnrollmentID == id));
	        return enrollment;
        }
    }
}
