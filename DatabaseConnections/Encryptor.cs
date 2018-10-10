using System.Text;
using System.Security.Cryptography;

namespace DatabaseAdmin.Model
{
    public static class Encryptor
    {
        /// <summary>
        /// Använder MD5 för kryptering av lösenord. Hashar texten och ändrar till 2 hexadecimal för varje byte i texten
        /// https://sv.wikipedia.org/wiki/Hexadecimala_talsystemet
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}


