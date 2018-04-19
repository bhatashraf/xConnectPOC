using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace xConnectPOC.Models
{
    public class ContactModel
    {
        [Display(Name ="First Name")]
        [Required(ErrorMessage ="First Name is required!")]
        public string FirstName { get; set; }
        [Display(Name ="Last Name: ")]
        [Required(ErrorMessage ="Last Name is required!")]
        public string LastName { get; set; }

        [Display(Name ="Email Address: ")]
        [Required(ErrorMessage = "The email address is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!")]
        public string Email { get; set; }

    }
}