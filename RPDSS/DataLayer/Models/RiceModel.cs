using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RPDSS.DataLayer.Models
{
    public class RiceModel
    {
        public int Id { get; set; }

        [Display(Name = "Variety")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Min. Rainfall")]
        [Required]
        public int MinRainfall { get; set; }

        [Display(Name = "Max. Rainfall")]
        [Required]
        public int MaxRainfall { get; set; }

        [Display(Name = "Min. Temperature")]
        [Required]
        public int MinTemperature { get; set; }

        [Display(Name = "Max. Temperature")]
        [Required]
        public int MaxTemperature { get; set; }

        public int Maturity { get; set; }

        [Display(Name = "Soil Type")]
        public string SoilType { get; set; }
    }
}
