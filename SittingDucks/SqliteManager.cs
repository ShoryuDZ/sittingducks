using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

namespace SittingDucks
{
    public static class SqliteManager
    {
        public static (SqliteConnection, bool) OpenConnection(SqliteConnection connection)
        {
            // Clear last connection to prevent circular call to update
            SqliteConnection localConnection = null;

            bool shouldClose = false;

            if (connection.State != ConnectionState.Open)
            {
                shouldClose = true;
                connection.Open();
            }

            return (localConnection, shouldClose);
        }

        public static SqliteConnection CloseConnection(bool shouldClose, SqliteConnection connection)
        {

            if (shouldClose)
            {
                connection.Close();
            }

            return connection;
        }

        public static SqliteConnection GetDatabaseConnection()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folder = Path.Combine(appData, "SittingDucks");
            string database = Path.Combine(folder, "database.db3");

            // Create the database if it doesn't already exist
            bool exists = File.Exists(database);
            if (!exists)
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(folder);
                SqliteConnection.CreateFile(database);
            }

            // Create connection to the database
            var conn = new SqliteConnection("Data Source=" + database);

            // Set the structure of the database
            if (!exists)
            {
                var commands = new[] { "CREATE TABLE Data (ID TEXT, Website TEXT, Account TEXT, Password TEXT)", "CREATE TABLE System (ID TEXT, Password TEXT, INIT BOOLEAN)", "INSERT INTO [System] (ID, Password, INIT) VALUES ('', '', false)" };
                conn.Open();
                foreach (var cmd in commands)
                {
                    using (var c = conn.CreateCommand())
                    {
                        c.CommandText = cmd;
                        c.CommandType = CommandType.Text;
                        c.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }

            return conn;
        }
    }
}
