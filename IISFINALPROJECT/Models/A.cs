using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IISFINALPROJECT.Models
{
    public class A
    {
        public List<Student> ListStudent { get; set; }
        public List<Course> ListCourse { get; set; }
        public List<Module> ListModule { get; set; }
        public List<Assigments> ListAssignment { get; set; }
        public List<StudentModule> ListStudentModules { get; set; }
        public List<StudentAssignment> ListStudentAssignment { get; set; }
        public string me { get; set; }


    }
}