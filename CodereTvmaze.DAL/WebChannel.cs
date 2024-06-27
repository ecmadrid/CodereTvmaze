using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{

    /// <summary>
    /// Class <c>WebChannel</c> Manages records from WebChannes table in database.
    /// </summary>
    public class WebChannel
    {
        /// <summary>
        /// This method add a new record into WebChannels table based on id field if there isn't a record yet with same id.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="countryCode"></param>
        /// <param name="officialSite"></param>
        public static void AddToDatabaseIfNotExists(DatabaseConnection connection, long id, string? name, string? countryCode, string? officialSite)
        {
            bool needCloseConnection = false;
            if (connection == null)
            {
                connection = new DAL.DatabaseConnection();
                connection.Open();
                connection.Begin();
                needCloseConnection = true;
            }
            string sql = @"SELECT COUNT(*) FROM WebChannels WHERE id = " + id.ToString();
            long? count = connection.ExecuteLongScalar(sql);

            if ((count != null) && (count > 0))
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
            sql = sql.Replace("@Name", name == null ? "NULL" : "'" + name.Replace("'", "''") + "'");
            sql = sql.Replace("@CountryCode", countryCode == null ? "NULL" : "'" + countryCode + "'");
            sql = sql.Replace("@OfficialSite", officialSite == null ? "NULL" : "'" + officialSite.Replace("'", "''") + "'");
            connection.ExecuteNonQuery(sql);

            if (needCloseConnection)
            {
                connection.Commit();
                connection.Close();
            }
        }

        /// <summary>
        /// Returns a record from WebChannels table with id passed as parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataRow GetWebChannelById(long id)
        {

            DatabaseConnection connection = connection = new DAL.DatabaseConnection();
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

        /// <summary>
        /// This method returns a datatable with all records from WebChannels table. If there idn't records it returns a null object.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAll()
        {

            DatabaseConnection connection = connection = new DAL.DatabaseConnection();
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
