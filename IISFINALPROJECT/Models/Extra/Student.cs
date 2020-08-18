using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace IISFINALPROJECT.Models
{   [MetadataType(typeof(userMetaData))]
    public partial class Student
    {
        public string confirmPassword { get; set; }
    }
    public class userMetaData

    {   
        [Display(Name= "Student Number")]
        [Required(AllowEmptyStrings=false,ErrorMessage="Your student number is taken")]
        public int Sudent_Number { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter the correct name")]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter the correct surname")]
        public string Surname { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter the correct date of birth")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode=true,DataFormatString="{0:MM/dd/yyyy}")]
        public string Date_of_birth { get; set; }


        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }

       
    }
}