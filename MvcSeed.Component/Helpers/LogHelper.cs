using log4net;
using System;

namespace MvcSeed.Component.Helpers
{
    public class LogHelper
    {
        private const string LOG_NAME = "Loggering";

        public static void Error(string msg)
        {
            var log = LogManager.GetLogger(LOG_NAME);
            log.Error(msg);
        }

        public static void Error(string msg, Exception ex)
        {
            var log = LogManager.GetLogger(LOG_NAME);
            log.Error(msg, ex);
        }

        public static void Error(Exception ex)
        {
            var log = LogManager.GetLogger(LOG_NAME);
            log.Error(ex);
        }
    }
}
