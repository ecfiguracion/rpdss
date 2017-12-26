using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RPDSS.DataLayer.Models
{
    public class LoginModel
    {
        [Display(Name = "Enter username")]
        [Required]
        public string Username { get; set; }

        [Display(Name = "Enter password")]
        [Required]
        public string Password { get; set; }
    }
}
