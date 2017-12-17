using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RPDSS.DataLayer.Models
{
    public class RainfallModel
    {
        public int Id { get; set; }
        public string Calendar { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public decimal Rainfall { get; set; }
    }

    public class RainfallDataEntryModel
    {
        public RainfallModel Rainfall { get; set; }
        public IEnumerable<CalendarModel> Calendar { get; set; }
    }
}
