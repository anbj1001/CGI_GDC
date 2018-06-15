using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DatabaseAdmin.DatabaseConnections
{
    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {// Funkar
            MD5 md5 = new MD5CryptoServiceProvider();

            //hasha texten
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //resultat av hash 
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //ändra till 2 hexadecimal för varje byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}


