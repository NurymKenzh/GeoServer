using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoServer.Models
{
    public class Pasture
    {
        public int Id { get; set; }
        public int pid { get; set; }
        public int class_id { get; set; }
        public int otdely_id{ get; set; }
        public int subtype_id { get; set; }
        public int group_id { get; set; }
        public int group_lt { get; set; }
        public decimal ur_v { get; set; }
        public decimal ur_l { get; set; }
        public decimal ur_o { get; set; }
        public decimal ur_z { get; set; }
        public decimal korm_v { get; set; }
        public decimal korm_l { get; set; }
        public decimal korm_o { get; set; }
        public decimal korm_z { get; set; }
        public int recom_id { get; set; }
        public string note { get; set; }
        public decimal areaGa { get; set; }

        private const decimal N_v = 1.17M;
        private const decimal N_l = 1.17M;
        private const decimal N_o = 1.17M;
        private const int P = 625;

        public decimal W_v
        {
            get
            {
                return areaGa * N_v / korm_v;
            }
        }
        public decimal W_l
        {
            get
            {
                return areaGa * N_l / korm_l;
            }
        }
        public decimal W_o
        {
            get
            {
                return areaGa * N_o / korm_o;
            }
        }

        // средняя скотоемкость
        public decimal E
        {
            get
            {
                return areaGa / (W_v + W_l + W_o);
            }
        }

        // скотоемкость
        public decimal Y
        {
            get
            {
                return (ur_v * P / 3 + ur_l * P / 3 + ur_o * P / 3) / P;
            }
        }

        // скотоемкость
        public decimal EY
        {
            get
            {
                return Y * areaGa / ((N_v + N_l +N_o) / 3);
            }
        }
    }
}
