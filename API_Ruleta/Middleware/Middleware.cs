using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Ruleta.Middleware
{
    public class Middleware
    {
        private readonly RequestDelegate reqDeg_;

        public Middleware(RequestDelegate reqDeg)
        {
            reqDeg_ = reqDeg;
        }

        public Task Invoke(HttpContext context)
        {
            Environment.SetEnvironmentVariable("Username", "Deybidev");
            Environment.SetEnvironmentVariable("httpMethod", $"{context.Request.Method}");
            Environment.SetEnvironmentVariable("action", $"{context.Request.Method}");
            Environment.SetEnvironmentVariable("callSite", $"{context.Request.Host.Value}{context.Request.Path.Value}");

            return reqDeg_.Invoke(context);
        }
    }
}
