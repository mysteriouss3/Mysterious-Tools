namespace MysteriousTools.HashServices.CriptoProvider
{
    public class HashGet
    {
        public static string MD5(string text) => HashServices.Instance.MD5Hash(text);
        public static string SHA1(string text) => HashServices.Instance.SHA1Hash(text);
        public static string SHA256(string text) => HashServices.Instance.SHA256Hash(text);
        public static string SHA384(string text) => HashServices.Instance.SHA384Hash(text);
        public static string SHA512(string text) => HashServices.Instance.SHA512Hash(text);
    }
}
