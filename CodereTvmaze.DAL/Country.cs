using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    /// <summary>
    /// Class <c>Country</c> Manages Countries table in database.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Add a new record into table if it doesn't exist. Code is the record identifier.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="timezone"></param>
        public static void AddToDatabaseIfNotExists(DatabaseConnection connection, string? name, string? code, string? timezone)
        {
            bool needCloseConnection = false;
            if (connection == null)
            {
                connection = new DAL.DatabaseConnection();
                connection.Open();
                connection.Begin();
                needCloseConnection = true;
            }

            string sql = @"SELECT COUNT(*) FROM Countries WHERE code = '" + code + "'";
            long? count = connection.ExecuteLongScalar(sql);
            if ((count != null) && (count > 0))
            {
                // Country exists in database.
                if (needCloseConnection)
                {
                    connection.Close();
                }
                return;
            }

            // Country doesn't exist in database. We add it.
            sql = @"INSERT INTO Countries (Name, Code, Timezone) VALUES( @Name, @Code, @Timezone)";
            sql = sql.Replace("@Name", name == null ? "NULL" : "'" + name.Replace("'", "''") + "'");
            sql = sql.Replace("@Code", code == null ? "NULL" : "'" + code.Replace("'", "''") + "'");
            sql = sql.Replace("@Timezone", timezone == null ? "NULL" : "'" + timezone.Replace("'", "''") + "'");
            connection.ExecuteNonQuery(sql);

            if (needCloseConnection)
            {
                connection.Commit();
                connection.Close();
            }


        }

        /// <summary>
        /// Returns a datarow object with a record fron Countries table with code indicated. Null if not found.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DataRow GetCountryByCode(string? code)
        {

            DatabaseConnection connection = connection = new DAL.DatabaseConnection();
            connection.Open();
            string sql = @"SELECT * FROM Countries WHERE Code = '" + code + "'";
            DataTable dt = connection.Execute(sql);

            connection.Close();

            if (dt == null)
            {
                return null;
            }

            if (dt.Rows.Count < 1)
            {
                return null;
            };

            return dt.Rows[0];
        }

        /// <summary>
        /// Retrieves all Countries table records. Null if table is empty.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAll()
        {

            DatabaseConnection connection = connection = new DAL.DatabaseConnection();
            connection.Open();
            string sql = @"SELECT * FROM Countries";
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
