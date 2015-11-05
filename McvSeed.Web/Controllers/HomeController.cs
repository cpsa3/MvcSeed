using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using McvSeed.Web.Models;
using McvSeed.Web.Security;

namespace McvSeed.Web.Controllers
{
    public class HomeController : Controller
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
        public ActionResult LoginDo(LoginDto dto)
        {
            if (dto.UserName == USERNAME && dto.Password == PASSWORD)
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