using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CourseManagementSystem.Models
{
    public partial class Student
    {
        public Student()
        {
            Takes = new HashSet<Take>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DeptIdFk { get; set; }

        [NotMapped]
        public string FullName 
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        public virtual Department DeptIdFkNavigation { get; set; }
        public virtual ICollection<Take> Takes { get; set; }
    }
}
