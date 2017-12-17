using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RPDSS.Constants;

namespace RPDSS.DataLayer.Models
{
    public class FuzzyParameterModel
    {
        public int Id { get; set; }

        [Required]
        public int Type { get; set; }

        public string TypeName
        {
            get
            {
                var typename = ParameterTypes.GetById(this.Type);
                if (typename != null)
                    return typename.Name;
                else
                    return string.Empty;                
            }
        }

        [Required]
        [Display(Name ="Growth Stages")]
        public int GrowthStages { get; set; }

        [Display(Name = "Growth Stage")]
        public string GrowthStagesName { get; set; }

        [Required]
        [Display(Name = "Absolute Minimum")]
        public int AbsoluteMin { get; set; }

        [Required]
        [Display(Name = "Optimum Minimum")]
        public int OptimumMin { get; set; }

        [Required]
        [Display(Name = "Optimum Maximum")]
        public int OptimumMax { get; set; }

        [Required]
        [Display(Name = "Absolute Maximum")]
        public int AbsoluteMax { get; set; }
    }

    public class FuzzyParameterDataEntryModel
    {
        public FuzzyParameterModel FuzzyParameter { get; set; }
        public IEnumerable<GrowthStagesModel> GrowthStages { get; set; }
        public IEnumerable<IdValue> ParameterTypes { get; set; }

        public FuzzyParameterDataEntryModel()
        {
            this.ParameterTypes = Constants.ParameterTypes.GetList();
        }
    }
}
