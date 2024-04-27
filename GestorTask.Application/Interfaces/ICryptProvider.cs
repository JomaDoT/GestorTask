namespace GestorTask.Helpers;

public interface ICryptProvider
{
    string Encrypt(string str);
    string Decrypt(string str);
    string EncryptWithEncode(string str);
    string DecryptWithEncode(string str);
    T DecryptWithEncode<T>(string str);
}
