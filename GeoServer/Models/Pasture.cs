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

        //public const decimal N_v = 1.5M; // 2.5 кг * 60 дней / 100 = ц
        //public const decimal N_l = 2.25M; // 2.5 кг * 90 дней / 100 = ц
        //public const decimal N_o = 2.25M; // 2.5 кг * 90 дней / 100 = ц
        //private const decimal N = 1.17M;
        public const decimal P_v = 1.5M; // 2.5 кг * 60 дней / 100 = ц
        public const decimal P_l = 2.25M; // 2.5 кг * 90 дней / 100 = ц
        public const decimal P_o = 2.25M; // 2.5 кг * 90 дней / 100 = ц

        public decimal W_v
        {
            get
            {
                return korm_v != 0 ? areaGa * 0.7M * P_v / (ur_v * areaGa * 0.7M) : 0;
            }
        }
        public decimal W_l
        {
            get
            {
                return korm_l != 0 ? areaGa * 0.7M * P_l / (ur_l * areaGa * 0.7M) : 0;
            }
        }
        public decimal W_o
        {
            get
            {
                return korm_o != 0 ? areaGa * 0.7M * P_o / (ur_o * areaGa * 0.7M) : 0;
            }
        }

        // средняя скотоемкость
        public decimal E
        {
            get
            {
                return areaGa * 0.7M / (W_v + W_l + W_o);
            }
        }

        //// скотоемкость
        //public decimal Y
        //{
        //    get
        //    {
        //        return (ur_v * P_v + ur_l * P_l + ur_o * P_o) / (P_v + P_l + P_o);
        //    }
        //}

        //// скотоемкость
        //public decimal EY
        //{
        //    get
        //    {
        //        return Y * areaGa * 0.7M / ((P_v + P_l +P_o) / 3);
        //    }
        //}

        //// скотоемкость КРС
        //public decimal EY_KRS
        //{
        //    get
        //    {
        //        return EY / 5;
        //    }
        //}
        //// скотоемкость лошади
        //public decimal EY_horses
        //{
        //    get
        //    {
        //        return EY / 6;
        //    }
        //}
        //// скотоемкость верблюды
        //public decimal EY_camels
        //{
        //    get
        //    {
        //        return EY / 7;
        //    }
        //}
    }
}
