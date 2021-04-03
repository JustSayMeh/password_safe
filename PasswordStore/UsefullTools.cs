using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PasswordStore
{
    class UsefulTools
    {
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

