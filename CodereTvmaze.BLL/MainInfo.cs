using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CodereTvmaze.BLL
{
    /// <summary>
    /// Class <c>MainInfo</c> contains all information related to a MainInfo object (with its related object included).
    /// </summary>
    public class MainInfo
    {
        public long id { get; set; }
        public string? url { get; set; }
        public string? name { get; set; }
        public string? type { get; set; }
        public string? language { get; set; }
        public List<string>? genres { get; set; }
        public string? status { get; set; }
        public long? runtime { get; set; }
        public long? averageRuntime { get; set; }
        public DateOnly? premiered { get; set; }
        public DateOnly? ended { get; set; }
        public string? officialSite { get; set; }
        public Schedule? schedule { get; set; }
        public Rating? rating { get; set; }
        public long? weight { get; set; }
        public Network? network { get; set; }
        public WebChannel? webChannel { get; set; }
        public Country? dvdCountry { get; set; }
        public Externals? externals { get; set; }
        public Image? image { get; set; }
        public string? summary { get; set; }
        public long? updated { get; set; }
        public Links? _links { get; set; }

        // Methods

        /// <summary>
        /// This method imports data of a MainInfo object from Tvmaze and insert data into datatable.
        /// It calls a endpoint in Tvmaze web api to retrieve all information needed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MainInfo ImportMainInfo(int id)
        {

            using (var http = new HttpClient())
            {
                var endpoint = "https://api.tvmaze.com/shows/" + id.ToString();
                var result = http.GetAsync(endpoint).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    var obj = JsonSerializer.Deserialize<MainInfo>(json);

                    if (obj != null)
                    {
                        obj.AddToDatabase();

                        return obj;
                    }
                }

                return null;
            };
        }

        /// <summary>
        /// This method imports data  from Tvmaze and insert data into datatable.
        /// It calls a endpoint in Tvmaze web api to retrieve all information needed.
        /// Calls are done paginated calculating first page needed until return response is "not found".
        /// </summary>
        /// <returns></returns>
        public static bool ImportMainInfoAll()
        {
            bool rs = true;

            try
            {
                using (var http = new HttpClient())
                {
                    decimal page = 0;

                    // Get last inserted id.
                    long lastId = CodereTvmaze.DAL.MainInfo.GetLastId();

                    // Determinate start page number.
                    page = Math.Floor(Convert.ToDecimal(lastId / 250));
                    bool hasResult = true;

                    do
                    {
                        var endpoint = "https://api.tvmaze.com/shows?page=" + page.ToString();
                        var result = http.GetAsync(endpoint).Result;
                        if (result.StatusCode == HttpStatusCode.NotFound)
                        {
                            hasResult = false;
                        }

                        if (hasResult)
                        {

                            var json = result.Content.ReadAsStringAsync().Result;
                            var objs = JsonSerializer.Deserialize<List<MainInfo>>(json);

                            foreach (var obj in objs)
                            {
                                obj.AddToDatabase();
                            }

                            page++;
                        }
                    } while (hasResult);
                };
            }
            catch (Exception ex)
            {
                // Here we can treat exception.
                rs = false;
            }
            return rs;
        }

        /// <summary>
        /// Add a MainInfo object information to database.
        /// </summary>
        public void AddToDatabase()
        {
            // Add MainInfo.
            string? previousEpisodeHref = null;
            string? previousEpisodeName = null;
            string? nextEpisodeHref = null;
            string? nextEpisodeName = null;
            string? imageMedium = null;
            string? imageOriginal = null;
            double? average = null;
            long? tvrage = null;
            long? thetvdb = null;
            string? imdb = null;
            long? networkId = null;
            long? webChannelId = null;
            string? dvdCountryCode = null;
            string? href = null;


            if (_links != null)
            {
                if (_links.previousepisode != null)
                {
                    previousEpisodeHref = _links.previousepisode.href;
                    previousEpisodeName = _links.previousepisode.name;
                }
                if (_links.nextepisode != null)
                {
                    nextEpisodeHref = _links.nextepisode.href;
                    nextEpisodeName = _links.nextepisode.name;
                }
            }

            if (image != null)
            {
                imageMedium = image.medium;
                imageOriginal = image.original;
            }

            if (rating != null)
            {
                average = rating.average;
            }

            if (externals != null)
            {
                tvrage = externals.tvrage;
                thetvdb = externals.thetvdb;
                imdb = externals.imdb;
            }

            if (dvdCountry != null)
            {
                dvdCountryCode = dvdCountry.code;
            }

            if (webChannel != null)
            {
                webChannelId = webChannel.id;
            }
            if (network != null)
            {
                networkId = network.id;
            }

            if (_links != null)
            {
                if (_links.self != null)
                {
                    href = _links.self.href;
                }
            }

            CodereTvmaze.DAL.DatabaseConnection connection = CodereTvmaze.DAL.MainInfo.AddToDatabase(id, url, name,
                type, language, status, runtime, averageRuntime, premiered, ended, officialSite, weight, summary, updated,
                previousEpisodeHref, previousEpisodeName, nextEpisodeHref, nextEpisodeName,
                imageMedium, imageOriginal, average, tvrage, thetvdb, imdb, dvdCountryCode, networkId, webChannelId, href);

            if (genres != null)
            {
                if (genres.Count > 0)
                {
                    // Delete previous informed genres.
                    CodereTvmaze.DAL.Genre.DeleteMainInfoGenres(connection, id);

                    // Add genres.
                    int pos = 1;
                    foreach (var genre in genres)
                    {
                        CodereTvmaze.DAL.Genre.AddGenre(connection, id, genre, pos);
                        ++pos;
                    }
                }
            }

            if (dvdCountry != null)
            {
                //Add Country if it doesn't exist.
                dvdCountry.AddToDatabaseIfNotExists(connection);
            }

            if (webChannel != null)
            {
                // Add WebChannel.
                webChannel.AddToDatabaseIfNotExists(connection);
            }
            if (network != null)
            {
                // Add Network.
                network.AddToDatabaseIfNotExists(connection);
            }

            if (schedule != null)
            {
                // Delete previous informed schedule.
                CodereTvmaze.DAL.Schedule.DeleteMainInfoSchedule(connection, id);

                // Add Schedule.
                schedule.AddSchedule(connection, id);
            }

            // Close connection.
            connection.Commit();
            connection.Close();
        }

        /// <summary>
        /// Return a MainInfo object with its data from database based on an id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MainInfo GetById(int id)
        {
            MainInfo mainInfo = null;
            DataRow mainInfoRow = CodereTvmaze.DAL.MainInfo.GetById(id);

            if (mainInfoRow == null)
                return null;

            mainInfo = CreateMainInfoFromDataRow(mainInfoRow);


            return mainInfo;
        }

        /// <summary>
        /// Return a MainInfo object array with all data from database.
        /// </summary>
        /// <returns></returns>
        public static List<MainInfo> GetAll()
        {
            DataTable dtMainInfo = CodereTvmaze.DAL.MainInfo.GetAll();
            List<MainInfo> mainInfos = null;

            if (dtMainInfo == null)
                return null;

            if (dtMainInfo.Rows.Count > 0)
            {

                mainInfos = new List<MainInfo>();

                foreach (DataRow mainInfoRow in dtMainInfo.Rows)
                {
                    // Create a new MainInfo object and add it to list.
                    MainInfo mainInfo = CreateMainInfoFromDataRow(mainInfoRow);
                    mainInfos.Add(mainInfo);
                }
            }

            return mainInfos;
        }

        /// <summary>
        /// Return a MainInfo object with its data from database based on an id interval (represented by a page).
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<MainInfo> GetByPage(long page)
        {
            // Determinate first and last page element ids.
            long firstId = page * 250;
            long lastId = ((page + 1) * 250) - 1;
            List<MainInfo> mainInfos = null;

            DataTable mainInfoTable = CodereTvmaze.DAL.MainInfo.GetByInterval(firstId, lastId);

            if (mainInfoTable == null)
                return null;

            if (mainInfoTable.Rows.Count > 0)
            {

                using (DataSet relationalTables = new DataSet())
                {
                    DataTable genresTable = DAL.Genre.GetAll();
                    DataTable countriesTable = DAL.Country.GetAll();
                    DataTable networksTable = DAL.Network.GetAll();
                    DataTable webChannelsTable = DAL.WebChannel.GetAll();
                    DataTable schedulesTable = DAL.Schedule.GetAll();

                    genresTable.TableName = "genresTable";
                    countriesTable.TableName = "countriesTable";
                    networksTable.TableName = "networksTable";
                    webChannelsTable.TableName = "webChannelsTable";
                    schedulesTable.TableName = "schedulesTable";

                    relationalTables.Tables.Add(genresTable);
                    relationalTables.Tables.Add(countriesTable);
                    relationalTables.Tables.Add(networksTable);
                    relationalTables.Tables.Add(webChannelsTable);
                    relationalTables.Tables.Add(schedulesTable);

                    mainInfos = new List<MainInfo>();

                    foreach (DataRow mainInfoRow in mainInfoTable.Rows)
                    {
                        // Create a new MainInfo object and add it to list.
                        // We use a dataset with preloades relational tables.
                        MainInfo mainInfo = CreateMainInfoFromDataRow(mainInfoRow, relationalTables);
                        mainInfos.Add(mainInfo);
                    }
                }
            }

            return mainInfos;

        }


        /// <summary>
        /// This method create a MainInfo object from a datarow. It is a method created to be
        /// used for all methods that return MainInfo object or MainInfo object array so we can
        /// recycle code.
        /// </summary>
        /// <param name="mainInfoRow"></param>
        /// <returns></returns>
        private static MainInfo CreateMainInfoFromDataRow(DataRow mainInfoRow)
        {
            MainInfo mainInfo = new MainInfo()
            {
                id = (long)mainInfoRow["Id"],
                url = mainInfoRow["Url"] == DBNull.Value ? null : mainInfoRow["Url"].ToString(),
                name = mainInfoRow["Name"] == DBNull.Value ? null : mainInfoRow["Name"].ToString(),
                type = mainInfoRow["Type"] == DBNull.Value ? null : mainInfoRow["Type"].ToString(),
                language = mainInfoRow["Language"] == DBNull.Value ? null : mainInfoRow["Language"].ToString(),
                status = mainInfoRow["Status"] == DBNull.Value ? null : mainInfoRow["Status"].ToString(),
                runtime = mainInfoRow["Runtime"] == DBNull.Value ? null : (long)(mainInfoRow["Runtime"]),
                averageRuntime = mainInfoRow["AverageRuntime"] == DBNull.Value ? null : (long)(mainInfoRow["AverageRuntime"]),
                premiered = mainInfoRow["Premiered"] == DBNull.Value ? null : DateOnly.Parse(mainInfoRow["Premiered"].ToString()),
                ended = mainInfoRow["Ended"] == DBNull.Value ? null : DateOnly.Parse(mainInfoRow["Ended"].ToString()),
                officialSite = mainInfoRow["OfficialSite"] == DBNull.Value ? null : mainInfoRow["OfficialSite"].ToString(),
                weight = mainInfoRow["Weight"] == DBNull.Value ? null : (long)(mainInfoRow["Weight"]),
                summary = mainInfoRow["Summary"] == DBNull.Value ? null : mainInfoRow["Summary"].ToString(),
                updated = mainInfoRow["Updated"] == DBNull.Value ? null : (long)(mainInfoRow["Updated"]),

            };

            // Genres.

            DataTable dtGenres = CodereTvmaze.DAL.Genre.GetGenreByMainInfoId(mainInfo.id);
            if (dtGenres != null)
            {
                if (dtGenres.Rows.Count > 0)
                {
                    mainInfo.genres = new List<string>();
                    foreach (DataRow genresRow in dtGenres.Rows)
                    {
                        mainInfo.genres.Add(genresRow["Genre"].ToString());
                    }
                }
            }

            // Schedule.

            mainInfo.schedule = Schedule.GetScheduleByMainInfoId(mainInfo.id);

            // Rating.

            mainInfo.rating = new Rating()
            {
                average = mainInfoRow["Average"] == DBNull.Value ? null : Convert.ToDouble(mainInfoRow["Average"].ToString())
            };

            // Externals.

            mainInfo.externals = new Externals()
            {
                tvrage = mainInfoRow["Tvrage"] == DBNull.Value ? null : Convert.ToInt64(mainInfoRow["Tvrage"].ToString()),
                thetvdb = mainInfoRow["TheTvDb"] == DBNull.Value ? null : Convert.ToInt64(mainInfoRow["TheTvDb"].ToString()),
                imdb = mainInfoRow["Imdb"] == DBNull.Value ? null : mainInfoRow["Imdb"].ToString()

            };

            // Image.

            mainInfo.image = new Image()
            {
                medium = mainInfoRow["ImageMedium"] == DBNull.Value ? null : mainInfoRow["ImageMedium"].ToString(),
                original = mainInfoRow["ImageOriginal"] == DBNull.Value ? null : mainInfoRow["ImageOriginal"].ToString()
            };

            // Network.

            if (mainInfoRow["NetworkId"] != DBNull.Value)
            {
                mainInfo.network = Network.GetNetworkById(Convert.ToInt64(mainInfoRow["NetworkId"].ToString()));
            }
            // Web channel.

            if (mainInfoRow["WebChannelId"] != DBNull.Value)
            {
                mainInfo.webChannel = WebChannel.GetWebChannelkById(Convert.ToInt64(mainInfoRow["WebChannelId"].ToString()));
            }

            // Dvd country.

            if (mainInfoRow["DvdCountryCode"] != DBNull.Value)
            {
                mainInfo.dvdCountry = Country.GetCountryByCode(mainInfoRow["DvdCountryCode"].ToString());
            }

            if (mainInfoRow["PreviousEpisodeHref"] != DBNull.Value)
            {
                DataRow countryRow = CodereTvmaze.DAL.Country.GetCountryByCode(mainInfoRow["PreviousEpisodeHref"].ToString());
                if (countryRow != null)
                {
                    mainInfo.dvdCountry = new Country();
                }
            }

            // Previous and next episodies.

            if ((mainInfoRow["PreviousEpisodeHref"] != DBNull.Value) || (mainInfoRow["NextEpisodeHref"] != DBNull.Value) || (mainInfoRow["Href"] != DBNull.Value))
            {
                mainInfo._links = new Links();

                if (mainInfoRow["Href"] != DBNull.Value)
                {
                    mainInfo._links.self = new Self();
                    mainInfo._links.self.href = mainInfoRow["Href"].ToString();
                }

                if (mainInfoRow["PreviousEpisodeHref"] != DBNull.Value)
                {
                    mainInfo._links.previousepisode = new Episode()
                    {
                        href = mainInfoRow["PreviousEpisodeHref"].ToString(),
                        name = mainInfoRow["PreviousEpisodeName"] == DBNull.Value ? null : mainInfoRow["PreviousEpisodeName"].ToString()
                    };
                }

                if (mainInfoRow["NextEpisodeHref"] != DBNull.Value)
                {
                    mainInfo._links.nextepisode = new Episode()
                    {
                        href = mainInfoRow["NextEpisodeHref"].ToString(),
                        name = mainInfoRow["NextEpisodeName"] == DBNull.Value ? null : mainInfoRow["NextEpisodeName"].ToString()
                    };
                }
            }


            return mainInfo;
        }

        /// <summary>
        /// This method create a MainInfo object from a datarow. It is a method created to be
        /// used for all methods that return MainInfo object or MainInfo object array so we can
        /// recycle code.
        /// This polymorphic method is designed for the call in the creation of an array of MainInfo objects,
        /// avoiding hundreds of queries by means of the dataset passed as a parameter
        /// since we pre-load the relational tables.
        /// </summary>
        /// <param name="mainInfoRow"></param>
        /// <returns></returns>
        private static MainInfo CreateMainInfoFromDataRow(DataRow mainInfoRow, DataSet relationalTables)
        {
            MainInfo mainInfo = new MainInfo()
            {
                id = (long)mainInfoRow["Id"],
                url = mainInfoRow["Url"] == DBNull.Value ? null : mainInfoRow["Url"].ToString(),
                name = mainInfoRow["Name"] == DBNull.Value ? null : mainInfoRow["Name"].ToString(),
                type = mainInfoRow["Type"] == DBNull.Value ? null : mainInfoRow["Type"].ToString(),
                language = mainInfoRow["Language"] == DBNull.Value ? null : mainInfoRow["Language"].ToString(),
                status = mainInfoRow["Status"] == DBNull.Value ? null : mainInfoRow["Status"].ToString(),
                runtime = mainInfoRow["Runtime"] == DBNull.Value ? null : (long)(mainInfoRow["Runtime"]),
                averageRuntime = mainInfoRow["AverageRuntime"] == DBNull.Value ? null : (long)(mainInfoRow["AverageRuntime"]),
                premiered = mainInfoRow["Premiered"] == DBNull.Value ? null : DateOnly.Parse(mainInfoRow["Premiered"].ToString()),
                ended = mainInfoRow["Ended"] == DBNull.Value ? null : DateOnly.Parse(mainInfoRow["Ended"].ToString()),
                officialSite = mainInfoRow["OfficialSite"] == DBNull.Value ? null : mainInfoRow["OfficialSite"].ToString(),
                weight = mainInfoRow["Weight"] == DBNull.Value ? null : (long)(mainInfoRow["Weight"]),
                summary = mainInfoRow["Summary"] == DBNull.Value ? null : mainInfoRow["Summary"].ToString(),
                updated = mainInfoRow["Updated"] == DBNull.Value ? null : (long)(mainInfoRow["Updated"]),

            };

            // Genres.

            DataRow[] genresRows = relationalTables.Tables["genresTable"].Select("MainInfoId = " + mainInfo.id.ToString());
            if (genresRows != null)
            {
                if (genresRows.Length > 0)
                {
                    mainInfo.genres = new List<string>();
                    foreach (DataRow genresRow in genresRows)
                    {
                        mainInfo.genres.Add(genresRow["Genre"].ToString());
                    }
                }
            }

            // Schedule.

            DataRow[] schedulesRows = relationalTables.Tables["schedulesTable"].Select("MainInfoId = " + mainInfo.id.ToString());

            Schedule schedule = null;

            if (schedulesRows != null)
            {
                if (schedulesRows.Length > 0)
                {
                    schedule = new Schedule()
                    {
                        time = schedulesRows[0]["Time"] == DBNull.Value ? null : schedulesRows[0]["Time"].ToString()
                    };

                    schedule.days = new List<string>();
                    foreach (DataRow scheduleRow in schedulesRows)
                    {
                        schedule.days.Add(scheduleRow["Day"] == DBNull.Value ? null : scheduleRow["Day"].ToString());
                    }
                }
            }

            mainInfo.schedule = schedule;

            // Rating.

            mainInfo.rating = new Rating()
            {
                average = mainInfoRow["Average"] == DBNull.Value ? null : Convert.ToDouble(mainInfoRow["Average"].ToString())
            };

            // Externals.

            mainInfo.externals = new Externals()
            {
                tvrage = mainInfoRow["Tvrage"] == DBNull.Value ? null : Convert.ToInt64(mainInfoRow["Tvrage"].ToString()),
                thetvdb = mainInfoRow["TheTvDb"] == DBNull.Value ? null : Convert.ToInt64(mainInfoRow["TheTvDb"].ToString()),
                imdb = mainInfoRow["Imdb"] == DBNull.Value ? null : mainInfoRow["Imdb"].ToString()

            };

            // Image.

            mainInfo.image = new Image()
            {
                medium = mainInfoRow["ImageMedium"] == DBNull.Value ? null : mainInfoRow["ImageMedium"].ToString(),
                original = mainInfoRow["ImageOriginal"] == DBNull.Value ? null : mainInfoRow["ImageOriginal"].ToString()
            };

            // Network.

            if (mainInfoRow["NetworkId"] != DBNull.Value)
            {
                DataRow[] networksRows = relationalTables.Tables["networksTable"].Select("Id = " + mainInfoRow["NetworkId"].ToString());

                Network network = null;

                if (networksRows != null)
                {
                    if (networksRows.Length > 0)
                    {
                        network = new Network()
                        {
                            id = Convert.ToInt64(networksRows[0]["Id"].ToString()),
                            name = networksRows[0]["Name"] == DBNull.Value ? null : networksRows[0]["Name"].ToString(),
                            officialSite = networksRows[0]["OfficialSite"] == DBNull.Value ? null : networksRows[0]["OfficialSite"].ToString(),
                        };

                        Country networkCountry = null;

                        if (networksRows[0]["CountryCode"] != DBNull.Value)
                        {
                            DataRow[] networkCountriesRows = relationalTables.Tables["CountriesTable"].Select("Code = '" + networksRows[0]["CountryCode"].ToString().ToString() + "'");

                            if (networkCountriesRows != null)
                            {
                                if (networkCountriesRows.Length > 0)
                                {
                                    networkCountry = new Country()
                                    {
                                        code = networkCountriesRows[0]["Code"] == null ? null : networkCountriesRows[0]["Code"].ToString(),
                                        name = networkCountriesRows[0]["Name"] == null ? null : networkCountriesRows[0]["Name"].ToString(),
                                        timezone = networkCountriesRows[0]["Timezone"] == null ? null : networkCountriesRows[0]["Timezone"].ToString()
                                    };
                                }
                            }

                            network.country = networkCountry;
                        }
                    }

                    mainInfo.network = network;
                }
            }
            // Web channel.

            if (mainInfoRow["WebChannelId"] != DBNull.Value)
            {
                DataRow[] webChannelsRows = relationalTables.Tables["webChannelsTable"].Select("Id = " + mainInfoRow["WebChannelId"].ToString());

                WebChannel webChannel = null;

                if (webChannelsRows != null)
                {
                    if (webChannelsRows.Length > 0)
                    {
                        webChannel = new WebChannel()
                        {
                            id = Convert.ToInt64(webChannelsRows[0]["Id"].ToString()),
                            name = webChannelsRows[0]["Name"] == DBNull.Value ? null : webChannelsRows[0]["Name"].ToString(),
                            officialSite = webChannelsRows[0]["OfficialSite"] == DBNull.Value ? null : webChannelsRows[0]["OfficialSite"].ToString(),
                        };

                        Country webChannelCountry = null;

                        if (webChannelsRows[0]["CountryCode"] != DBNull.Value)
                        {
                            DataRow[] webChannelCountriesRows = relationalTables.Tables["CountriesTable"].Select("Code = '" + webChannelsRows[0]["CountryCode"].ToString().ToString() + "'");

                            if (webChannelCountriesRows != null)
                            {
                                if (webChannelCountriesRows.Length > 0)
                                {
                                    webChannelCountry = new Country()
                                    {
                                        code = webChannelCountriesRows[0]["Code"] == null ? null : webChannelCountriesRows[0]["Code"].ToString(),
                                        name = webChannelCountriesRows[0]["Name"] == null ? null : webChannelCountriesRows[0]["Name"].ToString(),
                                        timezone = webChannelCountriesRows[0]["Timezone"] == null ? null : webChannelCountriesRows[0]["Timezone"].ToString()
                                    };
                                }
                            }

                            webChannel.country = webChannelCountry;
                        }
                    }
                }

                mainInfo.webChannel = webChannel;
            }

            // Dvd country.

            Country dvdCountry = null;

            if (mainInfoRow["DvdCountryCode"] != DBNull.Value)
            {
                DataRow[] dvdCountryRows = relationalTables.Tables["CountriesTable"].Select("Code = '" + mainInfoRow["DvdCountryCode"].ToString().ToString() + "'");

                if (dvdCountryRows != null)
                {
                    if (dvdCountryRows.Length > 0)
                    {
                        dvdCountry = new Country()
                        {
                            code = dvdCountryRows[0]["Code"] == null ? null : dvdCountryRows[0]["Code"].ToString(),
                            name = dvdCountryRows[0]["Name"] == null ? null : dvdCountryRows[0]["Name"].ToString(),
                            timezone = dvdCountryRows[0]["Timezone"] == null ? null : dvdCountryRows[0]["Timezone"].ToString()
                        };
                    }
                }
            }

            mainInfo.dvdCountry = dvdCountry;


            // Previous and next episodies.

            if ((mainInfoRow["PreviousEpisodeHref"] != DBNull.Value) || (mainInfoRow["NextEpisodeHref"] != DBNull.Value) || (mainInfoRow["Href"] != DBNull.Value))
            {
                mainInfo._links = new Links();

                if (mainInfoRow["Href"] != DBNull.Value)
                {
                    mainInfo._links.self = new Self();
                    mainInfo._links.self.href = mainInfoRow["Href"].ToString();
                }

                if (mainInfoRow["PreviousEpisodeHref"] != DBNull.Value)
                {
                    mainInfo._links.previousepisode = new Episode()
                    {
                        href = mainInfoRow["PreviousEpisodeHref"].ToString(),
                        name = mainInfoRow["PreviousEpisodeName"] == DBNull.Value ? null : mainInfoRow["PreviousEpisodeName"].ToString()
                    };
                }

                if (mainInfoRow["NextEpisodeHref"] != DBNull.Value)
                {
                    mainInfo._links.nextepisode = new Episode()
                    {
                        href = mainInfoRow["NextEpisodeHref"].ToString(),
                        name = mainInfoRow["NextEpisodeName"] == DBNull.Value ? null : mainInfoRow["NextEpisodeName"].ToString()
                    };
                }
            }

            return mainInfo;
        }
    }
}
