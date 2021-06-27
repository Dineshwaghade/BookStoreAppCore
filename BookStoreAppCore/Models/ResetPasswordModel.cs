using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAppCore.Models
{
    public class ResetPasswordModel
    {
        public string uid { get; set; }
        public string token { get; set; }
        [Required,Display(Name ="New Password"),DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required, Display(Name = "Confirm New Password"), DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
