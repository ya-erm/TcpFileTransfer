
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System;

namespace TCPClient.Logic.Utils
{
    public class MD5Hash
    {
        public static string FileHash(string filename)
        {
            if (!File.Exists(filename)) return null;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    var result = BitConverter.ToString(hash).Replace("-", "").ToUpper();
                    return result;
                }
            }
        }

        //public static byte[] GetHash(string inputString)
        //{
        //    HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
        //    return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        //}

        //public static string GetHashString(string inputString)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (byte b in GetHash(inputString))
        //        sb.Append(b.ToString("X2"));

        //    return sb.ToString();
        //}
    }
}
