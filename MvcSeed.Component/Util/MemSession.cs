using Memcached.ClientLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web;

namespace MvcSeed.Component.Util
{
    public class MemSession : ISession
    {
        private readonly int sessionHour;
        private readonly string cookieDomain;
        private readonly string preFix = string.Empty;
        readonly MemcachedClient mc;

        /// <summary>
        /// session构造函数
        /// </summary>
        /// <param name="strServer">服务器</param>
        /// <param name="sessionHour">session保存时间，单位为小时</param>
        /// <param name="cookieDomain">给cookie指定域名，用于跨域访问</param>
        public MemSession(string[] strServer, int sessionHour, string cookieDomain, string pre)
        {
            mc = new MemcachedClient();
            preFix = pre;
            this.sessionHour = sessionHour;
            this.cookieDomain = cookieDomain;
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(strServer);

            //初始化连接数
            pool.InitConnections = 3;
            //最小连接数
            pool.MinConnections = 3;
            //最大连接数
            pool.MaxConnections = 20;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();
        }

        public void Set<T>(string key, T value)
        {
            key = preFix + key;
            string sessionId = GetSessionId();
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = SetSessionId();
            }

            //TODO 这里高并发时，同一个sessionid的dict可能会被相互覆盖，有时间必须重写加锁
            object cachedItem = mc.Get(sessionId);
            if (cachedItem == null)
            {
                var dic = new Dictionary<string, object>();
                dic[key] = value;
                mc.Set(sessionId, dic, DateTime.Now.AddHours(sessionHour));
            }
            else
            {
                var dic = cachedItem as Dictionary<string, object>;
                if (dic != null)
                {
                    dic[key] = value;
                    mc.Set(sessionId, dic, DateTime.Now.AddHours(sessionHour));
                }
            }
        }

        //简单的超时管理，主要用于session缓存 设置间隔，避免重复set造成不必要的开销
        private ConcurrentDictionary<string, DateTime> keyTimeOut;
        private ConcurrentDictionary<string, DateTime> KeyTimeOut
        {
            get
            {
                if (this.keyTimeOut == null)
                {
                    this.keyTimeOut = new ConcurrentDictionary<string, DateTime>();
                }
                return this.keyTimeOut;
            }
            set
            {
                this.keyTimeOut = value;
            }
        }

        public T Get<T>(string key)
        {
            key = preFix + key;
            string sessionId = GetSessionId();
            T result = default(T);
            if (string.IsNullOrEmpty(sessionId))
            {
                return result; //浏览器cookie["sessionId"]不存在，返回null
            }

            object cachedItem = mc.Get(sessionId);
            if (cachedItem == null)
            {
                return result; //服务器上该session不存在
            }

            var dic = cachedItem as Dictionary<string, object>;

            if (dic != null && !dic.ContainsKey(key))
            {
                return result;
            }

            // 获取session对象后，延迟对象保存时间
            var timeOut = DateTime.Now.AddMinutes(15);

            //最近是否存储过session对象
            if (KeyTimeOut.Keys.Contains(sessionId))
            {
                //当前时间是否已经超过限制时间
                if (DateTime.Now > KeyTimeOut[sessionId])
                {
                    //超时 
                    //继续延长session时间,修改session保持的时间，保证缓存间隔15分钟以上
                    mc.Set(sessionId, dic, DateTime.Now.AddHours(sessionHour));
                    KeyTimeOut[sessionId] = timeOut;
                }
            }
            else
            {
                //最近未存储过session对象，记录session保存时间，保证缓存间隔15分钟以上
                KeyTimeOut.TryAdd(sessionId, timeOut);
                mc.Set(sessionId, dic, DateTime.Now.AddHours(sessionHour));
            }

            if (dic != null)
            {
                object value = dic[key];

                if (value is T)
                {
                    result = (T)value;
                }
            }

            return result;
        }

        public object this[string key]
        {
            get
            {
                return Get<object>(key);
            }
            set
            {
                Set(key, value);
            }
        }

        public void Clear(string sessionId = null)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = GetSessionId();
                //sessionId = null说明是删自己的session，要删除cookie
                DeleteCookie();
            }
            if (!string.IsNullOrEmpty(sessionId))
            {
                mc.Delete(sessionId);
            }
        }

        public string GetSessionId()
        {
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["SessionId"];
                if (cookie != null)
                {
                    return cookie.Value;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        #region 内部方法

        /// <summary>
        /// 第一次给session付值时创建
        /// </summary>
        /// <returns></returns>
        private string SetSessionId()
        {
            try
            {
                var cookie = new HttpCookie("SessionId");
                if (cookieDomain.ToLower() != "localhost")
                {
                    cookie.Domain = cookieDomain; //当要跨域名访问的时候,给cookie指定域名即可,格式为xxx.com
                }
                //cookie.Expires = DateTime.MaxValue; //关闭浏览器session失效
                cookie.Value = preFix + "_" + Guid.NewGuid();
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                return cookie.Value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 退出后清除Cookie保存的sessionId
        /// </summary>      
        /// <returns></returns>
        private bool DeleteCookie()
        {
            try
            {
                var cookie = new HttpCookie("SessionId") { Expires = DateTime.Now.AddDays(-1) };
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                System.Web.HttpContext.Current.Request.Cookies.Remove("SessionId");
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
