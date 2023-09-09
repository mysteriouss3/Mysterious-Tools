using System;
using System.Security.Cryptography;
using System.Text;

namespace MysteriousTools.HashServices.CriptoProvider
{
    public class HashServices : IHashService
    {
        public HashServices() { }

        private static HashServices _instance;

        public static HashServices Instance
        {
            get
            {
                if (_instance == null)
                    return _instance = new HashServices();
                return _instance;
            }
        }

        public string MD5Hash(string text)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] dizi = Encoding.UTF8.GetBytes(text);
                dizi = md5.ComputeHash(dizi);
                StringBuilder sb = new StringBuilder();
                foreach (byte ba in dizi)
                {
                    sb.Append(ba.ToString("x2").ToLower());
                }
                return sb.ToString();
            }
        }

        public string SHA1Hash(string text)
        {
            string source = text;
            using (SHA1 sha1Hash = SHA1.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }

        public string SHA256Hash(string text)
        {
            string source = text;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }

        public string SHA384Hash(string text)
        {
            string source = text;
            using (SHA384 sha384Hash = SHA384.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha384Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }

        public string SHA512Hash(string text)
        {
            string source = text;
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }
    }
}