using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RPDSS.DataLayer.Models
{
    public class TemperatureModel
    {
        public int Id { get; set; }
        public string Calendar { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public decimal Temperature { get; set; }
    }

    public class TemperatureDataEntryModel
    {
        public TemperatureModel Temperature { get; set; }
        public IEnumerable<CalendarModel> Calendar { get; set; }
    }
}
