using System;
using System.Security.Cryptography;
using System.Text;

namespace EncryptingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var message = Console.ReadLine();

            var hash = "f0xle@rn";

            var txtEncrypt = Encrypt(message, hash);
            
            var txtDecrypt = Decrypt(txtEncrypt, hash);

            Console.WriteLine(txtEncrypt);
            
            Console.WriteLine(txtDecrypt);
        }

        public static string Encrypt(string message, string hash)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(message);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7})
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }
        
        public static string Decrypt(string message, string hash)
        {
            byte[] data = Convert.FromBase64String(message);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7})
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    return UTF8Encoding.UTF8.GetString(result);
                }
            }
        }
    }
}