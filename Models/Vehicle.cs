using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace VehiclePostApi
{
    public class VehiclePost
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Vin { get; set; }

        internal AppDb Db { get; set; }

        public VehiclePost()
        {
        }

        internal VehiclePost(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `VehiclePost` (`Brand`, `Vin`) VALUES (@Brand, @Vin);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `VehiclePost` SET `Brand` = @Brand, `Vin` = @Vin WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `VehiclePost` WHERE `Id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Brand",
                DbType = DbType.String,
                Value = Brand,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Vin",
                DbType = DbType.String,
                Value = Vin,
            });
        }
    }
}