using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    public class Country
    {
        public static void AddToDatabaseIfNotExists(Connection connection, string name, string code, string timezone)
        {
            bool needCloseConnection = false;
            if (connection == null)
            {
                connection = new DAL.Connection();
                connection.Open();
                connection.Begin();
                needCloseConnection = true;
            }

            string sql = @"SELECT COUNT(*) FROM Countries WHERE code = '" + code + "'";
                long count = connection.ExecuteScalar(sql);
                if (count > 0)
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
                sql = sql.Replace("@Name", name == null ? "NULL" : "'" + name + "'");
                sql = sql.Replace("@Code","'" + code + "'");
                sql = sql.Replace("@Timezone", timezone == null ? "NULL" : "'" + timezone + "'");
                connection.ExecuteNonQuery(sql);

            if (needCloseConnection)
            {
                connection.Commit();
                connection.Close();
            }


        }

        public static DataRow GetCountryByCode(string code)
        {

            Connection connection = connection = new DAL.Connection();
            connection.Open();
            string sql = @"SELECT * FROM Countries WHERE Code = '" +code + "'";
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

        public static DataTable GetAll()
        {

            Connection connection = connection = new DAL.Connection();
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
