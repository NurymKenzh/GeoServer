using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoServer.Models
{
    public class ModisDataSet
    {
        public int Id { get; set; }

        public int ModisProductId { get; set; }
        public ModisProduct ModisProduct { get; set; }

        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Units { get; set; }
        public string DataType { get; set; }
        public string FillValue { get; set; }
        public string ValidRange { get; set; }
        public string ScalingFactor { get; set; }

        public string IndexName
        {
            get
            {
                return Index.ToString() + " - " + Name;
            }
        }
    }
}
