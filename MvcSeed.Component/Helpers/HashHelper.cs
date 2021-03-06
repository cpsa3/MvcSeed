﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MvcSeed.Component.Security;

namespace MvcSeed.Component.Helpers
{
    public class HashHelper
    {
        public static string BCryptPassword(string password, int workFactor = 12)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
        }

        public static bool BCryptVerify(string password, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }

        /// <summary>
        /// Md5加密，获取16位加密结果
        /// </summary>
        /// <param name="input">待加密的明文密码</param>
        /// <returns>返回16位加密结果，该结果取32位加密结果的第9位到25位</returns>
        public static string Md5Short(string input)
        {
            var md5 = new MD5CryptoServiceProvider();

            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            //转换成字符串，并取9到25位 
            string strResult = BitConverter.ToString(data, 4, 8);

            //转换成字符串，32位 
            //string strResult = BitConverter.ToString(data);

            //BitConverter转换出来的字符串会在每个字符中间产生一个分隔符，需要去除掉 
            strResult = strResult.Replace("-", "").ToLower();
            return strResult;
        }

        /// <summary>
        /// Md5加密，获取32位加密结果 
        /// </summary>
        /// <param name="input">待加密的明文密码</param>
        /// <returns>返回32位加密结果</returns>
        public static string Md5(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Return the hexadecimal string.
                return StringHelper.BytesToHexString(data);
            }
        }

        public static string SHA1(string input)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));

                return StringHelper.BytesToHexString(data);
            }
        }

        public static string PBKDF2(string input)
        {
            return PBKDF2Hash.CreateHash(input);
        }

        public static bool PBKDF2Verify(string password, string hashed)
        {
            return PBKDF2Hash.ValidatePassword(password, hashed);
        }

        public static string RSAEncrypt(string input, string publickey)
        {
            var rsaToEncrypt = new RSACryptoServiceProvider();
            rsaToEncrypt.FromXmlString(publickey);
            byte[] byEncrypted = rsaToEncrypt.Encrypt(Encoding.UTF8.GetBytes(input), false);

            return StringHelper.BytesToHexString(byEncrypted);
        }

        public static string RSADecrypt(string byEncryptedStr, string privatekey)
        {
            var data = StringHelper.HexStringToBytes(byEncryptedStr);
            return RSADecrypt(data, privatekey);
        }

        public static string RSADecrypt(byte[] byEncrypted, string privatekey)
        {
            var rsaToDecrypt = new RSACryptoServiceProvider();
            rsaToDecrypt.FromXmlString(privatekey);

            byte[] byDecrypted = rsaToDecrypt.Decrypt(byEncrypted, false);
            string strDecryptedPwd = Encoding.UTF8.GetString(byDecrypted);

            return strDecryptedPwd;
        }
    }
}
