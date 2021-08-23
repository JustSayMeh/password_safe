using System.Net;
using System.Text;


namespace PasswordStore.Factories
{
    enum ProtocolVersions
    {
        V01,
        LASTVERSION
    }
    class CryptoFileFactory
    {
        public static CryptoFile CreateCryptoFile(ProtocolVersions version, NetworkCredential password, string crypt_string)
        {
            switch (version)
            {
                case ProtocolVersions.LASTVERSION:
                case ProtocolVersions.V01:
                    return V01creator(password, crypt_string);
            }
            return new CryptoFile();
        }

        private static CryptoFile V01creator(NetworkCredential password, string crypt_string)
        {
            Crypter crypter = new Crypter(password.Password);
            byte[] encrypted = crypter.Encrypt(crypt_string);
            byte[] salty = UsefulTools.ComputeSaltySHA256(Encoding.UTF8.GetBytes(crypt_string), crypter.Salt);
            return new CryptoFile(encrypted, salty, crypter.IV, crypter.Salt, 0x01);
        }
    }
}
