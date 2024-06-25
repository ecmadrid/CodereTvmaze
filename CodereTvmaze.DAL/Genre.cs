using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.DAL
{
    public class Genre
    {
        public static void AddGenre(Connection connection, int mainInfoId, string genre, int pos)
        {
            bool needCloseConnection = false;
            if (connection == null)
            {
                connection = new DAL.Connection();
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

        public static void DeleteMainInfoGenres(Connection connection, int mainInfoId)
        {
            string sql = @"DELETE FROM Genres Where MainInfoId = @MainInfoId";
            sql = sql.Replace("@MainInfoId", mainInfoId.ToString());
            connection.ExecuteNonQuery(sql);
        }
    }
}
