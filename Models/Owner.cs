using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace OwnerPostApi
{
    public class OwnerPost
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string DriversLicence { get; set; }

        internal AppDb Db { get; set; }

        public OwnerPost()
        {
        }

        internal OwnerPost(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `OwnerPost` (`LastName`, `DriversLicence`) VALUES (@LastName, @DriversLicence);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `OwnerPost` SET `LastName` = @LastName, `DriversLicence` = @DriversLicence WHERE `Id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `OwnerPost` WHERE `Id` = @id;";
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
                ParameterName = "@LastName",
                DbType = DbType.String,
                Value = LastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@DriversLicence",
                DbType = DbType.String,
                Value = DriversLicence,
            });
        }
    }
}