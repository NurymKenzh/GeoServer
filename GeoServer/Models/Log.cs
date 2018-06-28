using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoServer.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime DateTime { get; set; }
        public string Function { get; set; }
        public string Operation { get; set; }
        public string Parameters { get; set; }
    }
}
