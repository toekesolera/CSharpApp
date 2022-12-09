using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace OwnerPostApi
{
    public class OwnerQuery
    {
        public AppDb Db { get; }

        public OwnerQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<OwnerPost> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `LastName`, `DriversLicence` FROM `OwnerPost` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<OwnerPost>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `LastName`, `DriversLicence` FROM `OwnerPost` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `OwnerPost`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<OwnerPost>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<OwnerPost>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new OwnerPost(Db)
                    {
                        Id = reader.GetInt32(0),
                        LastName = reader.GetString(1),
                        DriversLicence = reader.GetString(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}