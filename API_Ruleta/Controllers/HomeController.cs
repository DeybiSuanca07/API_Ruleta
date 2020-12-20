using API_Ruleta.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Ruleta.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController : GeneralMethods
    {
        private Response response;
        public HomeController()
        {
            response = new Response();
        }

        [HttpGet]
        public ActionResult<Response> Prueba()
        {

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
                response.Message = $"Ocurrio un error {id}";
                response.Status = false;
                response.Object = null;
            }
            return response;
        }
    }
}
