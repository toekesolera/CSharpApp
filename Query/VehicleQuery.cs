using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace VehiclePostApi
{
    public class VehicleQuery
    {
        public AppDb Db { get; }

        public VehicleQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<VehiclePost> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Brand`, `Vin` FROM `VehiclePost` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<VehiclePost>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Brand`, `Vin` FROM `VehiclePost` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `VehiclePost`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<VehiclePost>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<VehiclePost>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new VehiclePost(Db)
                    {
                        Id = reader.GetInt32(0),
                        Brand = reader.GetString(1),
                        Vin = reader.GetString(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}