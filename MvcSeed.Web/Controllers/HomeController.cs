using MvcSeed.Web.Models;
using MvcSeed.Web.Security;
using MvcSeed.Component.Helpers;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace MvcSeed.Web.Controllers
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
            var privateKey = rsa.ToXmlString(true);

            //将私钥存Session中
            DiMySession["PrivateKey"] = privateKey;

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