using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GeoServer.Controllers
{
    public class ModisController : Controller
    {
        public ModisController()
        {

        }

        //public string GetHDFFilePathByDate(int Year, int DayOfYear)
        //{
        //    string HDFFileName = $"MOD13Q1.A{Year.ToString()}{new string('0', 3 - DayOfYear.ToString().Length)}{DayOfYear.ToString()}";
        //    return HDFFileName;
        //}

        public int GetYearByFilePath(string FilePath)
        {
            string fileName = Path.GetFileName(FilePath);
            if(fileName.Contains("MOD13Q1.A"))
            {
                fileName = fileName.Remove(0, "MOD13Q1.A".Length);
            }
            if (fileName.Contains("MOD13Q1"))
            {
                fileName = fileName.Remove(0, "MOD13Q1".Length);
            }
            int year = Convert.ToInt32(fileName.Substring(0, 4));
            return year;
        }

        public int GetDayOfYearByFilePath(string FilePath)
        {
            string fileName = Path.GetFileName(FilePath);
            if(fileName.Contains("MOD13Q1.A"))
            {
                fileName = fileName.Remove(0, "MOD13Q1.A".Length);
            }
            if (fileName.Contains("MOD13Q1"))
            {
                fileName = fileName.Remove(0, "MOD13Q1".Length);
            }
            fileName = fileName.Remove(0, 4);
            int dayOfYear = Convert.ToInt32(fileName.Substring(0, 3));
            return dayOfYear;
        }
    }
}