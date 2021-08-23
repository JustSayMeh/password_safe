using PasswordStore.Factories;
using System.Collections;
using System.Net;
using System.Text;


namespace PasswordStore.Verificators
{
    class PasswordVerificator
    {
        public static bool PasswordIsValid(ProtocolVersions version, NetworkCredential password, CryptoFile cryptoFile)
        {
            switch (version)
            {
                case ProtocolVersions.LASTVERSION:
                case ProtocolVersions.V01:
                    return V01(password, cryptoFile);
            }
            return false;
        }

        private static bool V01(NetworkCredential password, CryptoFile cryptoFile)
        {
            Crypter crypter = new Crypter(password.Password, cryptoFile.Salt, cryptoFile.IV);
            string decrypt_data = crypter.Decrypt(cryptoFile.Cipher_text);
            byte[] hash = UsefulTools.ComputeSaltySHA256(Encoding.UTF8.GetBytes(decrypt_data), crypter.Salt);
            return StructuralComparisons.StructuralEqualityComparer.Equals(hash, cryptoFile.Hash);
        }
    }
}
