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
        public DateOnly? premiered { get; set; }
        public DateOnly? ended { get; set; }
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
            long? tvrage = null;
            long? thetvdb = null;
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
                if (_links.self != null)
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
            DataRow mainInfoRow = CodereTvmaze.DAL.MainInfo.GetById(id);

            if (mainInfoRow == null)
                return null;

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

            DataTable dtSchedule = CodereTvmaze.DAL.Schedule.GetScheduleByMainInfoId(mainInfo.id);
            if (dtSchedule != null)
            {
                if (dtSchedule.Rows.Count > 0)
                {
                    mainInfo.schedule = new Schedule();
                    mainInfo.schedule.days = new List<string>();
                    mainInfo.schedule.time = dtSchedule.Rows[0]["Time"].ToString();
                    foreach (DataRow scheduleRow in dtSchedule.Rows)
                    {
                        mainInfo.schedule.days.Add(scheduleRow["Day"].ToString());
                    }
                }
            }

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
