using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RPDSS.DataLayer.Models
{
    public class UserAccountModel
    {
        public int Id { get; set; }
        
       // [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

       // [Display(Name ="UserName")]
        [Required]
        public string UserName { get; set; }

       // [Display(Name = "Password")]
        [Required]
        public string Password { get; set; }
    }
    //public class UserAccountDataEntryModel
    //{
    //    public UserAccountModel UserAccount { get; set; }
    //}
}
