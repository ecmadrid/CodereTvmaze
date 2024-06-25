﻿using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    public class WebChannels
    {
        public static void AddToDatabaseIfNotExists(Connection connection, int id, string name, string? countryCode, string officialSite)
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
    }
}
