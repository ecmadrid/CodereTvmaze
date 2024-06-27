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
    /// Class <c>WebChannel</c> contains data from WebChannels database table related to a MainInfo object.
    /// </summary>
    public class WebChannel
    {
        public long id { get; set; }
        public string? name { get; set; }
        public Country? country { get; set; }
        public string? officialSite { get; set; }

        /// <summary>
        /// Adds a new record into database WebChannels table if it's no exists yet based on its id.
        /// </summary>
        /// <param name="connection"></param>
        public void AddToDatabaseIfNotExists(DatabaseConnection connection)
        {

            // Add Web channel country if it doesn't exist.

            string? countryCode = null;

            if (country != null)
            {
                countryCode = country.code;
                country.AddToDatabaseIfNotExists(connection);
            }

            CodereTvmaze.DAL.WebChannel.AddToDatabaseIfNotExists(connection, id, name, countryCode, officialSite);
        }

        public static WebChannel GetWebChannelkById(long id)
        {
            DataRow webChannelRow = CodereTvmaze.DAL.WebChannel.GetWebChannelById(id);

            if (webChannelRow == null)
            {
                return null;
            }

            WebChannel webChannel = new WebChannel()
            {
                id = Convert.ToInt64(webChannelRow["Id"].ToString()),
                name = webChannelRow["Name"] == DBNull.Value ? null : webChannelRow["Name"].ToString(),
                officialSite = webChannelRow["OfficialSite"] == DBNull.Value ? null : webChannelRow["OfficialSite"].ToString(),
            };

            if (webChannelRow["CountryCode"] != DBNull.Value)
            {
                webChannel.country = Country.GetCountryByCode(webChannelRow["CountryCode"].ToString());

            }

            return webChannel;
        }
    }
}
