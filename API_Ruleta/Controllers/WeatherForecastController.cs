using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using API_Ruleta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API_Ruleta.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : GeneralMethods
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration config_;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config)
        {
            _logger = logger;
            config_ = config;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            string accessKeyId = Environment.GetEnvironmentVariable("AccessKeyId");
            string secretAccessKey = Environment.GetEnvironmentVariable("SecretKeyId");
            string secretName = "probandoSecret/Secrets/db";
            var client = new AmazonSecretsManagerClient(accessKeyId, secretAccessKey, RegionEndpoint.USEast1);

            var req = new GetSecretValueRequest
            {
                SecretId = secretName
            };

            GetSecretValueResponse response = null;
            try
            {
                response = client.GetSecretValueAsync(req).Result;
            }
            catch (DecryptionFailureException e)
            {

                throw;
            }

            var id = Guid.NewGuid();

            try
            {
                var t = 10;
                var t1 = 0;
                var t2 = t / t1;
            }
            catch (Exception ex)
            {
                RegisterLogFatal(ex, id);
                //response.Message = $"Ocurrio un error {id}";
                //response.Status = false;
                //response.Object = null;
            }

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

        
        }
    }
}
