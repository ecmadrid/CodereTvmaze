using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    public class MainInfo
    {
        public static Connection AddToDatabase(long id, string url, string name, string type, string language,
            string status, long? runtime, long? averageRuntime, string premiered, string ended,
            string officialSite, long? weight, string summary, long? updated,
            string previousEpisodeHref, string previousEpisodeName,
                string nextEpisodeHref, string nextEpisodeName, string imageMedium, string imageOriginal,
                double? average, int? tvrage, int? thetvdb, string imdb,
                string dvdCountryCode, int? networkId, int? webChannelId, string href)
        {
            Connection connection = new DAL.Connection();
            connection.Open();
            connection.Begin();

            // First, we delete previous register if it exists.
            string sql = "DELETE FROM MainInfo WHERE Id = " + id.ToString();
            connection.ExecuteNonQuery(sql);

            sql = @"INSERT INTO MainInfo (Id, Url, Name, Type, Language, Status, Runtime, AverageRuntime, Premiered, Ended,
                OfficialSite, Weight, Summary, Updated, PreviousEpisodeHref, PreviousEpisodeName,
                NextEpisodeHref, NextEpisodeName, ImageMedium, ImageOriginal, Average,
                TvRage, TheTvDb, Imdb, DvdCountryCode, NetworkId, WebChannelId, Href)
                VALUES (@Id, @Url, @Name, @Type, @Language, @Status, @Runtime, @AverageRuntime, @Premiered, @Ended,
                @OfficialSite, @Weight, @Summary, @Updated, @PreviousEpisodeHref, @PreviousEpisodeName,
                @NextEpisodeHref, @NextEpisodeName, @ImageMedium, @ImageOriginal, @Average,
                @TvRage, @TheTvDb, @Imdb, @DvdCountryCode, @NetworkId, @WebChannelId, @Href)";
            sql = sql.Replace("@Id", id.ToString());
            sql = sql.Replace("@Url", url == null ? "NULL" : "'" + url + "'");
            sql = sql.Replace("@Name", name == null ? "NULL" : "'" + name.Replace("'", "''") + "'");
            sql = sql.Replace("@Type", type == null ? "NULL" : "'" + type + "'");
            sql = sql.Replace("@Language", language == null ? "NULL" : "'" + language + "'");
            sql = sql.Replace("@Status", status == null ? "NULL" : "'" + status + "'");
            sql = sql.Replace("@Runtime", runtime == null ? "NULL" : runtime.ToString());
            sql = sql.Replace("@AverageRuntime", averageRuntime == null ? "NULL" : averageRuntime.ToString());
            sql = sql.Replace("@Premiered", premiered == null ? "NULL" : "'" + premiered + "'");
            sql = sql.Replace("@Ended", ended == null ? "NULL" : "'" + ended + "'");
            sql = sql.Replace("@OfficialSite", officialSite == null ? "NULL" : "'" + officialSite + "'");
            sql = sql.Replace("@Weight", weight == null ? "NULL" : weight.ToString());
            sql = sql.Replace("@Summary", summary == null ? "NULL" : "'" + summary.Replace("'", "''") + "'");
            sql = sql.Replace("@Updated", updated == null ? "NULL" : updated.ToString());
            sql = sql.Replace("@PreviousEpisodeHref", previousEpisodeHref == null ? "NULL" : "'" + previousEpisodeHref + "'");
            sql = sql.Replace("@PreviousEpisodeName", previousEpisodeName == null ? "NULL" : "'" + previousEpisodeName.Replace("'", "''") + "'");
            sql = sql.Replace("@NextEpisodeHref", nextEpisodeHref == null ? "NULL" : "'" + nextEpisodeHref + "'");
            sql = sql.Replace("@NextEpisodeName", nextEpisodeName == null ? "NULL" : "'" + nextEpisodeName.Replace("'", "''") + "'");
            sql = sql.Replace("@ImageMedium", imageMedium == null ? "NULL" : "'" + imageMedium + "'");
            sql = sql.Replace("@ImageOriginal", imageOriginal == null ? "NULL" : "'" + imageOriginal + "'");
            sql = sql.Replace("@Average", average == null ? "NULL" : average.ToString().Replace(",", "."));
            sql = sql.Replace("@TvRage", tvrage == null ? "NULL" : tvrage.ToString());
            sql = sql.Replace("@TheTvDb", thetvdb == null ? "NULL" : thetvdb.ToString());
            sql = sql.Replace("@Imdb", imdb == null ? "NULL" : "'" + imdb + "'");
            sql = sql.Replace("@DvdCountryCode", dvdCountryCode == null ? "NULL" : "'" + dvdCountryCode + "'");
            sql = sql.Replace("@NetworkId", networkId == null ? "NULL" : networkId.ToString());
            sql = sql.Replace("@WebChannelId", webChannelId == null ? "NULL" : webChannelId.ToString());
            sql = sql.Replace("@Href", href == null ? "NULL" : "'" + href + "'");


            connection.ExecuteNonQuery(sql);

            return connection;
        }

        public static DataTable GetById(int id)
        {
            Connection connection = new DAL.Connection();
            connection.Open();

            string sql = "SELECT * FROM MainInfo WHERE Id = " + id.ToString();

            DataTable dt = connection.Execute(sql);

            connection.Close();

            return dt;
        }
    }
}