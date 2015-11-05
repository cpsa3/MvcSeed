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
            var rsa = new RSACryptoServiceProvider(1024);
            string publickey = rsa.ToXmlString(false);
            var privateKey = rsa.ToXmlString(true);
            //将私钥存Session中
            DiMySession["PrivateKey"] = privateKey;

            const string strPassword = "123456";
            var encrypt = HashHelper.RSAEncrypt(strPassword, publickey);
            var decrypt = HashHelper.RSADecrypt(encrypt, privateKey);


            //把公钥适当转换
            var parameter = rsa.ExportParameters(true);
            var strPublicKeyExponent = StringHelper.BytesToHexString(parameter.Exponent);
            var strPublicKeyModulus = StringHelper.BytesToHexString(parameter.Modulus);

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