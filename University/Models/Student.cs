using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace University.Models
{
    //public class Student
    //{
    //    public int ID { get; set; }

    //    [Required]
    //    [StringLength(50)]
    //    [Display(Name = "Last Name")]
    //    public string LastName { get; set; }

    //    [Column("FirstName")]
    //    [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
    //    [Display(Name = "First Name")]
    //    public string FirstMidName { get; set; }

    //    [DataType(DataType.Date)]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    [Display(Name = "Enrollment Date")]
    //    public DateTime EnrollmentDate { get; set; }

    //    [Display(Name = "Full Name")]
    //    public string FullName
    //    {
    //        get
    //        {
    //            return LastName + ", " + FirstMidName;
    //        }
    //    }

    //    public ICollection<Enrollment> Enrollments { get; set; }

    //}

    public class Student : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
