using AppKit;
using CoreGraphics;
using Mono.Data.Sqlite;

namespace SittingDucks
{
    public static class Authenticator
    {

        public static void Initialise(SqliteConnection connection, SqliteConnection _conn)
        {
            bool shouldClose;

            (_conn, shouldClose) = SqliteManager.OpenConnection(connection);

            string password = string.Empty;
            bool? isInit = null;

            using (var command = connection.CreateCommand())
            {
                // Create new command
                command.CommandText = "SELECT * FROM [System]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read() && isInit == null)
                    {
                        // Pull values back into class
                        password = (string)reader[1];
                        isInit = (bool)reader[2];
                    }
                }
            }

            if (isInit == true)
            {
                CheckPassword(password);
            }
            else
            {
                NewPassword();
            }
            
            _conn = SqliteManager.CloseConnection(shouldClose, connection);
        }

        public static void CheckPassword(string password, bool isError = false)
        {
            var passwordInput = new NSSecureTextField(new CGRect(0, 0, 300, 20));

            var passwordAlert = new NSAlert()
            {
                AlertStyle = isError ? NSAlertStyle.Critical : NSAlertStyle.Informational,
                InformativeText = isError ? "Previous password attempt was incorrect, please retry" : "Enter SittingDucks password",
                MessageText = "Authentication Required",
            };
            passwordAlert.AddButton("Enter");
            passwordAlert.AddButton("Change Password");
            passwordAlert.AccessoryView = passwordInput;
            passwordAlert.Layout();
            var result = passwordAlert.RunModal();

            passwordAlert.Dispose();

            if (passwordInput.StringValue != password)
            {
                CheckPassword(password, true);
            }
            else if (result == 1001)
            {
                NewPassword();
            }
        }

        public static void NewPassword()
        {
            //Mechanism for new password coming soon
        }
    }
}
