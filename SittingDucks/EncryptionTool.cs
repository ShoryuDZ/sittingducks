using System;
using System.Text;
using System.Security.Cryptography;

namespace SittingDucks
{
    public static class EncryptionTool
    {
        // Create byte array for additional entropy when using Protect method.
        static byte[] s_aditionalEntropy = { 9, 8, 7, 6, 5 };

        public static byte[] Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                // only by the same current user.
                var encryptedBytes = ProtectedData.Protect(plainTextBytes, s_aditionalEntropy, DataProtectionScope.CurrentUser);
                return encryptedBytes;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred. " + e.ToString() );
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public static string Decrypt(byte[] plainTextBytes)
        {
            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser.
                var decryptedBytes = ProtectedData.Unprotect(plainTextBytes, s_aditionalEntropy, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}