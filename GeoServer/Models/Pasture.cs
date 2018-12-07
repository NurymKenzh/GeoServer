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
                if(ur_v * areaGa * 0.7M!=0)
                {
                    return korm_v != 0 ? areaGa * 0.7M * P_v / (ur_v * areaGa * 0.7M) : 0;
                }
                return 0;
            }
        }
        public decimal W_l
        {
            get
            {
                if (ur_l * areaGa * 0.7M != 0)
                {
                    return korm_l != 0 ? areaGa * 0.7M * P_l / (ur_l * areaGa * 0.7M) : 0;
                }
                return 0;
            }
        }
        public decimal W_o
        {
            get
            {
                if (ur_o * areaGa * 0.7M != 0)
                {
                    return korm_o != 0 ? areaGa * 0.7M * P_o / (ur_o * areaGa * 0.7M) : 0;
                }
                return 0;
            }
        }

        // средняя скотоемкость
        public decimal E
        {
            get
            {
                if(W_v + W_l + W_o!=0)
                {
                    return areaGa * 0.6M / (W_v + W_l + W_o);
                }
                else
                {
                    return 0;
                }
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

        // скотоемкость КРС
        public decimal E_KRS
        {
            get
            {
                return E / 5;
            }
        }
        // скотоемкость лошади
        public decimal E_horses
        {
            get
            {
                return E / 6;
            }
        }
        // скотоемкость верблюды
        public decimal E_camels
        {
            get
            {
                return E / 7;
            }
        }
    }
}
