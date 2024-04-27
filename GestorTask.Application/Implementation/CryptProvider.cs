using GestorTask.Application.Models;
using GestorTask.Applications.Helpers;

namespace GestorTask.Helpers;

public class CryptProvider : ICryptProvider
{
    private readonly AESCrypt _Crypt;

    public CryptProvider(SecuritySetting securitySetting)
    {
        _Crypt = new AESCrypt(securitySetting.Key, securitySetting.Salt);
    }

    public string Decrypt(string str)
    {
        return _Crypt.Decrypt(str);
    }

    public string DecryptWithEncode(string str)
    {
        return _Crypt.Decrypt(System.Web.HttpUtility.UrlDecode(str));
    }
    public string EncryptWithEncode(string str)
    {
        return System.Web.HttpUtility.UrlDecode(_Crypt.Encrypt(str).Replace("/", ",").Replace("+", "%2E").Replace("==", "="));
    }

    public T DecryptWithEncode<T>(string str)
    {
        return (T)Convert.ChangeType(System.Web.HttpUtility.UrlDecode(_Crypt.Decrypt(str.Replace(",", "/").Replace(".", "+").Replace("=", "=="))), typeof(T));
    }

    public string Encrypt(string str)
    {
        return _Crypt.Encrypt(str);
    }


}
