using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RPDSS.DataLayer.Models
{
    public class FuzzySuitabilityIntervalModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Minimum")]
        [Required]
        public decimal Min { get; set; }

        [Display(Name = "Maximum")]
        [Required]
        public decimal Max { get; set; }
    }
}
