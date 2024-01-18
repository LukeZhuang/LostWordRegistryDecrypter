using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LostWordRegistryDecrypter
{
    internal class Program
    {
        private const string m_aespw = "dYrQM2pRZMYEFEha";
        static void Main(string[] args)
        {
            string path = @"eastreg_example_jp.ini";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            string src = br.ReadString();
            Console.WriteLine("Input:");
            Console.WriteLine(src);

            byte[] keyBytes = Encoding.GetEncoding("UTF-8").GetBytes(m_aespw);
            byte[] ivBytes = Enumerable.Repeat((byte)0, 16).ToArray();
            byte[] srcBytes = Convert.FromBase64String(src);
            byte[] tgtBytes = new byte[srcBytes.Length];

            using (var AES = new AesManaged { Padding = PaddingMode.PKCS7, Mode = CipherMode.CBC, KeySize = 128 })
            {
                using (var memoryStream = new MemoryStream(srcBytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, AES.CreateDecryptor(keyBytes, ivBytes), CryptoStreamMode.Read))
                    {
                        cryptoStream.Read(tgtBytes, 0, tgtBytes.Length);
                        string plainText = Encoding.UTF8.GetString(tgtBytes, 0, tgtBytes.Length);
                        Console.WriteLine("Output:");
                        Console.WriteLine(plainText);
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
