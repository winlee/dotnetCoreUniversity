using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using University.Data;
using University.Models.SchoolViewModels;
using Microsoft.EntityFrameworkCore;
using University.Models;
using System.Data.Common;

namespace University.Controllers
{
    public class InstructorsController : Controller
    {
        private SchoolContext _context;

        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Enrollments)
                            .ThenInclude(i => i.Student)
                 .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                  .AsNoTracking()
                  .OrderBy(i => i.LastName)
                  .ToListAsync();

            if (id != null)
            {
                ViewData["InstructorID"] = id.Value;
                Instructor instructor = viewModel.Instructors.Where(i => i.ID == id.Value).Single();
                viewModel.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null)
            {
                ViewData["CourseID"] = courseID.Value;

                #region Explicitly Loading

                var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                _context.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    _context.Entry(enrollment).Reference(x => x.Student).Load();
                }
                viewModel.Enrollments = selectedCourse.Enrollments;
                
                #endregion

                //viewModel.Enrollments = viewModel.Courses.Where(c => c.CourseID == courseID).Single().Enrollments;
            }

            return View(viewModel);
        }

        private async Task<ActionResult> TestRawSql(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string query = "select * from Department where DepartmentID={0}";
            var department = await _context.Departments
                .FromSql(query, id)
                .Include(d => d.Administrator)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        private async Task<ActionResult> TestRawSql2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<EnrollmentDateGroup> groups = new List<EnrollmentDateGroup>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query2 = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
                        + "FROM Person "
                        + "WHERE Discriminator = 'Student' "
                        + "GROUP BY EnrollmentDate";
                    command.CommandText = query2;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new EnrollmentDateGroup { EnrollmentDate = reader.GetDateTime(0), StudentCount = reader.GetInt32(1) };
                            groups.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }

            string query = "select * from Department where DepartmentID={0}";
            var department = await _context.Departments
                .FromSql(query, id)
                .Include(d => d.Administrator)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }
    }
}