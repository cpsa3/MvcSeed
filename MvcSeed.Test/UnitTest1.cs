using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSeed.Component.Helpers;

namespace MvcSeed.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BCryptTest()
        {
            const string password = "PASSWORD";
            const int workFactor = 14;

            var start = DateTime.UtcNow;
            var hashed = HashHelper.BCryptPassword(password, workFactor);
            var end = DateTime.UtcNow;
            var elapsedTime = end - start;

            var s = DateTime.UtcNow;
            var md5 = HashHelper.Md5("wangjiebo");
            var md52 = HashHelper.Md5Short("wangjiebo");
            var e = DateTime.UtcNow;
            var t = e - s;

            var hashed2 = HashHelper.BCryptPassword(password, workFactor);

            Assert.IsTrue(HashHelper.BCryptVerify("PASSWORD", hashed));
            Assert.IsFalse(HashHelper.BCryptVerify("PASSWORd", hashed));
            Assert.IsTrue(HashHelper.BCryptVerify("PASSWORD", hashed2));
            Assert.IsFalse(HashHelper.BCryptVerify("PASSWORd", hashed2));
            Assert.AreEqual(hashed.Length, 60);
            Assert.AreNotEqual(hashed, hashed2);
        }
    }
}
