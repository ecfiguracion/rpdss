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
    public class FuzzyParameterRepository
    {
        private System.Data.IDbConnection db;

        public FuzzyParameterRepository(IConfiguration config)
        {
            db = new SqlConnection(config.GetConnectionString(ConfigConstants.DefaultConnection));
        }

        public IEnumerable<FuzzyParameterModel> GetAll()
        {
            var sql = @"SELECT fp.id,fp.type,fp.growthstages,gs.name growthstagesname,fp.absolutemin,
	                        fp.optimummin,fp.optimummax,fp.absolutemax
                        FROM fuzzyparameter fp 
                        INNER JOIN growthstages gs ON fp.growthstages = gs.id";
            var lists = db.Query<FuzzyParameterModel>(sql);
            return lists;
        }

        public FuzzyParameterModel GetById(int id)
        {
            var sql = @"SELECT fp.id,fp.type,fp.growthstages,gs.name growthstagesname,fp.absolutemin,
	                        fp.optimummin,fp.optimummax,fp.absolutemax
                        FROM fuzzyparameter fp 
                        INNER JOIN growthstages gs ON fp.growthstages = gs.id
                        WHERE fp.id = @id";
            var item = db.QueryFirst<FuzzyParameterModel>(sql,new { id } );
            return item;
        }

        public IEnumerable<GrowthStagesModel> GetLookup()
        {
            var sql = @"SELECT id,name FROM growthstages";

            var lookup = db.Query<GrowthStagesModel>(sql);
            return lookup;
        }

        public void New(FuzzyParameterModel model)
        {
            var sql = @"INSERT INTO FuzzyParameter(type,growthstages,absolutemin,optimummin,optimummax,absolutemax)
                    VALUES (@type,@growthstages,@absolutemin,@optimummin,@optimummax,@absolutemax)
                    SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, new
            {
                model.Type,
                model.GrowthStages,
                model.AbsoluteMin,
                model.OptimumMin,
                model.OptimumMax,
                model.AbsoluteMax
            }).Single();
            model.Id = id;
        }

        public void Update(FuzzyParameterModel model)
        {
            var sql = @"UPDATE FuzzyParameter SET
                            type            = @type,
                            growthstages    = @growthstages,
                            absolutemin     = @absolutemin,
                            optimummin      = @optimummin,
                            optimummax      = @optimummax,
                            absolutemax     = @absolutemax
                        WHERE id = @id";
            db.Execute(sql, new {
                model.Type,
                model.GrowthStages,
                model.AbsoluteMin,
                model.OptimumMin,
                model.OptimumMax,
                model.AbsoluteMax,
                model.Id
            });
        }

        public void Delete(int id)
        {
            var sql = @"DELETE FROM FuzzyParameter WHERE id = @id";
            db.Execute(sql, new { id });
        }

    }
}
