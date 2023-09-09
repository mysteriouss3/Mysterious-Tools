using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousTools.HashServices.AES256Provider
{
    public interface IHashProvider
    {
        string EncryptString(string plainText, byte[] key, byte[] iv);
        string DecryptString(string cipherText, byte[] key, byte[] iv);
    }
}
