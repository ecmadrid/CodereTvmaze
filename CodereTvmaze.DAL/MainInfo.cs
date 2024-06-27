using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    /// <summary>
    /// Class <c>MainInfo</c> Main class containing main information.
    /// </summary>
    public class MainInfo
    {
     /// <summary>
     /// Method to add a new main information register to database. If there is a register with
     /// same main info id yet it's deleted. Returns a DatabaseConnection object to be used by
     /// other objects to include them in same transaction.
     /// </summary>
     /// <param name="id"></param>
     /// <param name="url"></param>
     /// <param name="name"></param>
     /// <param name="type"></param>
     /// <param name="language"></param>
     /// <param name="status"></param>
     /// <param name="runtime"></param>
     /// <param name="averageRuntime"></param>
     /// <param name="premiered"></param>
     /// <param name="ended"></param>
     /// <param name="officialSite"></param>
     /// <param name="weight"></param>
     /// <param name="summary"></param>
     /// <param name="updated"></param>
     /// <param name="previousEpisodeHref"></param>
     /// <param name="previousEpisodeName"></param>
     /// <param name="nextEpisodeHref"></param>
     /// <param name="nextEpisodeName"></param>
     /// <param name="imageMedium"></param>
     /// <param name="imageOriginal"></param>
     /// <param name="average"></param>
     /// <param name="tvrage"></param>
     /// <param name="thetvdb"></param>
     /// <param name="imdb"></param>
     /// <param name="dvdCountryCode"></param>
     /// <param name="networkId"></param>
     /// <param name="webChannelId"></param>
     /// <param name="href"></param>
     /// <returns></returns>
        public static DatabaseConnection AddToDatabase(long id, string? url, string? name, string? type, string? language,
            string? status, long? runtime, long? averageRuntime, DateOnly? premiered, DateOnly? ended,
            string? officialSite, long? weight, string? summary, long? updated,
            string? previousEpisodeHref, string? previousEpisodeName,
                string? nextEpisodeHref, string? nextEpisodeName, string? imageMedium, string? imageOriginal,
                double? average, long? tvrage, long? thetvdb, string? imdb,
                string? dvdCountryCode, long? networkId, long? webChannelId, string? href)
        {
            DatabaseConnection connection = new DAL.DatabaseConnection();
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

        /// <summary>
        /// Return a datarow with record from ;ainInfo table based on a id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataRow GetById(int id)
        {
            DatabaseConnection connection = new DAL.DatabaseConnection();
            connection.Open();

            string sql = "SELECT * FROM MainInfo WHERE Id = " + id.ToString();

            DataTable dt = connection.Execute(sql);

            connection.Close();

            if (dt == null)
            {
                return null;
            }

            if (dt.Rows.Count < 1)
            {
                return null;
            }

            return dt.Rows[0];
        }

        /// <summary>
        /// This method returns all records from MainInfo table into a datatable object. If there isn't
        /// any record it returns a null object.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAll()
        {
            DatabaseConnection connection = new DAL.DatabaseConnection();
            connection.Open();

            string sql = "SELECT * FROM MainInfo ORDER BY Id";

            DataTable dt = connection.Execute(sql);

            connection.Close();

            return dt;
        }

        /// <summary>
        /// returns records from MainInfo table into a datatable object whose ids are between aninterval. If there isn't
        /// records in criteria it returns a null object.
        /// </summary>
        /// <param name="firstId"></param>
        /// <param name="lastId"></param>
        /// <returns></returns>
        public static DataTable GetByInterval(long firstId, long lastId)
        {
            DatabaseConnection connection = new DAL.DatabaseConnection();
            connection.Open();

            string sql = "SELECT * FROM MainInfo WHERE Id BETWEEN @FirstId AND @LastId ORDER BY Id";
            sql = sql.Replace("@FirstId", firstId.ToString());
            sql = sql.Replace("@LastId", lastId.ToString());

            DataTable dt = connection.Execute(sql);

            connection.Close();

            return dt;
        }

        /// <summary>
        /// This method returns id field from last inserted MainInfo table.
        /// </summary>
        /// <returns></returns>
        public static long GetLastId()
        {
            DatabaseConnection connection = new DAL.DatabaseConnection();
            connection.Open();

            string sql = "SELECT ifnull(MAX(Id), 0) AS Id FROM MainInfo ORDER BY Id";

            long? maxId = connection.ExecuteLongScalar(sql);

            if (maxId == null)
            {
                maxId = 0;
            }

            connection.Close();

            return maxId.Value;
        }
    }
}