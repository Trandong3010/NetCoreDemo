using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.DataAccess.Context;
using ConsoleApp.DataAccess.Entity;
using ConsoleApp.Infrastructure.Data;
using ConsoleApp.Infrastructure.Models;
using ConsoleApp.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;
        static async Task Main()
        {
            Console.WriteLine("Hello World!");
            RegisterService();
            IServiceScope scope = _serviceProvider.CreateScope();
            var data = scope.ServiceProvider.GetRequiredService<IData>();
            var courseService = scope.ServiceProvider.GetRequiredService<ICourseService>();
            var studentService = scope.ServiceProvider.GetRequiredService<IStudentService>();
            var enrollmentService = scope.ServiceProvider.GetRequiredService<IEnrollmentService>();
            var departmentService = scope.ServiceProvider.GetRequiredService<IDepartmentService>();
            var instructorService = scope.ServiceProvider.GetRequiredService<IInstructorService>();
            //var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
            //data.Initialize(context);


            // Get list student
            //foreach (var item in await studentService.GetList())
            //{
            //    Console.WriteLine($"Last Name: {item.LastName} " +
            //                      $"FirtMidName: {item.FirstMidName} " +
            //                      $"EnrollmentDate: {item.EnrollmentDate}");
            //}
            // GetByid student
            //var liststudent = studentService.GetById(3);
            // Inset student
            //var students = new Student
            //    {FirstMidName = Faker.Name.First(), LastName = Faker.Name.Last(), EnrollmentDate = DateTime.Parse(DateTime.Now.ToShortDateString())};
            //studentService.Insert(students);

            // Update student
            //var students = new Student { ID = 2, FirstMidName = Faker.Name.First(), LastName = Faker.Name.Last(), EnrollmentDate = DateTime.Parse(DateTime.Now.ToShortDateString()) };
            //studentService.Update(students);

            // Delete student
            //studentService.Delete(10);

            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------//
            // Get list course
            //foreach (var item in await courseService.GetList())
            //{
            //    Console.WriteLine($"Course Title: {item.Title}" + 
            //                      $"-" +
            //                      $"Credits: {item.Credits}");
            //}
            //Insert course
            //var courses = new Course
            //{
            //    CourseID = Faker.RandomNumber.Next(4022),
            //    Title = Faker.Country.Name(),
            //    Credits = 3, DepartmentID = 1
            //};
            //courseService.Insert(courses);
            // Update course
            //var courses = new Course
            //{
            //    CourseID = 54,
            //    Title = Faker.Country.Name(),
            //    Credits = 3
            //};
            //courseService.Update(courses);
            // Delete course
            //courseService.Delete(54);
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------//
            // Insert enrollment
            //var enrollment = new Enrollment {StudentID = 9, CourseID = 4041, Grade = Grade.B};
            //enrollmentService.Insert(enrollment);
            // Update enrollment
            //var enrollment = new Enrollment { EnrollmentID = 13, StudentID = 9, CourseID = 3141, Grade = Grade.B };
            //enrollmentService.Update(enrollment);
            // Delete enrollment
            //enrollmentService.Delete(13);
            // Department
            //foreach (var item in await departmentService.GetList())
            //{
            //    Console.WriteLine($"Name: {item.Name} " +
            //                      $"Budget: {item.Budget} " +
            //                      $"StartDate: {item.StartDate}");
            //}
            // Instructor
            var list = await instructorService.GetById(2, 2021);
            foreach (var item in list.Instructors)
            {
                Console.WriteLine($"{item.LastName}" +
                                  $"{item.FirstMidName}"+
                                  $"{item.HireDate}"+
                                  $"{item.CourseAssignments.Select(x=>x.Course.Title)}");
            }
            DisposeServices();
        }


        private static void RegisterService()
        {
            var services = new ServiceCollection()
                .AddDbContext<SchoolContext>()
                .AddSingleton<ICourseService, CourseService>()
                .AddSingleton<IStudentService, StudentService>()
                .AddSingleton<IEnrollmentService, EnrollmentService>()
                .AddSingleton<IDepartmentService, DepartmentService>()
                .AddSingleton<IInstructorService, InstructorService>()
                .AddSingleton<IData, Data>();
            _serviceProvider = services?.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
                return;
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        
    }
}
