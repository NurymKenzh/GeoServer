using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoServer.Models
{
    public class ZonalStatPast
    {
        public int Id { get; set; }
        public string PastId { get; set; }
        public int Year { get; set; }
        public int DayOfYear { get; set; }
        public string ModisSource { get; set; }
        public string ModisProduct { get; set; }
        public string DataSet { get; set; }
        public decimal Value { get; set; }
    }

    public class ZonalStatPastIndexPageViewModel
    {
        public IEnumerable<ZonalStatPast> Items { get; set; }
        public Pager Pager { get; set; }
    }
}
