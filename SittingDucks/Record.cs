using System;
using System.Data;
using Mono.Data.Sqlite;

namespace SittingDucks
{
    public class Record
    {
        public Record(string website, string accountName, string password, SqliteConnection connection, Guid id = default(Guid), bool isSaved = true)
        {
            this.Website = website;
            this.AccountName = accountName;
            this.Password = password;

            if(id == default(Guid))
            {
                this.ID = Guid.NewGuid();
            }
            else
            {
                this.ID = id;
            }

            if (isSaved)
            {
                SaveRecord(connection);
            }
        }

        public Record() { }

        private void SaveRecord(SqliteConnection connection)
        {
            // Clear last connection to prevent circular call to update
            _conn = null;

            bool shouldClose = false;

            if (connection.State != ConnectionState.Open)
            {
                shouldClose = true;
                connection.Open();
            }

            // Execute query
            using (var command = connection.CreateCommand())
            {
                // Create new command
                command.CommandText = "INSERT INTO [Data] (ID, Website, Account, Password) VALUES (@COL1, @COL2, @COL3, @COL4)";

                // Populate with data from the record
                command.Parameters.AddWithValue("@COL1", ID.ToString());
                command.Parameters.AddWithValue("@COL2", Website);
                command.Parameters.AddWithValue("@COL3", AccountName);
                command.Parameters.AddWithValue("@COL4", Password);

                // Write to database
                command.ExecuteNonQuery();
            }

            if (shouldClose)
            {
                connection.Close();
            }

            //Save connection
            _conn = connection;
        }

        public string Website { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public Guid ID { get; set; }

        private SqliteConnection _conn = null;
    }
}
