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
    public class UserAccountRepository
    {
        private System.Data.IDbConnection db;

        public UserAccountRepository(IConfiguration config)
        {
            db = new SqlConnection(config.GetConnectionString(ConfigConstants.DefaultConnection));
        }

        public IEnumerable<UserAccountModel> GetAll()
        {
            var sql = @"SELECT id, name, username, password
                        FROM useraccount";
            var lists = db.Query<UserAccountModel>(sql);
            return lists;
        }

        public UserAccountModel GetById(int id)
        {
            var sql = @"SELECT id, name, username, password
                        FROM useraccount
                        WHERE id=@id";
            var item = db.QueryFirst<UserAccountModel>(sql,new { id } );
            return item;
        }

        public void New(UserAccountModel model)
        {
            var sql = @"INSERT INTO useraccount(name, username, password)
                        VALUES (@name, @username, @password)
                        SELECT CAST(SCOPE_IDENTITY() AS int)";
            var id = db.Query<int>(sql, new
            {
                model.Name,
                model.UserName,
                model.Password
            }).Single();
            model.Id = id;
        }
        public void Update(UserAccountModel model)
        {
            var sql = @"UPDATE useraccount SET
                            name = @name,
                            username = @username,
                            password = @password
                        WHERE id = @id";
            db.Execute(sql, new { model.Name,
                model.UserName,
                model.Password,
                model.Id});
        }

        public void Delete(int id)
        {
            var sql = @"DELETE FROM username WHERE id = @id";
            db.Execute(sql, new { id });
        }
    }
}
