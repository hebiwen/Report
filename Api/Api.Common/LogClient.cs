using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Common
{
    public class LogClient
    {
        public static readonly log4net.ILog logInfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logError = log4net.LogManager.GetLogger("logerror");

        public static void WriteLog(string message)
        {
            if (logInfo.IsInfoEnabled) {
                logInfo.Info(message);
            }
        }

        public static void WriteLog(string message, Exception ex)
        {
            if (logError.IsErrorEnabled) logError.Error(message, ex);
        }

    }
}
