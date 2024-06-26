using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static Country GetCountryByCode(string code)
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
