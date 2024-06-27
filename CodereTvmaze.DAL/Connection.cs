using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;


namespace CodereTvmaze.DAL
{
 /// <summary>
 /// Class <c>DatabaseConnection</c> This class open and close connections to database and manages transactions and querys.
 /// </summary>
    public class DatabaseConnection : IDisposable
    {
        /// <summary>
        /// Connection string value.
        /// </summary>
        public string? ConnectionString { get; set; }

        private SqliteConnection? Connection;
        private SqliteTransaction? Transaction;
        /// <summary>
        /// This method opens a database connection.
        /// </summary>
        public void Open()
        {
            ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["ProjectConnection"];

            Connection = new SqliteConnection(ConnectionString);
            Connection.Open();
        }

        /// <summary>
        /// This method closes the database connection.
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Begin transaction.
        /// </summary>
        public void Begin()
        {
            if (Transaction != null)
            {
                Transaction = Connection.BeginTransaction();
            }
        }

        /// <summary>
        /// Commit transaction.
        /// </summary>
        public void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
            }
        }

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
        }

        /// <summary>
        /// This method execute a sql statement with no return result.
        /// </summary>
        /// <param name="sql"></param>
        public void ExecuteNonQuery(string sql)
        {
            SqliteCommand command = new SqliteCommand(sql, Connection, Transaction);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// This method execute a query returning results in a datatable. If not results then return null object.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable Execute(string sql)
        {
            SqliteCommand command = new SqliteCommand(sql, Connection, Transaction);
            SqliteDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        /// <summary>
        ///         This method execute a query returning results in a long value. It's a simplified executeScalar call thinking in count querys.
        ///         This method is prepared only for long tipe returns.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public long? ExecuteLongScalar(string sql)
        {
            SqliteCommand command = new SqliteCommand(sql, Connection, Transaction);
            var obj = command.ExecuteScalar();
            if (obj == null)
            {
                return null;
            }

            return Convert.ToInt64(obj.ToString());
        }

        /// <summary>
        /// Disposes object.
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }

}
