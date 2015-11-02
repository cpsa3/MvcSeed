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
            const int workFactor = 12;

            var start = DateTime.UtcNow;
            var hashed = HashHelper.BCryptPassword(password, workFactor);
            var end = DateTime.UtcNow;
            var hashed2 = HashHelper.BCryptPassword(password, workFactor);

            var elapsedTime = end - start;

            Assert.IsTrue(HashHelper.BCryptVerify("PASSWORD", hashed));
            Assert.IsFalse(HashHelper.BCryptVerify("PASSWORd", hashed));
            Assert.IsTrue(HashHelper.BCryptVerify("PASSWORD", hashed2));
            Assert.IsFalse(HashHelper.BCryptVerify("PASSWORd", hashed2));
            Assert.AreEqual(hashed.Length, 60);
            Assert.AreNotEqual(hashed, hashed2);
        }
    }
}
