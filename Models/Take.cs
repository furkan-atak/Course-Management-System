using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CourseManagementSystem.Models
{
    public partial class Take
    {
        public int Id { get; set; }
        public int CourseIdFk { get; set; }
        public int StudentIdFk { get; set; }
        [Range(1,8)]
        public string Semester { get; set; }
        [Range(2020,2100)]
        public int? Year { get; set; }
        [Range(1,4)]
        public int? Grade { get; set; }

        public virtual Course CourseIdFkNavigation { get; set; }
        public virtual Student StudentIdFkNavigation { get; set; }
    }
}
