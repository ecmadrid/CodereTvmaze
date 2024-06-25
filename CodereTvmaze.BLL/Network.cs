using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.BLL
{
    public class Network
    {
        public int id { get; set; }
        public string name { get; set; }
        public Country country { get; set; }
        public string officialSite { get; set; }

        public void AddToDatabaseIfNotExists(Connection connection)
        {
            string? countryCode = null;

            // Add Network country if it doesn't exist.
            if (country != null)
            {
                countryCode = country.code;
                country.AddToDatabaseIfNotExists(connection);
            }

            CodereTvmaze.DAL.Network.AddToDatabaseIfNotExists(connection, id, name, countryCode, officialSite);
        }

    }
}
