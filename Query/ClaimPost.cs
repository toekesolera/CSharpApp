using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace ClaimPostApi
{
    public class ClaimQuery
    {
        public AppDb Db { get; }

        public ClaimQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<ClaimPost> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Description`, `Status` FROM `ClaimPost` WHERE `Id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<ClaimPost>> LatestPostsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Description`, `Status` FROM `ClaimPost` ORDER BY `Id` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ClaimPost`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<ClaimPost>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<ClaimPost>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new ClaimPost(Db)
                    {
                        Id = reader.GetInt32(0),
                        Description = reader.GetString(1),
                        Status = reader.GetString(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}