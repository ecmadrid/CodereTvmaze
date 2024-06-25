using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.BLL
{
    public class Schedule
    {
        public string time { get; set; }
        public List<string> days { get; set; }

        public void AddSchedule(Connection connection, int MainInfoId)
        {
            int pos = 1;
            foreach(string day in days)
            {
                CodereTvmaze.DAL.Schedule.AddSchedule(connection, MainInfoId, time, day, pos);
            }
        }
    }
}
