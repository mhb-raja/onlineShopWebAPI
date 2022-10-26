using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security
{
    public class PasswordHelper : IPasswordHelper
    {
        public string EncodePasswordMd5(string password)
        {
            byte[] originalBytes;
            byte[] encodedBytes;
            MD5 md5;
            md5 = MD5.Create();// new  md5cryptoserviceprovider(); ##############????????
            originalBytes = ASCIIEncoding.Default.GetBytes(password);
            encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }
    }
}
