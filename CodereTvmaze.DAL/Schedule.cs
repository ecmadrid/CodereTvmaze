using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    /// <summary>
    /// Class <c>Schedule</c> Manages record in Schedules table in database.
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// Method to add a new record into Schedules table if it doesn't exist based on its associated main info id field.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mainInfoId"></param>
        /// <param name="time"></param>
        /// <param name="day"></param>
        /// <param name="pos"></param>
        public static void AddSchedule(DatabaseConnection connection, long mainInfoId, string? time, string? day, int pos)
        {
            bool needCloseConnection = false;
            if (connection == null)
            {
                connection = new DAL.DatabaseConnection();
                connection.Open();
                connection.Begin();
                needCloseConnection = true;
            }

            string sql = @"INSERT INTO Schedules (MainInfoId, Time, Day, Pos) VALUES( @MainInfoId, @Time, @Day, @Pos)";
            sql = sql.Replace("@MainInfoId", mainInfoId.ToString());
            sql = sql.Replace("@Time", time == null ? "NULL" : "'" + time + "'");
            sql = sql.Replace("@Day", day == null ? "NULL" : "'" + day + "'");
            sql = sql.Replace("@Pos", pos.ToString());
            connection.ExecuteNonQuery(sql);

            if (needCloseConnection)
            {
                connection.Commit();
                connection.Close();
            }
        }

        /// <summary>
        /// Delete a record in table Schedules based on its associated main info id.
        /// Null object is returned if there aren't records with criteria.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mainInfoId"></param>
        public static void DeleteMainInfoSchedule(DatabaseConnection connection, long mainInfoId)
        {
            string sql = @"DELETE FROM Schedules Where MainInfoId = @MainInfoId";
            sql = sql.Replace("@MainInfoId", mainInfoId.ToString());
            connection.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// This method returns a datatable with records from table Schedules based on theirs associated main info id.
        /// Null object is returned if there aren't records with criteria.
        /// </summary>
        /// <param name="mainInfoId"></param>
        /// <returns></returns>
        public static DataTable GetScheduleByMainInfoId(long mainInfoId)
        {

            DatabaseConnection connection = connection = new DAL.DatabaseConnection();
            connection.Open();
            string sql = @"SELECT * FROM Schedules WHERE MainInfoId =" + mainInfoId.ToString() + " ORDER BY Pos";
            DataTable dt = connection.Execute(sql);

            connection.Close();

            return dt;
        }

        /// <summary>
        /// Returns all records in  table Schedules.
        /// Null object is returned if there aren't records in table.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAll()
        {

            DatabaseConnection connection = connection = new DAL.DatabaseConnection();
            connection.Open();
            string sql = @"SELECT * FROM Schedules ORDER BY MainInfoId, Pos";
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
