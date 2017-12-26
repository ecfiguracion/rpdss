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

        [Display(Name = "Average Yield")]
        public string AvgYield { get; set; }

        [Display(Name = "Maximum Yield")]
        public string MaxYield { get; set; }

        [Required]
        public int Maturity { get; set; }

        [Display(Name = "Height")]
        public string Height { get; set; }

        [Display(Name = "Grain Size")]
        public string GrainSize { get; set; }

        [Display(Name = "Milling Recovery")]
        public string MillingRecovery { get; set; }

        [Display(Name = "Eating Quality")]
        public string EatingQuality { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Nursery")]
        public int GrowthStage1 { get; set; }

        [Display(Name = "Preparation")]
        public int GrowthStage2 { get; set; }

        [Display(Name = "Planting to Panicle Initiation")]
        public int GrowthStage3 { get; set; }

        [Display(Name = "Panicle Initiation to Flowering")]
        public int GrowthStage4 { get; set; }

        [Display(Name = "Flowering to Maturity")]
        public int GrowthStage5 { get; set; }
    }
}
