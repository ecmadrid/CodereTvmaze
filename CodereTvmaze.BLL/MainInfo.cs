using CodereTvmaze.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CodereTvmaze.BLL
{
    public class MainInfo
    {
        public long id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string language { get; set; }
        public List<string> genres { get; set; }
        public string status { get; set; }
        public long? runtime { get; set; }
        public long? averageRuntime { get; set; }
        public string premiered { get; set; }
        public string ended { get; set; }
        public string officialSite { get; set; }
        public Schedule schedule { get; set; }
        public Rating rating { get; set; }
        public long? weight { get; set; }
        public Network network { get; set; }
        public WebChannel webChannel { get; set; }
        public Country dvdCountry { get; set; }
        public Externals externals { get; set; }
        public Image image { get; set; }
        public string summary { get; set; }
        public long? updated { get; set; }
        public Links _links { get; set; }

        // Methods

        public static MainInfo GetMainInfo(int id)
        {

            using (var http = new HttpClient())
            {
                var endpoint = "https://api.tvmaze.com/shows/" + id.ToString();
                var result = http.GetAsync(endpoint).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    var obj = JsonSerializer.Deserialize<MainInfo>(json);

                    obj.AddToDatabase();

                    return obj;
                }

                return null;
            };
        }

        public static List<MainInfo> GetMainInfoAll()
        {
            using (var http = new HttpClient())
            {
                var endpoint = "https://api.tvmaze.com/shows";
                var result = http.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var objs = JsonSerializer.Deserialize<List<MainInfo>>(json);

                foreach (var obj in objs)
                {
                    obj.AddToDatabase();
                }

                return objs;
            };
        }

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
            int? tvrage = null;
            int? thetvdb = null;
            string? imdb = null;
            int? networkId = null;
            int? webChannelId = null;
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
                if(_links.self != null)
                {
                    href = _links.self.href;
                }
            }

            CodereTvmaze.DAL.Connection connection = CodereTvmaze.DAL.MainInfo.AddToDatabase(id, url, name,
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

        public static MainInfo GetById(int id)
        {
            DataTable dt = CodereTvmaze.DAL.MainInfo.GetById(id);

            if (dt == null)
                return null;
            if (dt.Rows.Count < 1)
                return null;

            DataRow dr = dt.Rows[0];

            MainInfo mainInfo = new MainInfo()
            {
                id = (long)dr["Id"],
                url = dr["Url"] == DBNull.Value ? null : dr["Url"].ToString(),
                name = dr["Name"] == DBNull.Value ? null : dr["Name"].ToString(),
                type = dr["Type"] == DBNull.Value ? null : dr["Type"].ToString(),
                language = dr["Language"] == DBNull.Value ? null : dr["Language"].ToString(),
                status = dr["Status"] == DBNull.Value ? null : dr["Status"].ToString(),
                runtime = dr["Runtime"] == DBNull.Value ? null : (long)(dr["Runtime"]),
                averageRuntime = dr["AverageRuntime"] == DBNull.Value ? null : (long)(dr["AverageRuntime"]),
                premiered = dr["Premiered"] == DBNull.Value ? null : dr["Premiered"].ToString(),
                ended = dr["Ended"] == DBNull.Value ? null : dr["Ended"].ToString(),
                officialSite = dr["OfficialSite"] == DBNull.Value ? null : dr["OfficialSite"].ToString(),
                weight = dr["Weight"] == DBNull.Value ? null : (long)(dr["Weight"]),
                summary = dr["Summary"] == DBNull.Value ? null : dr["Summary"].ToString(),
                updated = dr["Updated"] == DBNull.Value ? null : (long)(dr["Updated"]),

            };

            // Genres.

            DataTable dtGenres = CodereTvmaze.DAL.Genre.GetGenreByMainInfoId(mainInfo.id);
            if (dtGenres != null)
            {
                if (dtGenres.Rows.Count > 0)
                {
                    mainInfo.genres = new List<string>();
                    foreach (DataRow r in dtGenres.Rows)
                    {
                        mainInfo.genres.Add(r["Genre"].ToString());
                    }
                }
            }

            // Previous and next episodies.

            if ((dr["PreviousEpisodeHref"] != DBNull.Value) || (dr["NextEpisodeHref"] != DBNull.Value) || (dr["Href"] != DBNull.Value))
            {
                mainInfo._links = new Links();

                if (dr["Href"] != DBNull.Value)
                {
                    mainInfo._links.self = new Self();
                    mainInfo._links.self.href = dr["Href"].ToString();
                }

                if (dr["PreviousEpisodeHref"] != DBNull.Value)
                {
                    mainInfo._links.previousepisode = new Episode()
                    {
                        href = dr["PreviousEpisodeHref"].ToString(),
                        name = dr["PreviousEpisodeName"] == DBNull.Value ? null : dr["PreviousEpisodeName"].ToString()
                    };
                }

                if (dr["NextEpisodeHref"] != DBNull.Value)
                {
                    mainInfo._links.nextepisode = new Episode()
                    {
                        href = dr["NextEpisodeHref"].ToString(),
                        name = dr["NextEpisodeName"] == DBNull.Value ? null : dr["NextEpisodeName"].ToString()
                    };
                }
            }


            return mainInfo;
        }

    }

}
