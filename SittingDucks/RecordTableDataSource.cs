using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using Mono.Data.Sqlite;

namespace SittingDucks
{
    public class RecordTableDataSource : NSTableViewDataSource
    {
        public RecordTableDataSource()
        {
            Records = new List<Record>();
        }

        public RecordTableDataSource(SqliteConnection connection)
        {
            Records = new List<Record>();
            bool shouldClose;

            (_conn, shouldClose) = SqliteManager.OpenConnection(connection);

            // Execute query
            using (var command = connection.CreateCommand())
            {
                // Create new command
                command.CommandText = "SELECT * FROM [Data]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Pull values back into class
                        var ID = (string)reader[0];
                        var website = EncryptionTool.Decrypt((byte[])reader[1]);
                        var account = EncryptionTool.Decrypt((byte[])reader[2]);
                        var password = EncryptionTool.Decrypt((byte[])reader[3]);
                        if (Records.Any(x => x.ID.ToString() == ID))
                        {
                            break;
                        }
                        else
                        {
                            Records.Add(new Record(website, account, password, connection, new Guid(ID), false));
                        }
                    }
                }
            }

            _conn = SqliteManager.CloseConnection(shouldClose, connection);
        }

        public void AddRecord(Record record, nint? index)
        {
            if (index.HasValue)
            {
                Records.Insert((int)index.Value, record);
            }
            else
            {
                Records.Add(record);
            }
        }

        public void RemoveRecord(Record record, SqliteConnection connection)
        {
            Records.Remove(record);
            bool shouldClose;

            (_conn, shouldClose) = SqliteManager.OpenConnection(connection);

            // Execute query
            using (var command = connection.CreateCommand())
            {
                // Create new command
                command.CommandText = "DELETE FROM [Data] WHERE (ID = @COL1)";

                // Delete data from the record
                command.Parameters.AddWithValue("@COL1", record.ID.ToString());

                // Write to database
                command.ExecuteNonQuery();
            }

            _conn = SqliteManager.CloseConnection(shouldClose, connection);
        }

        private SqliteConnection _conn;

        public List<Record> Records { get; set; }

        public override nint GetRowCount(NSTableView tableView)
        {
            return Records.Count;
        }
    }
}
