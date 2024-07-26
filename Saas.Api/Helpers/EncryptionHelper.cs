using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Api.Helpers
{
    public static class EncryptionHelper
    {
        private const int KeySize = 256;

        public static byte[] GenerateRandomKey()
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.GenerateKey();
                return aes.Key;
            }
        }
        public static byte[] GenerateRandomIV()
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateIV();
                return aes.IV;
            }
        }

        public static string Encrypt(string data, byte[] encryptionKey, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] encryptedBytes = Encoding.UTF8.GetBytes(data);
                    byte[] encrypted = encryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public static string Decrypt(string data, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(data);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}