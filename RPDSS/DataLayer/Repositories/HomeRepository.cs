using Dapper;
using Microsoft.Extensions.Configuration;
using RPDSS.Constants;
using RPDSS.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RPDSS.DataLayer.Repositories
{
    public class HomeRepository
    {
        private System.Data.IDbConnection db;

        public HomeRepository(IConfiguration config)
        {
            db = new SqlConnection(config.GetConnectionString(ConfigConstants.DefaultConnection));
        }

        public bool Authenticate(LoginModel login)
        {
            var sql = @"SELECT id FROM useraccount
                        WHERE username = @username AND password = @password";
            var id = db.QueryFirstOrDefault<int>(sql, new { login.Username, login.Password });
            return id > 0;
        }

        public HomeModel GetLookUps()
        {
            var homeModel = new HomeModel();

            // Calendar
            var sql = @"SELECT id,name FROM growthstages";
            homeModel.GrowthStages = db.Query<LookupModel>(sql);
            homeModel.SelectedGrowthStages = homeModel.GrowthStages.FirstOrDefault();

            // Years
            sql = @"select distinct year as Id, CAST(year AS NVARCHAR) as Name from rainfall
                    union
                    select distinct year as Id, CAST(year AS NVARCHAR) as Name from temperature
                    union
                    select 9999 as Id, 'PROJECTED' as Name";
            homeModel.Years = db.Query<LookupModel>(sql);
            homeModel.SelectedYear = homeModel.Years.FirstOrDefault();

            // Rice Varieties
            sql = @"SELECT id,name FROM rice";
            homeModel.RiceVarieties = db.Query<RiceModel>(sql);
            homeModel.SelectedRiceVariety = homeModel.RiceVarieties.FirstOrDefault();
          
            return homeModel;
        }


        public GraphData GetGraphsData(int year, int growthStageId)
        {
            var graphData = new GraphData();

            // Fuzzy Parameters
            var sql = @"SELECT fp.id,fp.type,fp.growthstages,gs.name growthstagesname,fp.absolutemin,
	                    fp.optimummin,fp.optimummax,fp.absolutemax
                    FROM fuzzyparameter fp 
                    INNER JOIN growthstages gs ON fp.growthstages = gs.id";
            var fuzzyParameters = db.Query<FuzzyParameterModel>(sql);

            // Fuzzy Suitability Interval
            sql = @"SELECT id,name,min,max
                    FROM fuzzysuitabilityinterval";
            var fuzzySuitabilityInterval = db.Query<FuzzySuitabilityIntervalModel>(sql);
            foreach (var item in fuzzySuitabilityInterval)
            {
                graphData.TemperatureSuitability.Add(new PlantingSuitabilityModel() { PlantingSuitability = item, Months=string.Empty });
                graphData.RainfallSuitability.Add(new PlantingSuitabilityModel() { PlantingSuitability = item, Months = string.Empty });
            }

            // Temperatures
            IEnumerable<TemperatureModel> temperatures = new List<TemperatureModel>();
            if (year == 9999)
            {
                sql = @"SELECT c.name as calendar, t.month, AVG(t.temperature) temperature
                    FROM temperature t
                    INNER JOIN calendar c ON t.month = c.id
                    GROUP BY t.month,c.name";
                temperatures = db.Query<TemperatureModel>(sql);
            }
            else
            {
                sql = @"SELECT c.name as calendar, t.month, t.year, t.Temperature
                    FROM Temperature t
                    INNER JOIN calendar c ON t.month = c.id
                    WHERE t.year = @year";
                temperatures = db.Query<TemperatureModel>(sql, new { year });
                graphData.TemperatureData = temperatures;
            }
            graphData.TemperatureData = temperatures;

            // Rainfall
            IEnumerable<RainfallModel> rainfalls = new List<RainfallModel>();

            if (year == 9999)
            {
                sql = @"SELECT c.name as calendar, r.month, AVG(r.rainfall) rainfall
                        FROM rainfall r
                        INNER JOIN calendar c ON r.month = c.id
                        GROUP BY r.month,c.name";
                rainfalls = db.Query<RainfallModel>(sql, new { year });
            }
            else
            {
                sql = @"SELECT r.id, c.name as calendar, r.month, r.year, r.rainfall
                        FROM rainfall r
                        INNER JOIN calendar c ON r.month = c.id
                        WHERE r.year = @year";
                rainfalls = db.Query<RainfallModel>(sql, new { year });
            }
            graphData.RainfallData = rainfalls;

            // Months
            sql = @"SELECT id,name FROM calendar";
            var months = db.Query<LookupModel>(sql);

            // Compute for Rainfall 
            var rainfallType = 1;
            var temperatureType = 2;

            var fuzzyParamRainfall = fuzzyParameters.SingleOrDefault(x => x.GrowthStages == growthStageId && x.Type == rainfallType);
            var fuzzyParamTemperature = fuzzyParameters.SingleOrDefault(x => x.GrowthStages == growthStageId && x.Type == temperatureType);

            foreach (var item in months)
            {
                graphData.Months.Add(item.Name);

                // For rainfall
                var rainfall = rainfalls.SingleOrDefault(y => y.Month == item.Id);
                var x = rainfall != null ? rainfall.Rainfall : 0;
                var a = (decimal)fuzzyParamRainfall.AbsoluteMin;
                var b = (decimal)fuzzyParamRainfall.OptimumMin;
                var c = (decimal)fuzzyParamRainfall.OptimumMax;
                var d = (decimal)fuzzyParamRainfall.AbsoluteMax;

                var fuzzyRainfallValue = this.GetFuzzyValue(x, a, b, c, d);
                graphData.Rainfalls.Add(fuzzyRainfallValue);

                // For temperature
                var temperature = temperatures.SingleOrDefault(y => y.Month == item.Id);
                x = temperature != null ? temperature.Temperature : 0;
                a = (decimal)fuzzyParamTemperature.AbsoluteMin;
                b = (decimal)fuzzyParamTemperature.OptimumMin;
                c = (decimal)fuzzyParamTemperature.OptimumMax;
                d = (decimal)fuzzyParamTemperature.AbsoluteMax;

                var fuzzyTempValue = this.GetFuzzyValue(x, a, b, c, d);
                graphData.Temperatures.Add(fuzzyTempValue);

                // Check for suitability
                foreach (var suitability in graphData.TemperatureSuitability)
                {
                    if (this.IsSuitable(suitability.PlantingSuitability,fuzzyTempValue,0))
                    {
                        if (suitability.Months.Length > 0)
                            suitability.Months += "," + item.Name;
                        else
                            suitability.Months += item.Name;
                    }
                }

                foreach (var suitability in graphData.RainfallSuitability)
                {
                    if (this.IsSuitable(suitability.PlantingSuitability, 0, fuzzyRainfallValue))
                    {
                        if (suitability.Months.Length > 0)
                            suitability.Months += "," + item.Name;
                        else
                            suitability.Months += item.Name;
                    }
                }
            }

            return graphData;
        }

        private bool IsSuitable(FuzzySuitabilityIntervalModel suitability, decimal temp, decimal rainfall)
        {
            if (temp > 0)
            {
                return temp >= suitability.Min && temp <= suitability.Max;
            }

            if (rainfall > 0)
            {
                return rainfall >= suitability.Min && rainfall <= suitability.Max;
            }

            return false;
        }

        public CropCalendar GetCropCalendar(int rice, DateTime startdate)
        {
            var cropCalendar = new CropCalendar();

            var sql = @"SELECT id,name,avgyield,maxyield,maturity,height,grainsize,
                            millingrecovery,eatingquality,notes, growthstage1,
                            growthstage2,growthstage3,growthstage4,growthstage5
                        FROM rice
                        WHERE id = @id";
            cropCalendar.RiceProfile = db.QuerySingleOrDefault<RiceModel>(sql,new { id = rice });

            // Create calendar
            var currentDate = startdate.AddDays(cropCalendar.RiceProfile.GrowthStage1);
            cropCalendar.CalendarData.Add(new Calendar("Nursery", startdate.ToLongDateString() + " to " + currentDate.ToLongDateString()));

            currentDate = startdate.AddDays(cropCalendar.RiceProfile.GrowthStage2);
            cropCalendar.CalendarData.Add(new Calendar("Preparation", startdate.ToLongDateString() + " to " + currentDate.ToLongDateString()));

            startdate = currentDate.AddDays(1);
            currentDate = startdate.AddDays(cropCalendar.RiceProfile.GrowthStage3);
            cropCalendar.CalendarData.Add(new Calendar("Planting to Panicle Initiation", startdate.ToLongDateString() + " to " + currentDate.ToLongDateString()));

            startdate = currentDate.AddDays(1);
            currentDate = startdate.AddDays(cropCalendar.RiceProfile.GrowthStage4);
            cropCalendar.CalendarData.Add(new Calendar("Panicle Initiation to Flowering", startdate.ToLongDateString() + " to " + currentDate.ToLongDateString()));

            startdate = currentDate.AddDays(1);
            currentDate = startdate.AddDays(cropCalendar.RiceProfile.GrowthStage5);
            cropCalendar.CalendarData.Add(new Calendar("Flowering to Maturity", startdate.ToLongDateString() + " to " + currentDate.ToLongDateString()));

            return cropCalendar;
        }

        private decimal GetFuzzyValue(decimal x, decimal a, decimal b, decimal c, decimal d)
        {
            decimal result = 0;
            if (x <= a)
            {
                result = 0;
            }
            else if ((a <= x) && (x <= b))
            {
                result = x - a;
            }
            else if ((b <= x) && (x <= c))
            {
                result = 1;
            }
            else if ((c <= x) && (x <= d))
            {
                result = (d - x) / (d - c);
            }
            else if (x <= d)
            {
                result = 0;
            }
            return result;
        }

    }
}
