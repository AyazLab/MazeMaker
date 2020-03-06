using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace MazeMaker
{
    
    class Kayit
    {        
        private static string  kullanici = "RJZuyYN";
        private static string  sifre = "vHNstI5";

        public static bool Check(string user, string code)
        {
            string c1 = Decrypt(code);
            if (c1.Length>0 && user.CompareTo(c1) == 0)
                return true;
            return false;
        }

        private static string Encrypt(string input)
        {
            try
            {
                return Encrypt(input, "v2.6");
            }
            catch
            {
                return "";
            }
        }

        private static string Decrypt(string input)
        {
            try
            {
                return Decrypt(input, "v2.6");
            }
            catch
            {
                return "";
            }
        }

        private static string Encrypt(string data, string password)
        {
            if (String.IsNullOrEmpty(data))
                throw new ArgumentException("No data given");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("No password given");

            // setup the encryption algorithm
            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, 8);
            Rijndael aes = Rijndael.Create();
            aes.IV = keyGenerator.GetBytes(aes.BlockSize / 8);
            aes.Key = keyGenerator.GetBytes(aes.KeySize / 8);

            // encrypt the data
            byte[] rawData = Encoding.Unicode.GetBytes(data);
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                memoryStream.Write(keyGenerator.Salt, 0, keyGenerator.Salt.Length);
                cryptoStream.Write(rawData, 0, rawData.Length);
                cryptoStream.Close();

                byte[] encrypted = memoryStream.ToArray();
                //return Encoding.Unicode.GetString(encrypted);
                return Convert.ToBase64String(encrypted);
            }
        }

        private static string Decrypt(string data, string password)
        {
            if (String.IsNullOrEmpty(data))
                throw new ArgumentException("No data given");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("No password given");

            //byte[] rawData = Encoding.Unicode.GetBytes(data);
            byte[] rawData = Convert.FromBase64String(data);
            if (rawData.Length < 8)
                throw new ArgumentException("Invalid input data");

            // setup the decryption algorithm
            byte[] salt = new byte[8];
            for (int i = 0; i < salt.Length; i++)
                salt[i] = rawData[i];

            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, salt);
            Rijndael aes = Rijndael.Create();
            aes.IV = keyGenerator.GetBytes(aes.BlockSize / 8);
            aes.Key = keyGenerator.GetBytes(aes.KeySize / 8);

            // decrypt the data
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(rawData, 8, rawData.Length - 8);
                cryptoStream.Close();

                byte[] decrypted = memoryStream.ToArray();
                return Encoding.Unicode.GetString(decrypted);
            }
        }

        public static bool SetRegistry(string user, string code)
        {
            if(Kayit.Check(user,code) ==false ) return false;
            // The name of the key must include a valid root.
            const string userRoot = "HKEY_LOCAL_MACHINE\\SOFTWARE";
            const string subkey = "MM";
            const string keyName = userRoot + "\\" + subkey;
            Registry.SetValue(keyName, kullanici, GetCoded(user));
            Registry.SetValue(keyName, sifre, GetCoded(code));
            return true;
        }

        public static bool CheckRegistry()
        {
            // The name of the key must include a valid root.
            const string userRoot = "HKEY_LOCAL_MACHINE\\SOFTWARE";
            const string subkey = "MM";
            const string keyName = userRoot + "\\" + subkey;

            try
            {
                string user = GetPlain((string)Registry.GetValue(keyName, kullanici, ""));
                string code = GetPlain((string)Registry.GetValue(keyName, sifre, ""));
                return DoProcess(user, code);
            }
            catch
            {
                return false;
            }
        }

        public static string GetUser()
        {
            // The name of the key must include a valid root.
            const string userRoot = "HKEY_LOCAL_MACHINE\\SOFTWARE";
            const string subkey = "MM";
            const string keyName = userRoot + "\\" + subkey;

            try
            {
                string user = GetPlain((string)Registry.GetValue(keyName, kullanici, "-"));
                return user;
            }
            catch
            {
                return "";
            }
        }
        private static string GetCoded(string inp)
        {
            return Encrypt(inp);
            //return inp;
        }
        private static string GetPlain(string inp)
        {
            return Decrypt(inp);
            //return inp;
        }
        private static bool DoProcess(string user, string code)
        {
            if (user != null && user != "" && code != null && code != "")
            {
                if(user.CompareTo(Decrypt(code))==0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
