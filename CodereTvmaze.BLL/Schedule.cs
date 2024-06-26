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
        public string time { get; set; }
        public List<string> days { get; set; }

        public void AddSchedule(Connection connection, long MainInfoId)
        {
            int pos = 1;
            foreach (string day in days)
            {
                CodereTvmaze.DAL.Schedule.AddSchedule(connection, MainInfoId, time, day, pos);
            }
        }

        public static Schedule GetScheduleByMainInfoId(long mainInfoId)
        {
            DataTable dtSchedule = CodereTvmaze.DAL.Schedule.GetScheduleByMainInfoId(mainInfoId);
            if (dtSchedule == null)
            {
                return null;
            }
            if (dtSchedule.Rows.Count < 1)
            {
                return null;
            }
            Schedule schedule = new Schedule();
            schedule.days = new List<string>();
            schedule.time = dtSchedule.Rows[0]["Time"].ToString();
            foreach (DataRow scheduleRow in dtSchedule.Rows)
            {
                schedule.days.Add(scheduleRow["Day"].ToString());
            }

            return schedule;
        }
    }
}
