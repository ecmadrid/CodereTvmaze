using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    public class Network
    {
        public static void AddToDatabaseIfNotExists(Connection connection, long id, string name, string countryCode, string officialSite)
        {
            bool needCloseConnection = false;
            if (connection == null)
            {
                connection = new DAL.Connection();
                connection.Open();
                connection.Begin();
                needCloseConnection = true;
            }

                string sql = @"SELECT COUNT(*) FROM Networks WHERE id = " + id.ToString();
                long count = connection.ExecuteScalar(sql);
                if (count > 0)
                {
                // Network exists in database.
                if (needCloseConnection)
                {
                    connection.Close();
                }
                return;
                }

                // Network doesn't exist in database. We add it.

                sql = @"INSERT INTO Networks (Id, Name, CountryCode, OfficialSite) VALUES( @Id, @Name, @CountryCode, @OfficialSite)";
                sql = sql.Replace("@Id", id.ToString());
                sql = sql.Replace("@Name", name == null ? "NULL" : "'" + name + "'");
                sql = sql.Replace("@CountryCode", countryCode == null ? "NULL" : "'" + countryCode + "'");
                sql = sql.Replace("@OfficialSite", officialSite == null ? "NULL" : "'" + officialSite + "'");
                connection.ExecuteNonQuery(sql);

            if (needCloseConnection)
            {
                connection.Commit();
                connection.Close();
            }

        }

        public static DataRow GetNetworkById(long id)
        {

            Connection connection = connection = new DAL.Connection();
            connection.Open();
            string sql = @"SELECT * FROM Networks WHERE Id =" + id.ToString();
            DataTable dt = connection.Execute(sql);

            connection.Close();

            if (dt == null)
            {
                return null;
            }

            if (dt.Rows.Count < 1)
            {
                return null;
            }

            return dt.Rows[0];
        }

        public static DataTable GetAll()
        {

            Connection connection = connection = new DAL.Connection();
            connection.Open();
            string sql = @"SELECT * FROM Networks";
            DataTable dt = connection.Execute(sql);

            connection.Close();

            if (dt.Rows.Count < 1)
            {
                return null;
            };

            return dt;
        }
    }
}
