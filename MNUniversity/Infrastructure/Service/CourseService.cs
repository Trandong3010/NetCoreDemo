using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess.Context;
using DataAccess.Entity;
using Infrastructure.Models.Courses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseModel>> GetList();
        void Insert(CourseModel model);
        void Update(CourseModel model);
        void Delete(int? id);
        CourseModel GetById(int id);
    }

    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;

        public CourseService(SchoolContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CourseModel>> GetList()
        {

            var list = await _context.Courses
	            .ToListAsync();
            return _mapper.Map<IEnumerable<CourseModel>>(list);
        }

        public void Insert(CourseModel model)
        {
            if(model == null) return;
            var entity = _mapper.Map<CourseModel, Course>(model);
            _context.Courses.Add(entity);
            _context.SaveChanges();
        }

        public void Update(CourseModel model)
        {
            if(model == null) return;
            var entity = _mapper.Map<CourseModel, Course>(model);
            _context.Courses.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int? id)
        {
            if(id == 0) return;

            var course = _context.Courses.FirstOrDefault(x => x.CourseID == id);
            if(course == null) return;
            _context.Courses.Remove(course);
            _context.SaveChanges();
        }

        public CourseModel GetById(int id)
        {
	        var course = _mapper.Map<CourseModel>
	        (_context.Courses
		        .Include(x => x.Department)
		        .Include(x => x.Enrollments)
		        .Include(x => x.CourseAssignments)
		        .AsNoTracking().FirstOrDefault(x => x.CourseID == id));
	        return course;
        }

        //private List<Object> PopulateDepartmentsDropDownList(object selectedDepartment = null)
        //{
        //    var departmentsQuery = from d in _context.Departments orderby d.Name select d;
        //    return departmentsQuery;
        //}
    }
}
