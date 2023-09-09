namespace MysteriousTools.HashServices.CriptoProvider
{
    public interface IHashService
    {
        string MD5Hash(string text);
        string SHA1Hash(string text);
        string SHA256Hash(string text);
        string SHA384Hash(string text);
        string SHA512Hash(string text);
    }
}
