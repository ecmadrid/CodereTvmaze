using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.BLL
{
    public class Country
    {
        public string name { get; set; }
        public string code { get; set; }
        public string timezone { get; set; }

        public void AddToDatabaseIfNotExists(Connection connection)
        {
            CodereTvmaze.DAL.Country.AddToDatabaseIfNotExists(connection, name, code, timezone);
        }
    }
}
