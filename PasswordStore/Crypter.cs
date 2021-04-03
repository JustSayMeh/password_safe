using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordStore
{
    public class Crypter
    {
        public byte[] Key { get; private set; }
        public byte[] Salt { get; private set; }
        public byte[] IV { get; private set; }

        public Crypter(string pass, byte[] salt, byte[] IV)
        {
            Rfc2898DeriveBytes psw = new Rfc2898DeriveBytes(pass, salt);
            this.Salt = salt;
            this.IV = IV;
            Key = psw.GetBytes(32);
        }
        public Crypter(string pass) : this(pass, UsefulTools.GenerateSalt(32), UsefulTools.GenerateSalt(16))
        {
        }
        public byte[] Encrypt(string plain)
        {
            byte[] encrypted;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                rijAlg.Mode = CipherMode.CBC;
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plain);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public string Decrypt(byte[] cipher)
        {
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            // Create an Aes object
            // with the specified key and IV.
            using (RijndaelManaged aesAlg = new RijndaelManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipher))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
