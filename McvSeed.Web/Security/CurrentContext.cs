using Microsoft.Practices.Unity;
using MvcSeed.Business.Util;
using MvcSeed.Component.Util;

namespace MvcSeed.Web.Security
{
    public class CurrentContext
    {
        public static void SetUser(CurrentUser user)
        {
            var mySession = Bootstrapper.Instance.UnityContainer.Resolve<ISession>();
            mySession.Set("Current_User", user);
        }

        public static CurrentUser GetCurrentUser()
        {

            var mySession = Bootstrapper.Instance.UnityContainer.Resolve<ISession>();
            var user = mySession["Current_User"] as CurrentUser;
            if (user == null)
            {
                user = new CurrentUser();
                mySession.Set("Current_User", user);
            }
            return user;
        }

        public static void ClearInfo()
        {
            var mySession = Bootstrapper.Instance.UnityContainer.Resolve<ISession>();
            var user = mySession.Get<CurrentUser>("Current_User");
            if (user != null)
            {
                user.UserId = 0;
                user.UserName = string.Empty;
                mySession.Set("Current_User", user);
            }
        }
    }
}