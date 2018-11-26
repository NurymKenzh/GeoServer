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

        public const decimal N_v = 2.5M;
        public const decimal N_l = 2.5M;
        public const decimal N_o = 2.5M;
        //private const decimal N = 1.17M;
        public const decimal P_v = 70;
        public const decimal P_l = 90;
        public const decimal P_o = 90;

        public decimal W_v
        {
            get
            {
                return korm_v != 0 ? areaGa * N_v / korm_v : 0;
            }
        }
        public decimal W_l
        {
            get
            {
                return korm_l != 0 ? areaGa * N_l / korm_l : 0;
            }
        }
        public decimal W_o
        {
            get
            {
                return korm_o != 0 ? areaGa * N_o / korm_o : 0;
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
                return (ur_v * P_v + ur_l * P_l + ur_o * P_o) / (P_v + P_l + P_o);
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
