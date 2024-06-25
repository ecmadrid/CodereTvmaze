using System;
using System.Collections.Generic;
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
        public int id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string language { get; set; }
        public List<string> genres { get; set; }
        public string status { get; set; }
        public int? runtime { get; set; }
        public int averageRuntime { get; set; }
        public string premiered { get; set; }
        public string ended { get; set; }
        public string officialSite { get; set; }
        public Schedule schedule { get; set; }
        public Rating rating { get; set; }
        public int weight { get; set; }
        public Network network { get; set; }
        public WebChannel webChannel { get; set; }
        public Country dvdCountry { get; set; }
        public Externals externals { get; set; }
        public Image image { get; set; }
        public string summary { get; set; }
        public int updated { get; set; }
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

            CodereTvmaze.DAL.Connection connection = CodereTvmaze.DAL.MainInfo.AddToDatabase(id, url, name,
                type, language, status, runtime, averageRuntime, premiered, ended, officialSite, weight, summary, updated,
                previousEpisodeHref, previousEpisodeName, nextEpisodeHref, nextEpisodeName,
                imageMedium, imageOriginal, average, tvrage, thetvdb, imdb, dvdCountryCode, networkId, webChannelId);

            if (genres != null)
            {
                if (genres.Count > 0)
                {
                    // Delete previous informed genres.
                    CodereTvmaze.DAL.Genre.DeleteMainInfoGenres(connection, id);

                    // Add genres.
                    int pos = 1;
                    foreach(var genre in genres)
                    {
                        CodereTvmaze.DAL.Genre.AddGenre(connection, id, genre, pos);
                        ++pos;
                    }
                }
            }

            if(dvdCountry != null)
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

    }

    }
