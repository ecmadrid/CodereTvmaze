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
    /// Claass <c>Country</c>
    /// </summary>
    public class Country
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public string? timezone { get; set; }

        /// <summary>
        /// Add a new record to Countries database table from object if there's not yet a record whith same code.
        /// </summary>
        /// <param name="connection"></param>
        public void AddToDatabaseIfNotExists(DatabaseConnection connection)
        {
            CodereTvmaze.DAL.Country.AddToDatabaseIfNotExists(connection, name, code, timezone);
        }

        /// <summary>
        /// Creates a Country object with data stored in table based on its code. Null if code is not found.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Country GetCountryByCode(string? code)
        {
            DataRow countryRow = CodereTvmaze.DAL.Country.GetCountryByCode(code);

            if (countryRow == null)
            {
                return null;
            }

            Country country = new Country()
            {
                code = countryRow["Code"].ToString(),
                name = countryRow["Name"] == DBNull.Value ? null : countryRow["Name"].ToString(),
                timezone = countryRow["Timezone"] == DBNull.Value ? null : countryRow["Timezone"].ToString(),
            };

            return country;
        }
    }
}
