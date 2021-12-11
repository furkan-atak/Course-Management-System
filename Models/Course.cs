using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CourseManagementSystem.Models
{
    public partial class Course
    {
        public Course()
        {
            Takes = new HashSet<Take>();
            Teaches = new HashSet<Teach>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field Name is Required")]
        public string Name { get; set; }
        public int DeptIdFk { get; set; }
        public DateTime? Date { get; set; }
        [Range(1, 10)]
        public int Credit { get; set; }
        [RegularExpression("^(?![\\d\\W]+$).+")]
        [MaxLength(5)]
        public string Place { get; set; }

        public virtual Department DeptIdFkNavigation { get; set; }
        public virtual ICollection<Take> Takes { get; set; }
        public virtual ICollection<Teach> Teaches { get; set; }
    }
}
