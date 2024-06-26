using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.BLL
{
    public class Network
    {
        public long id { get; set; }
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

        public static Network GetNetworkById(long id)
        {
            DataRow netwotkRow = CodereTvmaze.DAL.Network.GetNetworkById(id);

            if (netwotkRow == null)
            {
                return null;
            }

            Network network = new Network()
            {
                id = Convert.ToInt64(netwotkRow["Id"].ToString()),
                name = netwotkRow["Name"] == DBNull.Value ? null : netwotkRow["Name"].ToString(),
                officialSite = netwotkRow["OfficialSite"] == DBNull.Value ? null : netwotkRow["OfficialSite"].ToString(),
            };

            if (netwotkRow["CountryCode"] != DBNull.Value)
            {
                network.country = Country.GetCountryByCode(netwotkRow["CountryCode"].ToString());

            }

            return network;
        }

    }
}
