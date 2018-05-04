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
        private readonly GDALController _GDAL;

        public ModisController(GDALController GDAL)
        {
            _GDAL = GDAL;
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

        public List<string> GetHDFFilePaths()
        {
            List<string> HDFFilePaths = Directory
                .GetFiles(Startup.Configuration["Modis:ModisPath"], "*.hdf", SearchOption.TopDirectoryOnly)
                .ToList();
            return HDFFilePaths;
        }

        public List<string> GetTifFilePaths()
        {
            List<string> TifFilePaths = Directory
                .GetFiles(Startup.Configuration["Modis:ModisPath"], "*.tif", SearchOption.TopDirectoryOnly)
                .ToList();
            return TifFilePaths;
        }

        public List<string> GetTifFileNames()
        {
            List<string> TifFileNames = Directory
                .GetFiles(Startup.Configuration["Modis:ModisPath"], "*.tif", SearchOption.TopDirectoryOnly)
                .ToList();
            for(int i=0;i< TifFileNames.Count();i++)
            {
                TifFileNames[i] = Path.GetFileName(TifFileNames[i]);
            }
            return TifFileNames;
        }

        public List<string> GetTifFileNamesOfHDF(string HDFFilePath)
        {
            List<string> TifFileNames = Directory
                .GetFiles(Startup.Configuration["Modis:ModisPath"], Path.GetFileNameWithoutExtension(HDFFilePath) + "*.tif", SearchOption.TopDirectoryOnly)
                .ToList();
            return TifFileNames;
        }

        public void ConvertHdfsToTifs(string CoordinateSystem)
        {
            List<string> HDFFilePaths = GetHDFFilePaths(),
                TifFilePaths = GetTifFilePaths(),
                TifFileNames = GetTifFileNames();

            foreach (string HDFFilePath in HDFFilePaths)
            {
                if(GetTifFileNamesOfHDF(HDFFilePath).Count()<12)
                {
                    _GDAL.HdfToGeoTIFF(HDFFilePath, CoordinateSystem);
                }
            }
        }

        public List<KeyValuePair<int, int>> GetHDFsYearsDaysOfYears()
        {
            List<KeyValuePair<int, int>> yearsdays = new List<KeyValuePair<int, int>>();
            List<string> HDFFilePaths = GetHDFFilePaths();
            foreach(string HDFFilePath in HDFFilePaths)
            {
                yearsdays.Add(new KeyValuePair<int, int>(GetYearByFilePath(HDFFilePath), GetDayOfYearByFilePath(HDFFilePath)));
            }
            yearsdays = yearsdays.Distinct().ToList();
            return yearsdays;
        }

        public string GetBandByFilePath(string FilePath)
        {
            string band = Path.GetFileNameWithoutExtension(FilePath);
            band = band.Substring(band.IndexOf('_') + 1, 2);
            return band;
        }

        public List<string> GetTifPathsByYearDayBand(int Year, int DayOfYear, string Band)
        {
            List<string> TifFilePaths = GetTifFilePaths(),
                TifPathsByYearDayBand = new List<string>();
            foreach(string TifFilePath in TifFilePaths)
            {
                if(GetYearByFilePath(TifFilePath)==Year
                    && GetDayOfYearByFilePath(TifFilePath)==DayOfYear
                    && GetBandByFilePath(TifFilePath) == Band)
                {
                    TifPathsByYearDayBand.Add(TifFilePath);
                }
            }
            return TifPathsByYearDayBand;
        }

        public void MergeTifs()
        {
            List<KeyValuePair<int, int>> yearsdays = GetHDFsYearsDaysOfYears();
            List<string> bands = new List<string>();
            for(int i=1;i<=12;i++)
            {
                string newband = i.ToString().Length  == 2 ? i.ToString() : "0" + i.ToString();
                bands.Add(newband);
            }
            foreach(KeyValuePair<int, int> yearday in yearsdays)
            {
                foreach(string band in bands)
                {
                    List<string> tifPathsToMerge = GetTifPathsByYearDayBand(yearday.Key, yearday.Value, band);
                    string year = yearday.Key.ToString(),
                        day = yearday.Value.ToString();
                    if(day.Length<3)
                    {
                        day = "0" + day;
                    }
                    if (day.Length < 3)
                    {
                        day = "0" + day;
                    }
                    _GDAL.MergeTifs(Path.Combine(Startup.Configuration["Modis:ModisPath"], $"MOD13Q1.A{year}{day}_{band}.tif"), tifPathsToMerge.ToArray());
                }
            }
        }
    }
}