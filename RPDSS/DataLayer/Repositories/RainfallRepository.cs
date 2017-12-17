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
    public class RainfallRepository
    {
        private System.Data.IDbConnection db;

        public RainfallRepository(IConfiguration config)
        {
            db = new SqlConnection(config.GetConnectionString(ConfigConstants.DefaultConnection));
        }

        public IEnumerable<RainfallModel> GetAll()
        {
            var sql = @"SELECT r.id, c.name as calendar, r.month, r.year, r.rainfall
                        FROM rainfall r
                        INNER JOIN calendar c ON r.month = c.id";
            var lists = db.Query<RainfallModel>(sql);
            return lists;
        }

        public RainfallModel GetById(int id)
        {
            var sql = @"SELECT r.id, c.name as calendar, r.month, r.year, r.rainfall
                        FROM rainfall r
                        INNER JOIN calendar c ON r.month = c.id
                        WHERE r.id = @id";
            var item = db.QueryFirst<RainfallModel>(sql,new { id } );
            return item;
        }

        public IEnumerable<CalendarModel> GetLookup()
        {
            var sql = @"SELECT id,name FROM calendar";

            var lookup = db.Query<CalendarModel>(sql);
            return lookup;
        }

        public void New(RainfallModel model)
        {
            var sql = @"INSERT INTO rainfall(month,year,rainfall)
                    VALUES (@month,@year,@rainfall)
                    SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, new
            {
                model.Month,
                model.Year,
                model.Rainfall
            }).Single();
            model.Id = id;
        }

        public void Update(RainfallModel model)
        {
            var sql = @"UPDATE rainfall SET
                            month = @month,
                            year = @year,
                            rainfall = @rainfall
                        WHERE id = @id";
            db.Execute(sql, new { model.Month,model.Year,model.Rainfall,model.Id });
        }

        public void Delete(int id)
        {
            var sql = @"DELETE FROM rainfall WHERE id = @id";
            db.Execute(sql, new { id });
        }

    }
}
