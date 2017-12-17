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
    public class TemperatureRepository
    {
        private System.Data.IDbConnection db;

        public TemperatureRepository(IConfiguration config)
        {
            db = new SqlConnection(config.GetConnectionString(ConfigConstants.DefaultConnection));
        }

        public IEnumerable<TemperatureModel> GetAll()
        {
            var sql = @"SELECT r.id, c.name as calendar, r.month, r.year, r.Temperature
                        FROM Temperature r
                        INNER JOIN calendar c ON r.month = c.id";
            var lists = db.Query<TemperatureModel>(sql);
            return lists;
        }

        public TemperatureModel GetById(int id)
        {
            var sql = @"SELECT r.id, c.name as calendar, r.month, r.year, r.Temperature
                        FROM Temperature r
                        INNER JOIN calendar c ON r.month = c.id
                        WHERE r.id = @id";
            var item = db.QueryFirst<TemperatureModel>(sql,new { id } );
            return item;
        }

        public IEnumerable<CalendarModel> GetLookup()
        {
            var sql = @"SELECT id,name FROM calendar";

            var lookup = db.Query<CalendarModel>(sql);
            return lookup;
        }

        public void New(TemperatureModel model)
        {
            var sql = @"INSERT INTO Temperature(month,year,Temperature)
                    VALUES (@month,@year,@Temperature)
                    SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, new
            {
                model.Month,
                model.Year,
                model.Temperature
            }).Single();
            model.Id = id;
        }

        public void Update(TemperatureModel model)
        {
            var sql = @"UPDATE Temperature SET
                            month = @month,
                            year = @year,
                            Temperature = @Temperature
                        WHERE id = @id";
            db.Execute(sql, new { model.Month,model.Year,model.Temperature,model.Id });
        }

        public void Delete(int id)
        {
            var sql = @"DELETE FROM Temperature WHERE id = @id";
            db.Execute(sql, new { id });
        }

    }
}
