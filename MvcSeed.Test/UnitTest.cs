﻿using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSeed.Component.Helpers;
using MvcSeed.Component.Security;

namespace MvcSeed.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void BCryptTest()
        {
            const string password = "PASSWORD";
            const int workFactor = 12;

            var start = DateTime.UtcNow;
            var bcryptHashed = HashHelper.BCryptPassword(password, workFactor);
            var end = DateTime.UtcNow;
            var elapsedTime = end - start;

            var s = DateTime.UtcNow;
            //var md5Hashed = HashHelper.Md5("wangjiebo");
            //var sha1Hashed = HashHelper.SHA1("wangjiebo");
            var pbkdf2Hashed = HashHelper.PBKDF2(password);
            var e = DateTime.UtcNow;
            var t = e - s;


            Assert.IsTrue(HashHelper.BCryptVerify("PASSWORD", bcryptHashed));
            Assert.IsFalse(HashHelper.BCryptVerify("PASSWORd", bcryptHashed));

            Assert.IsTrue(HashHelper.PBKDF2Verify("PASSWORD", pbkdf2Hashed));
            Assert.IsFalse(HashHelper.PBKDF2Verify("PASSWORd", pbkdf2Hashed));
            Assert.AreEqual(bcryptHashed.Length, 60);
        }

        [TestMethod]
        public void RSATest()
        {
            var rsa = new RSACryptoServiceProvider(1024);
            var publickey = rsa.ToXmlString(false);
            var privateKey = rsa.ToXmlString(true);

            const string strPassword = "123456";
            var encrypt = HashHelper.RSAEncrypt(strPassword, publickey);
            var decrypt = HashHelper.RSADecrypt(encrypt, privateKey);

            Assert.AreEqual(strPassword, decrypt);
        }
    }
}
