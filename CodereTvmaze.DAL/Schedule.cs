using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    public class Schedule
    {
        public static void AddSchedule(Connection connection, int mainInfoId, string time, string day, int pos)
        {
            bool needCloseConnection = false;
            if (connection == null)
            {
                connection = new DAL.Connection();
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

        public static void DeleteMainInfoSchedule(Connection connection, int mainInfoId)
        {
            string sql = @"DELETE FROM Schedules Where MainInfoId = @MainInfoId";
            sql = sql.Replace("@MainInfoId", mainInfoId.ToString());
            connection.ExecuteNonQuery(sql);
        }
    }
}
