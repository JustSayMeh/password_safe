using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace PasswordStore
{
    class UsefulTools
    {
        public static string GenerateRandomString(string str, int length)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(str, length).Select(s => s[random.Next(str.Length)]).ToArray());
        }

        private static uint[] generateNumbers(int length)
        {
            byte[] bytes = new byte[length * sizeof(int)];
            uint[] numbers = new uint[length];
            RandomNumberGenerator randomNumberGenerator = new RNGCryptoServiceProvider();
            randomNumberGenerator.GetBytes(bytes);
            for (int i = 0; i < length; i++)
            {
                numbers[i] = BitConverter.ToUInt32(bytes, i * 4);
            }
            return numbers;
        }

        public static SecureString GenerateRandomSecureString(string str, int length)
        {
            SecureString secureString = new SecureString();
            uint[] numbers = generateNumbers(length);
            for (int i = 0; i < length; i++)
            {
                uint index = (uint)(numbers[i] % str.Length);
                secureString.AppendChar(str[Math.Abs((int)index)]);
            }
            return secureString;
        }

        public static byte[] GenerateSalt(int size)
        {
            byte[] salt = new byte[size];
            Random random = new Random();
            random.NextBytes(salt);
            return salt;
        }


        public static byte[] ComputeSaltySHA256(byte[] data, byte[] salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] salted_data = new byte[data.Length + salt.Length];
                data.CopyTo(salted_data, 0);
                salt.CopyTo(salted_data, data.Length);
                return sha256.ComputeHash(salted_data);
            }
        }
    }
}

