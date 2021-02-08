using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Music.SDK.Tools
{
    public static class AESTools
    {
        public static string Encrypt(string text, string password, string iv)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.KeySize = 128;
            rijndaelManaged.BlockSize = 128;

            byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelManaged.Key = keyBytes;

            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            rijndaelManaged.IV = ivBytes;

            ICryptoTransform iCryptoTransform = rijndaelManaged.CreateEncryptor();
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] targetBytes = iCryptoTransform.TransformFinalBlock(textBytes, 0, textBytes.Length);
            return Convert.ToBase64String(targetBytes);
        }
    }
}
