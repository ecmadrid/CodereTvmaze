using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.BLL
{
    public class WebChannel
    {
        public int id { get; set; }
        public string name { get; set; }
        public Country country { get; set; }
        public string officialSite { get; set; }

        public void AddToDatabaseIfNotExists(Connection connection)
        {

            // Add Web channel country if it doesn't exist.

            string? countryCode = null;

            if (country != null)
            {
                countryCode = country.code;
                country.AddToDatabaseIfNotExists(connection);
            }

            CodereTvmaze.DAL.WebChannels.AddToDatabaseIfNotExists(connection, id, name, countryCode, officialSite);
        }
    }
}
