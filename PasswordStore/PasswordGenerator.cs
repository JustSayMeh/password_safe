using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStore
{
   

    class PasswordGenerator
    {
        private const string latin = "abcdefghijklmnopqrstuvwxyz";
        private const string numbers = "0123456789";
        private const string special_symbols = "!\"#$%&'()*+,-./\\;><=?@|{}`~";
        private const string latin_upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string kyrilic_upper = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private const string kyrilic = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        private uint flags = 1;
        private enum Flags
        {
            LATIN = 1,
            NUMBERS = 2,
            SPECIAL_SYMBOLS = 4,
            LATIN_UPPER = 8,
            KYRILIC = 16,
            KYRILIC_UPPER = 32
        }
        public void Enable_numbers() => flags ^= (uint)Flags.NUMBERS;
        public void Enable_special_symbols() => flags ^= (uint)Flags.SPECIAL_SYMBOLS;
        public void Enable_latin_upper() => flags ^= (uint)Flags.LATIN_UPPER;
        public void Enable_kyrilic_upper() => flags ^= (uint)Flags.KYRILIC_UPPER;
        public void Enable_kyrilic() => flags ^= (uint)Flags.KYRILIC;

        

        public NetworkCredential Generate(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if ((flags & (uint)Flags.LATIN) != 0 )
            {
                stringBuilder.Append(latin);
            }
            if ((flags & (uint)Flags.NUMBERS) != 0)
            {
                stringBuilder.Append(numbers);
            }
            if ((flags & (uint)Flags.LATIN_UPPER) != 0)
            {
                stringBuilder.Append(latin_upper);
            }
            if ((flags & (uint)Flags.KYRILIC) != 0)
            {
                stringBuilder.Append(kyrilic);
            }
            if ((flags & (uint)Flags.KYRILIC_UPPER) != 0)
            {
                stringBuilder.Append(kyrilic_upper);
            }
            if ((flags & (uint)Flags.SPECIAL_SYMBOLS) != 0)
            {
                stringBuilder.Append(special_symbols);
            }
            string str = stringBuilder.ToString();
            //str = UsefulTools.GenerateRandomString(str, str.Length);
            SecureString secureString = UsefulTools.GenerateRandomSecureString(str, length);
            return new NetworkCredential("", secureString);
        }
    }
}
