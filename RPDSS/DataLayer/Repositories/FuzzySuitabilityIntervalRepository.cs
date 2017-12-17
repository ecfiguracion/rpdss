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
    public class FuzzySuitabilityIntervalRepository
    {
        private System.Data.IDbConnection db;

        public FuzzySuitabilityIntervalRepository(IConfiguration config)
        {
            db = new SqlConnection(config.GetConnectionString(ConfigConstants.DefaultConnection));
        }

        public IEnumerable<FuzzySuitabilityIntervalModel> GetAll()
        {
            var sql = @"SELECT id,name,min,max
                        FROM fuzzysuitabilityinterval";
            var lists = db.Query<FuzzySuitabilityIntervalModel>(sql);
            return lists;
        }

        public FuzzySuitabilityIntervalModel GetById(int id)
        {
            var sql = @"SELECT id,name,min,max
                        FROM fuzzysuitabilityinterval
                        WHERE id = @id";
            var item = db.QueryFirst<FuzzySuitabilityIntervalModel>(sql,new { id } );
            return item;
        }

        public void New(FuzzySuitabilityIntervalModel model)
        {
            var sql = @"INSERT INTO FuzzySuitabilityInterval(name,min,max)
                        VALUES (@name,@min,@max)
                        SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, new
            {
                model.Name,
                model.Min,
                model.Max
            }).Single();
            model.Id = id;
        }

        public void Update(FuzzySuitabilityIntervalModel model)
        {
            var sql = @"UPDATE FuzzySuitabilityInterval SET
                            name = @name,
                            min = @min,
                            max = @max
                        WHERE id = @id";
            db.Execute(sql, new {
                model.Name,
                model.Min,
                model.Max,
                model.Id
            });
        }

        public void Delete(int id)
        {
            var sql = @"DELETE FROM FuzzySuitabilityInterval WHERE id = @id";
            db.Execute(sql, new { id });
        }

    }
}
