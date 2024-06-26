using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;


namespace CodereTvmaze.DAL
{
    public class Connection : IDisposable
    {
        public string connectionString { get; set; }

        private SqliteConnection conn;
        private SqliteTransaction transaction;
        public void Open()
        {
            connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["ProjectConnection"];

            conn = new SqliteConnection(connectionString);
            conn.Open();
        }

        public void Close()
        {
            conn.Close();
        }

        public void Begin()
        {
            transaction = conn.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void ExecuteNonQuery(string sql)
        {
            SqliteCommand command = new SqliteCommand(sql, conn, transaction);
            command.ExecuteNonQuery();
        }

        public DataTable Execute(string sql)
        {
            SqliteCommand command = new SqliteCommand(sql, conn, transaction);
            SqliteDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return dt;
        }

        public long ExecuteScalar(string sql)
        {
            SqliteCommand command = new SqliteCommand(sql, conn, transaction);
            return (long)command.ExecuteScalar();
        }

        public void Dispose()
        {
            Close();
        }
    }

}
