using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    public class WebChannel
    {
        public static void AddToDatabaseIfNotExists(Connection connection, long id, string name, string? countryCode, string officialSite)
        {
            bool needCloseConnection = false;
            if (connection == null)
            {
                connection = new DAL.Connection();
                connection.Open();
                connection.Begin();
                needCloseConnection = true;
            }
            string sql = @"SELECT COUNT(*) FROM WebChannels WHERE id = " + id.ToString();
            long count = connection.ExecuteScalar(sql);
            if (count > 0)
            {
                // Web channel exists in database.
                if (needCloseConnection)
                {
                    connection.Close();
                }
                return;
            }

            // Web channel doesn't exist in database. We add it.

            sql = @"INSERT INTO WebChannels (Id, Name, CountryCode, OfficialSite) VALUES( @Id, @Name, @CountryCode, @OfficialSite)";
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

        public static DataRow GetWebChannelById(long id)
        {

            Connection connection = connection = new DAL.Connection();
            connection.Open();
            string sql = @"SELECT * FROM WebChannels WHERE Id =" + id.ToString();
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
            string sql = @"SELECT * FROM WebChannels";
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
