using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAppCore.Models
{
    public class ForgotPasswordModel
    {
        [Required,Display(Name ="Email address")]
        public string Email { get; set; }
        public bool EmailSent{ get; set; }
    }
}
