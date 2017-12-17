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

        [Display(Name = "Rice Variety")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Minimum Rainfall")]
        [Required]
        public int MinRainfall { get; set; }

        [Display(Name = "Maximum Rainfall")]
        [Required]
        public int MaxRainfall { get; set; }

        [Display(Name = "Minimum Temperature")]
        [Required]
        public int MinTemperature { get; set; }

        [Display(Name = "Maximum Temperature")]
        [Required]
        public int MaxTemperature { get; set; }

        public int Maturity { get; set; }

        [Display(Name = "Soil Type")]
        public string SoilType { get; set; }
    }
}
