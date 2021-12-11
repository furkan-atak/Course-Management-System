using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CourseManagementSystem.Models
{
    public partial class Teach
    {
        public int Id { get; set; }
        public int CourseIdFkk { get; set; }
        public int TeacherIdFk { get; set; }
        [Range(1,8)]
        public string Semester { get; set; }
        [Range(2020,2100)]
        public int? Year { get; set; }

        public virtual Course CourseIdFkkNavigation { get; set; }
        public virtual Teacher TeacherIdFkNavigation { get; set; }
    }
}
