using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoServer.Models
{
    public class ModisProduct
    {
        public int Id { get; set; }
        public int ModisSourceId { get; set; }
        public ModisSource ModisSource { get; set; }
        public string Name { get; set; }
    }
}
