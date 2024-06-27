using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    /// <summary>
    /// Class <c>Genre</c> Manages Genres table in database.
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// This method adds a new record associated to a MainInfo id.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mainInfoId"></param>
        /// <param name="genre"></param>
        /// <param name="pos"></param>
        public static void AddGenre(DatabaseConnection connection, long mainInfoId, string genre, int pos)
        {
            bool needCloseConnection = false;
            if (connection == null)
            {
                connection = new DAL.DatabaseConnection();
                connection.Open();
                connection.Begin();
                needCloseConnection = true;
            }

            string sql = @"INSERT INTO Genres (MainInfoId, Genre, Pos) VALUES( @MainInfoId, @Genre, @Pos)";
            sql = sql.Replace("@MainInfoId", mainInfoId.ToString());
            sql = sql.Replace("@Genre", genre == null ? "NULL" : "'" + genre + "'");
            sql = sql.Replace("@Pos", pos.ToString());
            connection.ExecuteNonQuery(sql);

            if (needCloseConnection)
            {
                connection.Commit();
                connection.Close();            }
        }

        /// <summary>
        /// Deletes records relationed with a MainInfo id.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mainInfoId"></param>
        public static void DeleteMainInfoGenres(DatabaseConnection connection, long mainInfoId)
        {
            string sql = @"DELETE FROM Genres Where MainInfoId = @MainInfoId";
            sql = sql.Replace("@MainInfoId", mainInfoId.ToString());
            connection.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// This method returns a datatable with all genres associated to a MainInfo id. Null if not registers found.
        /// </summary>
        /// <param name="mainInfoId"></param>
        /// <returns></returns>
        public static DataTable GetGenreByMainInfoId(long mainInfoId)
        {

            DatabaseConnection connection = connection = new DAL.DatabaseConnection();
            connection.Open();
            string sql = @"SELECT * FROM Genres WHERE MainInfoId =" + mainInfoId.ToString() + " ORDER BY Pos";
            DataTable dt = connection.Execute(sql);

            connection.Close();

            return dt;
        }

        /// <summary>
        /// Retrieves all records from Genres table in a datatable object. Null if table is empty.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAll()
        {

            DatabaseConnection connection = connection = new DAL.DatabaseConnection();
            connection.Open();
            string sql = @"SELECT * FROM Genres ORDER BY MainInfoId, Pos";
            DataTable dt = connection.Execute(sql);

            connection.Close();

            if (dt.Rows.Count < 1)
            {
                return null;
            };

            return dt;
        }
    }

}
