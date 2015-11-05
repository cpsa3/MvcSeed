using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using McvSeed.Web.Security;
using Microsoft.Practices.Unity;
using MvcSeed.Component.Data;
using MvcSeed.Component.Util;

namespace McvSeed.Web.Controllers
{
    public class BaseController : Controller
    {
        [Dependency]
        public ISession DiMySession { get; set; }

        [Dependency]
        public ICache DiCache { get; set; }

        [Dependency("AppCache")]
        public ICache DiAppCache { get; set; }

        public CurrentUser CurrentUser { get; set; }

        [Dependency("WriteUnitOfWork")]
        public IUnitOfWork UnitOfWork { get; set; }

        [Dependency("ReadUnitOfWork")]
        public IUnitOfWork ReadUnitOfWork { get; set; }

        public BaseController()
        {
            CurrentUser = CurrentContext.GetCurrentUser();
        }
	}
}