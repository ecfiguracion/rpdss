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
            var sql = @"SELECT id,name,avgyield,maxyield,maturity,height,grainsize,
                            millingrecovery,eatingquality,notes, growthstage1,
                            growthstage2,growthstage3,growthstage4,growthstage5
                        FROM rice";
            var lists = db.Query<RiceModel>(sql);
            return lists;
        }

        public RiceModel GetById(int id)
        {
            var sql = @"SELECT id,name,avgyield,maxyield,maturity,height,grainsize,
                            millingrecovery,eatingquality,notes, growthstage1,
                            growthstage2,growthstage3,growthstage4,growthstage5
                        FROM rice
                        WHERE id = @id";
            var item = db.QueryFirst<RiceModel>(sql,new { id } );
            return item;
        }

        public void New(RiceModel model)
        {
            var sql = @"INSERT INTO rice(name,avgyield,maxyield,maturity,height,grainsize,
                            millingrecovery,eatingquality,notes, growthstage1,
                            growthstage2,growthstage3,growthstage4,growthstage5)
                    VALUES (@name,@avgyield,@maxyield,@maturity,@height,@grainsize,
                            @millingrecovery,@eatingquality,@notes, @growthstage1,
                            @growthstage2,@growthstage3,@growthstage4,@growthstage5)
                    SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, new
            {
                model.Name,
                model.AvgYield,
                model.MaxYield,
                model.Maturity,
                model.Height,
                model.GrainSize,
                model.MillingRecovery,
                model.EatingQuality,
                model.Notes,
                model.GrowthStage1,
                model.GrowthStage2,
                model.GrowthStage3,
                model.GrowthStage4,
                model.GrowthStage5
            }).Single();
            model.Id = id;
        }

        public void Update(RiceModel model)
        {
            var sql = @"UPDATE rice SET
                            name = @name,
                            avgyield = @avgyield,
                            maxyield = @maxyield,
                            maturity = @maturity,
                            height = @height,
                            grainsize = @grainsize,
                            millingrecovery = @millingrecovery,
                            eatingquality = @eatingquality,
                            notes = @notes,
                            growthstage1 = @growthstage1,
                            growthstage2 = @growthstage2,
                            growthstage3 = @growthstage3,
                            growthstage4 = @growthstage4,
                            growthstage5 = @growthstage5
                        WHERE id = @id";
            db.Execute(sql, new {
                model.Name,
                model.AvgYield,
                model.MaxYield,
                model.Maturity,
                model.Height,
                model.GrainSize,
                model.MillingRecovery,
                model.EatingQuality,
                model.Notes,
                model.GrowthStage1,
                model.GrowthStage2,
                model.GrowthStage3,
                model.GrowthStage4,
                model.GrowthStage5,
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
