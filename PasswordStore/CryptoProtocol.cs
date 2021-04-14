using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PasswordStore
{
    public struct CryptoFile
    {
        public byte[] Cipher_text;
        public byte[] Hash;
        public byte[] IV;
        public byte[] Salt;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cipher_text">Шифротекст</param>
        /// <param name="hash">соленный хэш</param>
        /// <param name="iv">вектор инициализации</param>
        /// <param name="salt">соль</param>
        public CryptoFile(byte[] cipher_text, byte[] hash, byte[] iv, byte[] salt)
        {
            Cipher_text = cipher_text;
            Hash = hash;
            IV = iv;
            Salt = salt;
        }
    }
    class CryptoProtocol
    {
        private string plaindata;
        private Crypter crypter;
        private const byte version_byte = 0x01;
        private const byte cipher_byte = 0xd0;
        private const byte hash_byte = 0xd1;
        private const byte IV_byte = 0xd2;
        private const byte salt_byte = 0xd3;


        public static void Save(CryptoFile crypto_file, string filepath)
        {

            using (BinaryWriter writer = new BinaryWriter(File.Open(filepath, FileMode.Create)))
            {
                writer.Write(version_byte);
                writer.Write(cipher_byte);
                writer.Write(crypto_file.Cipher_text.Length);
                writer.Write(crypto_file.Cipher_text);
                writer.Write(hash_byte);
                writer.Write(32);
                writer.Write(crypto_file.Hash);
                writer.Write(IV_byte);
                writer.Write(crypto_file.IV.Length);
                writer.Write(crypto_file.IV);
                writer.Write(salt_byte);
                writer.Write(crypto_file.Salt.Length);
                writer.Write(crypto_file.Salt);
            }
        }


        public static CryptoFile Read(string filepath)
        {
            CryptoFile file = new CryptoFile();
            int size;

            using (BinaryReader streamReader = new BinaryReader(File.Open(filepath, FileMode.OpenOrCreate)))
            {
                if (streamReader.BaseStream.Position == streamReader.BaseStream.Length)
                {
                    throw new FileFormatException("Invalid store file format!");
                }
                byte version = streamReader.ReadByte();
                if (streamReader.BaseStream.Position == streamReader.BaseStream.Length)
                {
                    throw new FileFormatException("Invalid store file format!");
                }
                while (streamReader.BaseStream.Position != streamReader.BaseStream.Length)
                {
                    byte code = streamReader.ReadByte();
                    switch (code)
                    {
                        case cipher_byte:
                            size = streamReader.ReadInt32();
                            file.Cipher_text = streamReader.ReadBytes(size);
                            break;
                        case hash_byte:
                            size = streamReader.ReadInt32();
                            file.Hash = streamReader.ReadBytes(size);
                            break;
                        case IV_byte:
                            size = streamReader.ReadInt32();
                            file.IV = streamReader.ReadBytes(size);
                            break;
                        case salt_byte:
                            size = streamReader.ReadInt32();
                            file.Salt = streamReader.ReadBytes(size);
                            break;
                        default:
                            throw new FileFormatException("Invalid store file format!");
                    }
                }
            }
            return file;
        }
        public static CryptoFile getEmptyStore()
        {
            CryptoFile cryptoFile = new CryptoFile();
            return cryptoFile;
        }
    }
}
