using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using McvSeed.Web.Models;
using McvSeed.Web.Security;
using MvcSeed.Component.Helpers;

namespace McvSeed.Web.Controllers
{
    public class HomeController : BaseController
    {
        private const string USERNAME = "cpsa3";
        private const string PASSWORD = "123456";
        private const long USERID = 1;

        [HttpGet]
        public ActionResult Login()
        {
            var rsa = new RSACryptoServiceProvider();
            string publickey = rsa.ToXmlString(false);
            var privateKey = rsa.ToXmlString(true);
            //将私钥存Session中
            DiMySession["PrivateKey"] = privateKey;

            const string strPassword = "123456";

            var str1 = HashHelper.RSAEncrypt(strPassword, publickey);

            var rsaToEncrypt = new RSACryptoServiceProvider();
            rsaToEncrypt.FromXmlString(publickey);
            byte[] byEncrypted = rsaToEncrypt.Encrypt(Encoding.UTF8.GetBytes(strPassword), false);
            var x = BitConverter.ToString(byEncrypted).Replace("-", "").ToLower();

            var data = SoapHexBinary.Parse(x);

            var str = HashHelper.RSADecrypt(x, privateKey);



            //把公钥适当转换
            var parameter = rsa.ExportParameters(true);
            var strPublicKeyExponent = HashHelper.BytesToHexString(parameter.Exponent);
            var strPublicKeyModulus = HashHelper.BytesToHexString(parameter.Modulus);

            ViewBag.strPublicKeyExponent = strPublicKeyExponent;
            ViewBag.strPublicKeyModulus = strPublicKeyModulus;

            return View();
        }

        [HttpPost]
        public ActionResult LoginDo(LoginDto dto)
        {
            var privateKey = DiMySession.Get<string>("PrivateKey");
            var password = HashHelper.RSADecrypt(dto.Password, privateKey);

            if (dto.UserName == USERNAME && password == PASSWORD)
            {
                var currentUser = CurrentContext.GetCurrentUser();
                currentUser.UserId = USERID;
                currentUser.UserName = USERNAME;
                CurrentContext.SetUser(currentUser);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            CurrentContext.ClearInfo();
            return RedirectToAction("Login");
        }

        [Authentication]
        public ActionResult Index()
        {
            return View();
        }
    }
}