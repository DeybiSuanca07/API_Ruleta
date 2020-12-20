using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Ruleta.Models
{
    public class Response
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public object Object { get; set; }
    }
}
