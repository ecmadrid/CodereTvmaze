using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.BLL
{
    public class Schedule
    {
        public string? time { get; set; }
        public List<string>? days { get; set; }

        /// <summary>
        /// Add schedule data related a MainInfo object.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="MainInfoId"></param>
        public void AddSchedule(DatabaseConnection connection, long MainInfoId)
        {
            if (days != null)
            {
                int pos = 1;
                foreach (string day in days)
                {
                    CodereTvmaze.DAL.Schedule.AddSchedule(connection, MainInfoId, time, day, pos);
                }
            }
        }

        /// <summary>
        /// This method return a Schedule object related to a MainInfo object or Null object if it's not exist.
        /// </summary>
        /// <param name="mainInfoId"></param>
        /// <returns></returns>
        public static Schedule GetScheduleByMainInfoId(long mainInfoId)
        {
            DataTable scheduleTable = CodereTvmaze.DAL.Schedule.GetScheduleByMainInfoId(mainInfoId);
            if (scheduleTable == null)
            {
                return null;
            }
            if (scheduleTable.Rows.Count < 1)
            {
                return null;
            }
            Schedule schedule = new Schedule();
            schedule.days = new List<string>();
            schedule.time = scheduleTable.Rows[0]["Time"].ToString();
            foreach (DataRow scheduleRow in scheduleTable.Rows)
            {
                if (scheduleRow["Day"] != DBNull.Value){
                    schedule.days.Add(scheduleRow["Day"].ToString());
                }
            }

            return schedule;
        }
    }
}
