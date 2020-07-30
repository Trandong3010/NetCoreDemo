using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Context;
using DataAccess.Entity;
using Infrastructure.Common;
using Infrastructure.Models.Students;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service
{

	public interface IStudentService
	{
		Task<IEnumerable<StudentModel>> GetList();
		void Insert(StudentModel model);
		void Update(StudentModel model);
		void Delete(int? id);
		StudentModel GetById(int id);

		Task<IEnumerable<Student>> GetListPaging(string sortOrder, string currentFilter, string searchString,
			int? pageNumber);

		Task<IList<Student>> GetAll();
	}

	public class StudentService : IStudentService
	{
		protected readonly SchoolContext _context;
		private readonly IMapper _mapper;
		public StudentService(SchoolContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<IEnumerable<StudentModel>> GetList()
		{

			var list = await _context.Students.AsNoTracking().ToListAsync();
			return _mapper.Map<IEnumerable<StudentModel>>(list);
		}

		public void Insert(StudentModel model)
		{
			if (model == null) return;
			var entity = _mapper.Map<StudentModel, Student>(model);
			_context.Students.Add(entity);
			_context.SaveChanges();
		}

		public void Update(StudentModel model)
		{
			if (model == null) return;
			var entity = _mapper.Map<StudentModel, Student>(model);
			_context.Students.Update(entity);
			_context.SaveChanges();
		}

		public void Delete(int? id)
		{
			if (id == null) return;

			var student = _context.Students.FirstOrDefault(x => x.ID == id);
			if (student == null) return;
			_context.Students.Remove(student);
			_context.SaveChanges();
		}

		public StudentModel GetById(int id)
		{
			var student = _mapper.Map<StudentModel>(_context.Students
				.AsNoTracking()
				.FirstOrDefault(x => x.ID == id));
			return student;
		}

		public async Task<IEnumerable<Student>> GetListPaging(string sortOrder, string currentFilter, string searchString, int? pageNumber)
		{
			if (searchString != null)
			{
				pageNumber = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			var students = from s in _context.Students select s;

			if (!String.IsNullOrEmpty(searchString))
			{
				students = students.Where(x =>
					x.LastName.Contains(searchString) || x.FirstMidName.Contains(searchString));
			}

			switch (sortOrder)
			{
				case "name_desc":
					students = students.OrderByDescending(x => x.LastName);
					break;
				case "Date":
					students = students.OrderByDescending(x => x.EnrollmentDate);
					break;
				case "date_desc":
					students = students.OrderBy(x => x.LastName);
					break;
				default:
					students = students.OrderBy(x => x.LastName);
					break;
			}

			int pageSize = 3;
			return await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize);
		}

		public async Task<IList<Student>> GetAll()
		{
			var list = await _context.Students
				.Include(x => x.Enrollments)
				.AsNoTracking().ToListAsync();
			return list;
		}
	}
}
