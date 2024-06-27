using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.BLL
{
    /// <summary>
    /// Class <c></c> contains data from Networks database table related to a MainInfo object.
    /// </summary>
    public class Network
    {
        public long id { get; set; }
        public string? name { get; set; }
        public Country? country { get; set; }
        public string? officialSite { get; set; }

        /// <summary>
        /// Add a new record into database Networks table if it's no exists yet based on its id.
        /// </summary>
        /// <param name="connection"></param>
        public void AddToDatabaseIfNotExists(DatabaseConnection connection)
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

        /// <summary>
        /// This method retun a Network object from data in database based on a id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
