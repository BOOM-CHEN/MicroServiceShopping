using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingUser.Application.Utils
{
    public static class PasswordAES
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>cipherText密文</returns>
        public static byte[] Encrypt(string passWord, out byte[] publicKey, out byte[] IV)
        {
            using (Aes aes = Aes.Create())
            {
                publicKey = aes.Key;
                IV = aes.IV;
                return Encrypt_AES(passWord, publicKey, IV);
            }

        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <returns>密码</returns>
        public static String Decrypt(byte[] cipherText, byte[] publicKey, byte[] IV)
        {
            return Decrypt_AES(cipherText, publicKey, IV);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="publicKey">密钥</param>
        /// <returns></returns>
        private static byte[] Encrypt_AES(string passWord, byte[] publicKey, byte[] IV)
        {
            // Check arguments.
            if (passWord == null || passWord.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (publicKey == null || publicKey.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = publicKey;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(passWord);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="publicKey">密钥</param>
        /// <returns></returns>
        private static string Decrypt_AES(byte[] cipherText, byte[] publicKey, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (publicKey == null || publicKey.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = publicKey;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
