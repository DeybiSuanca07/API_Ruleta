using Amazon;
using Amazon.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.AWS.Logger;
using NLog.Common;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Ruleta
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<Middleware.Middleware>();
            ConfigureLogger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureLogger()
        {
            var config = new LoggingConfiguration();
            var awsTarget = CreateAwsTarget(logGroup:Configuration["InformationAWS:LogGroup"], region: Configuration["InformationAWS:Region"]);

            config.AddTarget(name: "AWSTarget", awsTarget);
            config.AddRuleForAllLevels(awsTarget);

            InternalLogger.LogFile = Configuration["Logging:InternalLog"];
            LogManager.Configuration = config;
        }

        private AWSTarget CreateAwsTarget(string logGroup, string region)
        {
            var target = new AWSTarget();
            target.Credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AccessKeyId"), Environment.GetEnvironmentVariable("SecretKeyId"));
            target.LogGroup = logGroup;
            target.Region = region;

            var format = "{" +
                            "Id=" + "${event-properties:item=idlog}" + '\'' +
                            ", LogTimeStamp='" + "${longdate}" + '\'' +
                            ", MachineName='" + "${aspnet-request-ip}" + '\'' +
                            ", Level='" + "${level}" + '\'' +
                            ", Message='" + "${message}" +
                            ", Exception='" + "${stacktrace}" +
                            ", Payload='" + "${when:when='${aspnet-request-method}' == 'GET':inner='${aspnet-request-querystring}':else='${aspnet-request-posted-body}'}" + '\'' +
                            ", CallSite='" + "${aspnet-request-url:IncludePort=true:IncludeQueryString=true}" + '\'' +
                            ", Action='" + "${aspnet-request-method}" + '\'' +
                            ", Username='" + "${aspnet-sessionid}" + '\'' +
                            ", MethodName='" + "${event-properties:item=methodName}" + '\'' +
                            ", ApplicationName='" + "Test" +
                         "}";
            target.LogStreamNameSuffix = "Demo";
            target.LogStreamNamePrefix = "Logger";
            target.Layout = format;
            return target;
        }
    }
}
