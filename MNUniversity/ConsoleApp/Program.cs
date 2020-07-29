using System;
using System.Threading.Tasks;
using DataAccess.Context;
using Infrastructure.Data;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;

#pragma warning disable 1998
        private static async Task Main()
#pragma warning restore 1998
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


            //Get list student
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
            //var list = await instructorService.GetById(2, 2021);
            //foreach (var item in list.Instructors)
            //{
            //    Console.WriteLine($"{item.LastName}" +
            //                      $"{item.FirstMidName}"+
            //                      $"{item.HireDate}"+
            //                      $"{item.CourseAssignments.Select(x=>x.Course.Title)}");
            //}
            // Update Instructor and Course
            //instructorService.Update(10, );
            DisposeServices();
        }


        private static void RegisterService()
        {
            var config = LoadConfiguration();
            var connectionString = config.GetConnectionString("ConnectionString");
            var services = new ServiceCollection()
                .AddDbContext<SchoolContext>(options => options.UseSqlServer(connectionString))
                .AddSingleton<ICourseService, CourseService>()
                .AddSingleton<IStudentService, StudentService>()
                .AddSingleton<IEnrollmentService, EnrollmentService>()
                .AddSingleton<IDepartmentService, DepartmentService>()
                .AddSingleton<IInstructorService, InstructorService>()
                .AddSingleton<IData, Data>()
                .AddSingleton(config);
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
        protected static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .Build();
            return builder;
        }

    }
}
