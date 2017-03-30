using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using University.Data;
using Microsoft.EntityFrameworkCore;
using University.Models;

namespace University.Controllers
{
    public class CoursesController : Controller
    {
        private SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var courses = _context.Courses.Include(c => c.Department)
                .AsNoTracking();

            #region 3 ways to Loading Navigation properties data
            //1. Eager Loading
            //var departments = _context.Departments.Include(d => d.Courses);

            //List<string> courselist = new List<string>();
            //foreach (var d in departments)
            //{
            //    foreach (var c in d.Courses)
            //    {
            //        courselist.Add(d.Name + c.Title);
            //    }
            //}

            //2. Explicit Loading
            //var departments2 = _context.Departments;
            //foreach (Department d in departments2)
            //{
            //    _context.Entry(d).Collection(p => p.Courses).Load();
            //    _context.Courses.Where(c => c.DepartmentID == d.DepartmentID).Load();
            //    foreach (Course c in d.Courses)
            //    {
            //        courselist.Add(d.Name + c.Title);
            //    }
            //}

            //3. EF Core does not support Lazy Loading

            #endregion

            return View(await courses.ToListAsync());
        }
    }
}