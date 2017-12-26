using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPDSS.DataLayer.Models
{
    public class PlantingSuitabilityModel
    {
        public FuzzySuitabilityIntervalModel PlantingSuitability { get; set; }
        public string Months { get; set; }
        public string MonthsDisplay
        {
            get
            {
                if (this.Months.Length > 0)
                    return this.Months;
                else
                    return "N/A";
            }
        }
    }

    public class GraphData
    {
        public ICollection<string> Months { get; set; }
        public ICollection<decimal> Rainfalls { get; set; }
        public ICollection<decimal> Temperatures { get; set; }
        public IEnumerable<RainfallModel> RainfallData { get; set; }
        public IEnumerable<TemperatureModel> TemperatureData { get; set; }
        public ICollection<PlantingSuitabilityModel> PlantingSuitability { get; set; }

        public GraphData()
        {
            this.Months = new List<string>();
            this.Rainfalls = new List<decimal>();
            this.Temperatures = new List<decimal>();
            this.PlantingSuitability = new List<PlantingSuitabilityModel>();
        }
    }

    public class Calendar
    {
        public string GrowthStage { get; set; }
        public string SuggestedDate { get; set; }

        public Calendar(string growthStage, string suggestedDate)
        {
            this.GrowthStage = growthStage;
            this.SuggestedDate = suggestedDate;
        }
    }

    public class CropCalendar
    {
        public RiceModel RiceProfile { get; set; }
        public ICollection<Calendar> CalendarData { get; set; }

        public CropCalendar()
        {
            this.CalendarData = new List<Calendar>();
        }
    }

    public class HomeModel
    {
        public IEnumerable<LookupModel> Years { get; set; }
        public LookupModel SelectedYear { get; set; }
        public IEnumerable<LookupModel> GrowthStages { get; set; }
        public LookupModel SelectedGrowthStages { get; set; }
        public IEnumerable<RiceModel> RiceVarieties { get; set; }
        public RiceModel SelectedRiceVariety { get; set; }
    }
}
