using System;
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
            this.ShowPassword = false;

            if (id == default(Guid))
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
            bool shouldClose;

            (_conn, shouldClose) = SqliteManager.OpenConnection(connection);

            // Execute query
            using (var command = connection.CreateCommand())
            {
                // Create new command
                command.CommandText = "INSERT INTO [Data] (ID, Website, Account, Password) VALUES (@COL1, @COL2, @COL3, @COL4)";

                // Populate with data from the record
                command.Parameters.AddWithValue("@COL1", ID.ToString());
                command.Parameters.AddWithValue("@COL2", EncryptionTool.Encrypt(Website));
                command.Parameters.AddWithValue("@COL3", EncryptionTool.Encrypt(AccountName));
                command.Parameters.AddWithValue("@COL4", EncryptionTool.Encrypt(Password));

                // Write to database
                command.ExecuteNonQuery();
            }

            _conn = SqliteManager.CloseConnection(shouldClose, connection);
        }

        public string Website { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public Guid ID { get; set; }
        public bool ShowPassword { get; set; }

        private SqliteConnection _conn = null;
    }
}
