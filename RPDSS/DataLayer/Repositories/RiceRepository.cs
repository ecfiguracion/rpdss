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
    public class RiceRepository
    {
        private System.Data.IDbConnection db;

        public RiceRepository(IConfiguration config)
        {
            db = new SqlConnection(config.GetConnectionString(ConfigConstants.DefaultConnection));
        }

        public IEnumerable<RiceModel> GetAll()
        {
            var sql = @"SELECT id,name,minrainfall,maxrainfall,mintemperature,
                            maxtemperature,maturity,soiltype
                        FROM rice";
            var lists = db.Query<RiceModel>(sql);
            return lists;
        }

        public RiceModel GetById(int id)
        {
            var sql = @"SELECT id,name,minrainfall,maxrainfall,mintemperature,
                            maxtemperature,maturity,soiltype
                        FROM rice
                        WHERE id = @id";
            var item = db.QueryFirst<RiceModel>(sql,new { id } );
            return item;
        }

        public void New(RiceModel model)
        {
            var sql = @"INSERT INTO rice(name,minrainfall,maxrainfall,mintemperature,maxtemperature,maturity,soiltype)
                    VALUES (@name,@minrainfall,@maxrainfall,@mintemperature,@maxtemperature,@maturity,@soiltype)
                    SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, new
            {
                model.Name,
                model.MinRainfall,
                model.MaxRainfall,
                model.MinTemperature,
                model.MaxTemperature,
                model.Maturity,
                model.SoilType
            }).Single();
            model.Id = id;
        }

        public void Update(RiceModel model)
        {
            var sql = @"UPDATE rice SET
                            name = @name,
                            minrainfall = @minrainfall,
                            maxrainfall = @maxrainfall,
                            mintemperature = @mintemperature,
                            maxtemperature = @maxtemperature,
                            maturity = @maturity,
                            soiltype = @soiltype
                        WHERE id = @id";
            db.Execute(sql, new {
                model.Name,
                model.MinRainfall,
                model.MaxRainfall,
                model.MinTemperature,
                model.MaxTemperature,
                model.Maturity,
                model.SoilType,
                model.Id
            });
        }

        public void Delete(int id)
        {
            var sql = @"DELETE FROM rice WHERE id = @id";
            db.Execute(sql, new { id });
        }

    }
}
