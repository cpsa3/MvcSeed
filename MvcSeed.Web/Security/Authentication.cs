using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSeed.Web.Security
{
    public class Authentication : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 是否打开鉴权功能
        /// </summary>
        private readonly bool _openAuth;

        private readonly Level[] _level;

        private readonly FuncAuthority[] _authority;

        public Authentication()
        {
            _openAuth = true;
        }

        public Authentication(bool openAuth)
        {
            _openAuth = openAuth;
        }

        public Authentication(Level[] level, FuncAuthority[] authority)
        {
            _level = level;
            _authority = authority;
            _openAuth = true;
        }

        public Authentication(FuncAuthority[] authority)
        {
            _openAuth = true;
            _level = null;
            _authority = authority;
        }

        public Authentication(Level[] level)
        {
            _openAuth = true;
            _level = level;
            _authority = null;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!_openAuth)
            {
                return;
            }

            var user = CurrentContext.GetCurrentUser();

            if (user.UserId == 0)
            {
                filterContext.Result = new RedirectResult("~/Home/Login");
                return;
            }
        }

    }
}