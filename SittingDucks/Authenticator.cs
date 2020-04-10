using System;
using System.Collections.Generic;
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

            byte[] encryptedPassword = { };
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
                        encryptedPassword = (byte[])reader[1];
                        isInit = (bool)reader[2];
                    }
                }
            }

            var password = EncryptionTool.Decrypt(encryptedPassword);

            if (isInit == true)
            {
                CheckPassword(connection, _conn, password);
            }
            else
            {
                NewPassword(connection, _conn);
            }
            
            _conn = SqliteManager.CloseConnection(shouldClose, connection);
        }

        public static void CheckPassword(SqliteConnection connection, SqliteConnection _conn, string password, bool isError = false)
        {
            var passwordInput = new NSSecureTextField(new CGRect(0, 0, 300, 20));
            passwordInput.PlaceholderAttributedString = new Foundation.NSAttributedString("Enter password...");

            var passwordAlert = new NSAlert()
            {
                AlertStyle = isError ? NSAlertStyle.Critical : NSAlertStyle.Informational,
                InformativeText = isError ? "Previous password attempt was incorrect, please retry" : "Enter SittingDucks Password",
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
                CheckPassword(connection, _conn, password, true);
            }
            else if (result == 1001)
            {
                NewPassword(connection, _conn);
            }
        }

        public static void NewPassword(SqliteConnection connection, SqliteConnection _conn)
        {
            var newPasswordInput = new NSStackView(new CGRect(0, 0, 300, 50));

            var originalPassword = new NSSecureTextField(new CGRect(0, 25, 300, 20));
            originalPassword.PlaceholderAttributedString = new Foundation.NSAttributedString("Type new password...");
            var confirmedPassword = new NSSecureTextField(new CGRect(0, 0, 300, 20));
            confirmedPassword.PlaceholderAttributedString = new Foundation.NSAttributedString("Confirm password...");

            newPasswordInput.AddSubview(originalPassword);
            newPasswordInput.AddSubview(confirmedPassword);

            var newPasswordAlert = new NSAlert()
            {
                AlertStyle = NSAlertStyle.Informational,
                InformativeText = "Enter new password to secure SittingDucks",
                MessageText = "Adding New Password",
            };
            var enterButton = newPasswordAlert.AddButton("Enter");
            originalPassword.NextKeyView = confirmedPassword;
            confirmedPassword.NextKeyView = enterButton;

            newPasswordAlert.AccessoryView = newPasswordInput;
            newPasswordAlert.Layout();
            var result = newPasswordAlert.RunModal();

            if (result == 1000 && originalPassword.StringValue == confirmedPassword.StringValue)
            {
                bool shouldClose;
                var encryptedPassword = EncryptionTool.Encrypt(originalPassword.StringValue);

                (_conn, shouldClose) = SqliteManager.OpenConnection(connection);

                // Execute query
                using (var command = connection.CreateCommand())
                {
                    // Create new command
                    command.CommandText = "UPDATE [System] SET ID = @COL1, Password = @COL2, INIT = @COL3";

                    // Populate with data from the record
                    command.Parameters.AddWithValue("@COL1", new Guid());
                    command.Parameters.AddWithValue("@COL2", encryptedPassword);
                    command.Parameters.AddWithValue("@COL3", true);

                    // Write to database
                    command.ExecuteNonQuery();
                }

                _conn = SqliteManager.CloseConnection(shouldClose, connection);

                newPasswordAlert.Dispose();

                var confirmPasswordAlert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Informational,
                    InformativeText = "Remember this password, store it somewhere safe! You will not be able to recover it if lost.",
                    MessageText = "Password sucessfully saved",
                };
                confirmPasswordAlert.AddButton("OK");
                var confirmResult = confirmPasswordAlert.RunModal();

                if (confirmResult == 1000)
                {
                    confirmPasswordAlert.Dispose();
                }
            }
            else if (result == 1000 && originalPassword.StringValue != confirmedPassword.StringValue)
            {
                newPasswordAlert.AlertStyle = NSAlertStyle.Warning;
                newPasswordAlert.InformativeText = "Passwords do not match";
            }
        }
    }
}
