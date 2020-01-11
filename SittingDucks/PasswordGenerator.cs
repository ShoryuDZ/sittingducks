using System;
using System.Linq;
using System.Security.Cryptography;

namespace SittingDucks
{
    public static class PasswordGenerator
    {
        public static RNGCryptoServiceProvider RandomNumbers = new RNGCryptoServiceProvider();

        public static string GeneratePassword()
        {
            const int length = 16;

            var characterSet = new [] { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "0123456789", "!@#$%^&*-`~,.;<>?" };
         
            var bytes = new byte[length * 8];
            RandomNumbers.GetBytes(bytes);
            var result = new char[length];

            //Creating four groups, each containing one character from each of the four groups
            for (var outerCounter = 0;  outerCounter < 4; outerCounter++)
            {
                var innerCounter = 0;

                foreach (var characterGroup in characterSet)
                {
                    var index = (outerCounter * 4) + innerCounter;
                    ulong value = BitConverter.ToUInt64(bytes, index * 8);
                    result[index] = characterGroup[(int)(value % (uint)characterGroup.Length)];
                    innerCounter++;
                }
            }

            //Shuffling the string before returning
            return new string(result.OrderBy(x => Guid.NewGuid()).ToArray());
        }
    }
}
