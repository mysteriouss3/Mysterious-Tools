using System.Security.Cryptography;
using System.Text;

namespace MysteriousTools.HashServices.AES256Provider
{
    public class Hashing : HashProvider
    {
        public Hashing() { }

        private static Hashing _instance;

        public static Hashing Instance
        {   
            get
            {
                if (_instance == null)
                    _instance = new Hashing();
                return _instance;
            }
        }

        private static string Key { get; set; }

        /// <summary>
        /// Şifreleme Yapmak için öncellikle secret keyi tanimlamak gerekiyor
        /// </summary>
        /// <param name="PrivateKey">Şifrelenecek Text'e Secret Key Yaz</param>
        public static void Initialize(string PrivateKey)
        {
            Key = PrivateKey;
        }
        public string Sifrele(string CipherText)
        {
            string message = CipherText;
            string password = Key;

            // Create sha256 hash
            SHA256 mySHA256 = SHA256.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Create secret IV
            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            string encrypted = EncryptString(message, key, iv);
            return ConvertToHashedString(encrypted, true);
        }
        public string SifreCoz(string ByteText)
        {
            string message = ConvertToHashedString(ByteText, false);
            string password = Key;

            // Create sha256 hash
            SHA256 mySHA256 = SHA256.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Create secret IV
            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            string decrypted = DecryptString(message, key, iv);
            return decrypted;
        }
    }
}
