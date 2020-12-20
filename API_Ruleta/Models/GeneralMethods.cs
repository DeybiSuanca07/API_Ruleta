using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Ruleta.Models
{
    public class GeneralMethods
    {
        private static Logger logger_;

        public GeneralMethods()
        {
            logger_ = LogManager.GetCurrentClassLogger();
        }

        public void RegisterLogFatal(Exception ex, Guid id)
        {
            var log = new LogEventInfo(LogLevel.Fatal, loggerName: "", message: "")
            {
                Message = ex.Message,
                Exception = ex
            };
            log.Properties.Add("idLog", id.ToString());
            log.Properties.Add("methodName", $"{ex.TargetSite.DeclaringType?.FullName}.{ex.TargetSite.Name}");
            logger_.Fatal(log);
        }
    }
}
