using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace GestorTask.Applications.Helpers
{
    public class AESCrypt
    {
        #region Private Fields
        private readonly byte[] Key;
        private readonly byte[] IV;
        #endregion Private Fields

        #region Read-Only Properties
        public string Key_Str
        {
            get
            {
                return Convert.ToBase64String(Key);
            }
        }
        public string IV_Str
        {
            get
            {
                return Convert.ToBase64String(IV);
            }
        }
        #endregion Read-Only Properties

        #region Constructors
        public AESCrypt()
        {
            Aes AES_Alg = null;

            try
            {
                // Tip: When the object is created, Key and IV properties are initialized to random values.
                //      If the class is created using this constructors, we will use the random values.etc for that. 
                // Tip: Default Cipher Mode is: CipherMode.CBC
                Key = AES_Alg.Key;
                IV = AES_Alg.IV;
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (AES_Alg is null)
                    AES_Alg.Clear();
            }
        }
        public AESCrypt(string pass, string keySalt)
            : this(pass, Encoding.ASCII.GetBytes(keySalt))
        {

        }
        public AESCrypt(string pass, byte[] keySalt)
        {

            if (pass == null || pass.Length <= 0)
                throw new ArgumentNullException(pass);
            if (keySalt == null || keySalt.Length <= 0)
                throw new ArgumentNullException(keySalt.ToString());

            // Derive a Key and an IV from the Password and create an algorithm
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(pass, keySalt); // Weak - based on PBKDF1

            Key = pdb.GetBytes(32);
            IV = pdb.GetBytes(16);
        }
        #endregion Constructor
        #region Methods
        public string Encrypt(string str)
        {
            // Check arguments.
            if (str == null || str.Length <= 0)
                throw new ArgumentNullException(str);

            Aes AES_Alg = null;

            // Declare the stream used to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt = null;

            try
            {
                // Tip: When the object is created, Key and IV properties are initialized to random values.
                //      In this class we want fixed Key and IV values. We use a PasswordDeriveBytes objetc for that. 
                // Tip: Default Cipher Mode is: CipherMode.CBC
                AES_Alg.Key = Key;
                AES_Alg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = AES_Alg.CreateEncryptor(AES_Alg.Key, AES_Alg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();

                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using StreamWriter swEncrypt = new(csEncrypt);
                //Write all data to the stream.
                swEncrypt.Write(str);
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (AES_Alg is null)
                    AES_Alg.Clear();
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        public string Decrypt(string str)
        {

            byte[] cipherText = Convert.FromBase64String(str);

            // Check arguments.
            if (str == null || str.Length <= 0)
                throw new ArgumentNullException(str);

            // Declare the RijndaelManaged object
            // used to encrypt the data.
            Aes AES_Alg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // Tip: When the object is created, Key and IV properties are initialized to random values.
                //      In this class we want fixed Key and IV values. We use a PasswordDeriveBytes objetc for that. 
                AES_Alg.Key = Key;
                AES_Alg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = AES_Alg.CreateDecryptor(AES_Alg.Key, AES_Alg.IV);

                // Create the streams used for decryption.
                using MemoryStream msDecrypt = new(cipherText);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);

                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                plaintext = srDecrypt.ReadToEnd();
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (AES_Alg is null)
                    AES_Alg.Clear();
            }

            return plaintext;
        }
        public static string GenerarSalt(int iteraciones)
        {
            Random r = new();
            string salt = string.Empty;
            // Bucle para generar los caractéres del salt. n veces introducidas en la variable iteraciones
            for (int i = 1; i <= iteraciones; i++)
            {
                // Generamos un número aleatorio
                int aleatorio = r.Next(1, 255);
                // Obtenemos el valor de la letra en formato ASCII del número generado aleatoriamente
                char letra = Convert.ToChar(aleatorio);
                // Lo concatenamos al salt
                salt += letra;
            }
            // Devolvemos el salt
            return salt;
        }
        public static string CreateMD5(string input)
        {
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("X2"));

            return sb.ToString();
        }
        #endregion Methods
    }
}
