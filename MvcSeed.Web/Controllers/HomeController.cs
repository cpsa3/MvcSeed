using MvcSeed.Component.Helpers;
using MvcSeed.Repository.Entity;
using MvcSeed.Web.Models;
using MvcSeed.Web.Security;
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
            return View();
        }

        [HttpPost]
        public JsonResult GetRSAPublicKey()
        {
            var rsa = new RSACryptoServiceProvider(1024);
            var privateKey = rsa.ToXmlString(true);

            //将私钥存Session中
            this.DiMySession["PrivateKey"] = privateKey;

            //把公钥适当转换
            var parameter = rsa.ExportParameters(true);

            return Json(new RSAPublicKey
            {
                Exponent = StringHelper.BytesToHexString(parameter.Exponent),
                Modulus = StringHelper.BytesToHexString(parameter.Modulus)
            });
        }

        [HttpPost]
        public JsonResult LoginDo(LoginDto dto)
        {
            var privateKey = DiMySession.Get<string>("PrivateKey");
            var password = HashHelper.RSADecrypt(dto.Password, privateKey);

            if (dto.UserName == USERNAME && password == PASSWORD)
            {
                var currentUser = CurrentContext.GetCurrentUser();
                currentUser.UserId = USERID;
                currentUser.UserName = USERNAME;
                currentUser.Source = OAuthSource.Local;
                CurrentContext.SetUser(currentUser);

                return Json(new LoginResult
                {
                    State = 1
                });
            }

            return Json(new LoginResult
            {
                State = 0
            });
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