using System;
using System.Data;
using Mono.Data.Sqlite;

namespace SittingDucks
{
    public class SqliteManager
    {
        public (SqliteConnection, bool) OpenConnection(SqliteConnection connection)
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

        public SqliteConnection CloseConnection(bool shouldClose, SqliteConnection connection)
        {

            if (shouldClose)
            {
                connection.Close();
            }

            return connection;
        }
    }
}
