using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Data;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GeoServer.Controllers
{
    public class GDALController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        public GDALController(IHostingEnvironment hostingEnvironment,
            ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        private string PythonExecuteWithParameters(params string[] Arguments)
        {
            Process process = new Process();
            try
            {
                Arguments[0] = Path.GetFullPath(
                    Path.ChangeExtension(
                        Path.Combine(
                            _hostingEnvironment.ContentRootPath,
                            Path.Combine("Python", Arguments[0])),
                        "py")
                    );

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = Startup.Configuration["GDAL:PythonFullPath"];
                process.StartInfo.Arguments = string.Join(' ', Arguments);
                process.Start();
                string pyhonOutput = process.StandardOutput.ReadToEnd();
                string pyhonError = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(pyhonError))
                {
                    throw new Exception(pyhonError);
                }
                else
                {
                    return pyhonOutput;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        /// <summary>
        /// Запуск py-файла
        /// </summary>
        /// <param name="Arguments">
        /// Первый элемент массива - название py-файла без пути и расширения.
        /// Остальные элементы массива - параметры для передачи в Python
        /// </param>
        /// <returns>
        /// Возвращает текс с Python
        /// </returns>
        private string PythonExecute(params string[] Arguments)
        {
            Process process = new Process();
            try
            {
                Arguments[0] = Path.GetFullPath(
                    Path.ChangeExtension(
                        Path.Combine(
                            _hostingEnvironment.ContentRootPath,
                            Path.Combine("Python", Arguments[0])),
                        "py")
                    );

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput= true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = Startup.Configuration["GDAL:PythonFullPath"];
                process.StartInfo.Arguments = Arguments[0];
                process.Start();
                for(int i = 1; i < Arguments.Count(); i++)
                {
                    process.StandardInput.WriteLine(Arguments[i]);
                }
                string pyhonOutput = process.StandardOutput.ReadToEnd();
                string pyhonError = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(pyhonError))
                {
                    throw new Exception(pyhonError);
                }
                else
                {
                    return pyhonOutput;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        /// <summary>
        /// Запуск py-файла
        /// </summary>
        /// <param name="Arguments">
        /// Первый элемент массива - название py-файла без пути и расширения.
        /// Остальные элементы массива - параметры для передачи в Python
        /// </param>
        /// <returns>
        /// Возвращает текс с Python
        /// </returns>
        public void PythonExecuteWithParameters(string FileName, params string[] Parameters)
        {
            Process process = new Process();
            try
            {
                //FileName = Path.GetFullPath(
                //    Path.ChangeExtension(
                //        Path.Combine(
                //            _hostingEnvironment.ContentRootPath,
                //            Path.Combine("Python", FileName)),
                //        "py")
                //    );

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = Startup.Configuration["GDAL:CmdFullPath"];
                //process.StartInfo.Arguments = FileName + " " + string.Join(" ", Parameters);
                process.Start();
                //for (int i = 1; i < Arguments.Count(); i++)
                //{
                //    process.StandardInput.WriteLine(Arguments[i]);
                //}

                process.StandardInput.WriteLine("cd \\");
                process.StandardInput.WriteLine("cd Python27\\Scripts");

                process.StandardInput.WriteLine(FileName + " " + string.Join(" ", Parameters));
                string pyhonOutput = process.StandardOutput.ReadToEnd();
                string pyhonError = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(pyhonError))
                {
                    throw new Exception(pyhonError);
                }
                else
                {
                    //return pyhonOutput;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        private string QGISPythonExecute(params string[] Arguments)
        {
            Process process = new Process();
            try
            {
                Arguments[0] = Path.GetFullPath(
                    Path.ChangeExtension(
                        Path.Combine(
                            _hostingEnvironment.ContentRootPath,
                            Path.Combine("Python", Arguments[0])),
                        "py")
                    );

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardError = true;

                string[] arguments = new string[3];
                arguments[0] = Startup.Configuration["GDAL:QGISPythonO4wenvBatFullPath"];
                arguments[1] = Startup.Configuration["GDAL:QGISPythonFullPath"];
                arguments[2] = Path.GetFullPath(
                    Path.ChangeExtension(
                        Path.Combine(
                            _hostingEnvironment.ContentRootPath,
                            Path.Combine("Python", Arguments[0])),
                        "py")
                    );
                process.StartInfo.FileName = Startup.Configuration["GDAL:CmdFullPath"];
                process.StartInfo.CreateNoWindow = false;
                process.Start();
                process.StandardInput.WriteLine($"\"{arguments[0]}\"");
                process.StandardInput.WriteLine($"\"{arguments[1]}\" \"{arguments[2]}\"");
                for(int i=1;i< Arguments.Count();i++)
                {
                    process.StandardInput.WriteLine(Arguments[i]);
                }
                process.StandardInput.Flush();
                process.StandardInput.Close();
                string pyhonOutput = process.StandardOutput.ReadToEnd();
                string pyhonError = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(pyhonError))
                {
                    throw new Exception(pyhonError);
                }
                else
                {
                    return pyhonOutput;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        private string QGISShellExecute(params string[] Arguments)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardError = true;

                string[] arguments = new string[3];
                arguments[0] = Startup.Configuration["GDAL:QGISPythonO4wenvBatFullPath"];
                arguments[1] = Startup.Configuration["GDAL:QGISPythonFullPath"];
                arguments[2] = Path.GetFullPath(
                    Path.ChangeExtension(
                        Path.Combine(
                            _hostingEnvironment.ContentRootPath,
                            Path.Combine("Python", Arguments[0])),
                        "py")
                    );
                process.StartInfo.FileName = Startup.Configuration["GDAL:CmdFullPath"];
                process.StartInfo.CreateNoWindow = false;
                process.Start();
                process.StandardInput.WriteLine($"cd {Path.GetDirectoryName(Startup.Configuration["GDAL:QGISPythonO4wenvBatFullPath"])}");
                process.StandardInput.WriteLine(Path.GetFileName(Startup.Configuration["GDAL:QGISPythonO4wenvBatFullPath"]));
                process.StandardInput.WriteLine($"{Path.GetFileName(Startup.Configuration["GDAL:QGISPythonFullPath"])} {string.Join(' ', Arguments)}");
                process.StandardInput.Flush();
                process.StandardInput.Close();
                string pyhonOutput = process.StandardOutput.ReadToEnd();
                string pyhonError = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(pyhonError))
                {
                    throw new Exception(pyhonError);
                }
                else
                {
                    return pyhonOutput;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        private string GDALShellExecute(params string[] Arguments)
        {
            Process process = new Process();
            try
            {
                string startInfoFileName = Arguments[0];
                string[] arguments = new string[Arguments.Count() - 1];
                Array.Copy(Arguments, 1, arguments, 0, arguments.Count());

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = startInfoFileName;
                process.StartInfo.Arguments = string.Join(' ', arguments);
                process.Start();
                string shellOutput = process.StandardOutput.ReadToEnd();
                string shellError = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (!string.IsNullOrEmpty(shellError))
                {
                    throw new Exception(shellError);
                }
                else
                {
                    return shellOutput;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public void Test1()
        {
            Thread.Sleep(60000);
            Console.WriteLine("Fire-and-forget!");
        }

        public void Test2()
        {
            Thread.Sleep(60000);
            Console.WriteLine("Delayed!");
        }

        public void LogTask(string Email,
            DateTime DateTime,
            string Function,
            string Operation,
            string Parameters)
        {
            Models.Log logs = new Models.Log
            {
                Email = Email,
                DateTime = DateTime,
                Function = Function,
                Operation = Operation,
                Parameters = Parameters
            };

            //_context.Log.Add(logs);
            //_context.SaveChanges();

            using (var context = new MyContext())
            {
                context.Log.Add(logs);
                context.SaveChanges();
            }

        }

        private void ModisDownload(string[] ModisSpan,
            string ModisSource,
            string ModisProduct,
            DateTime DateStart,
            DateTime DateFinish)
        {
            try
            {
                string userName = User.Identity.Name.ToString();
                //StringBuilder sb = new StringBuilder();
                //for (int i = 0; i < ModisSpan.Length; i++)
                //{
                //    sb.AppendFormat("{0} ", ModisSpan[i]);
                //    if (i < ModisSpan.Length - 1)
                //    {
                //        sb.AppendLine();
                //    }
                //}
                //string modisSpan = sb.ToString().Replace(",", "");

                //LogTask(userName, DateTime.Now.ToLocalTime(), MethodBase.GetCurrentMethod().Name, "start",
                //    "ModisSourse = " + ModisSource + ", ModisProduct = " + ModisProduct + ", ModisSpan = " + modisSpan + ", DateStart = " + DateStart  + ", DateFinish = " + DateFinish);
                LogTask(userName, DateTime.Now.ToLocalTime(), MethodBase.GetCurrentMethod().Name, "start",
                    $"ModisSourse = \"{ModisSource}\", ModisProduct = \"{ModisProduct}\", ModisSpan = \"{string.Join(", ", ModisSpan)}\", DateStart = \"{DateStart.ToShortDateString()}\", DateFinish = \"{DateFinish.ToShortDateString()}\"");

                string folder = Path.Combine(Startup.Configuration["Modis:ModisPath"], ModisSource, ModisProduct);
                Directory.CreateDirectory(folder);
                //var jobId = BackgroundJob.Schedule(
                //    () => PythonExecuteWithParameters("modis_download.py", $"-r -s {ModisSource} -p {ModisProduct} -t {string.Join(',', ModisSpan)} -f {DateStart.Year}-{DateStart.Month}-{DateStart.Day} -e {DateFinish.Year}-{DateFinish.Month}-{DateFinish.Day} {folder}"),
                //    TimeSpan.FromMilliseconds(1000));

                //var jobId1 = BackgroundJob.Enqueue(
                //    () => Test1());

                //var jobId2 = BackgroundJob.Schedule(
                //    () => Test2(),
                //    TimeSpan.FromMilliseconds(1000));

                PythonExecuteWithParameters("modis_download.py", $"-r -s {ModisSource} -p {ModisProduct}.006 -t {string.Join(',', ModisSpan)} -f {DateStart.Year}-{DateStart.Month}-{DateStart.Day} -e {DateFinish.Year}-{DateFinish.Month}-{DateFinish.Day} {folder}");
                //Thread.Sleep(5000);
                //LogTask(userName, DateTime.Now.ToLocalTime(), MethodBase.GetCurrentMethod().Name, "finish",
                //    "ModisSourse = " + ModisSource + ", ModisProduct = " + ModisProduct + ", ModisSpan = " + modisSpan + ", DateStart = " + DateStart + ", DateFinish = " + DateFinish);
                LogTask(userName, DateTime.Now.ToLocalTime(), MethodBase.GetCurrentMethod().Name, "finish",
                    $"ModisSourse = \"{ModisSource}\", ModisProduct = \"{ModisProduct}\", ModisSpan = \"{string.Join(", ", ModisSpan)}\", DateStart = \"{DateStart.ToShortDateString()}\", DateFinish = \"{DateFinish.ToShortDateString()}\"");
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        private void MosaicModis(string ModisSource,
            string ModisProduct,
            int ModisDataSet,
            string File,
            string FileName)
        {                     
            int a = ModisDataSet;
            string[] s = new string[a + 1];

            for (int i = 0; i < s.Length; i++)
            {
                    for (int j = 0; j < ModisDataSet; j++)
                    {

                        if (i == ModisDataSet)
                        {
                            s[i] = "1";
                            break;
                        }
                        else
                        {
                            if (j == ModisDataSet - 1)
                            {
                                s[i] = "0";
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                    }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                sb.AppendFormat("{0} ", s[i]);
                if (i < s.Length - 1)
                {
                    sb.AppendLine();
                }
            }

            string folder = Path.Combine(Startup.Configuration["Modis:ModisPath"], ModisSource, ModisProduct),
                batfile = Path.Combine(folder, "bat.bat"),
                indexes = sb.ToString().Replace("\r\n", "");


            using (var sw = new StreamWriter(batfile))
            {
                //sw.WriteLine($"modis_mosaic.py -o {FileName}.tif -s \"{indexes}\"  {folder}\\{File}");
                sw.WriteLine($"modis_mosaic.py -s \"{indexes}\" -o {FileName} -v {folder}\\{File}");
            }

            Process process = new Process();
            try
            {
                process.StartInfo.WorkingDirectory = folder;
                process.StartInfo.FileName = batfile;
                process.Start();
                process.WaitForExit();
                System.IO.File.Delete(batfile);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        private static string CleanFileName(string fileName)
        {
            //return Path.GetInvalidPathChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
            char[] invalid = new char[1] { ':' };
            return String.Join("", fileName.Split(invalid));
        }

        public void ReprojectVrt(string ModisSource,
            string ModisProduct,
            string File,
            string CoordinateSystem)
        {
            string folder = Path.Combine(Startup.Configuration["Modis:ModisPath"], ModisSource, ModisProduct),
               batfile = Path.Combine(folder, "bat.bat");
            CoordinateSystem = CoordinateSystem.Remove(0, CoordinateSystem.IndexOf(":") + 1);
            string dataset = "( 1 )";

            using (var sw = new StreamWriter(batfile))
            {
                //sw.WriteLine($"modis_mosaic.py -o {FileName}.tif -s \"{indexes}\"  {folder}\\{File}");
                sw.WriteLine($"modis_convert.py -v -s \"{dataset}\" -o OUTPUT_EVI -e {CoordinateSystem} {folder}\\{File}");
            }

             Process process = new Process();
            try
            {
                process.StartInfo.WorkingDirectory = folder;
                process.StartInfo.FileName = batfile;
                process.Start();
                process.WaitForExit();
                System.IO.File.Delete(batfile);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }

            //string folder = Path.Combine(Startup.Configuration["Modis:ModisPath"], ModisSource, ModisProduct),
            //foldernew = Path.Combine(folder, CleanFileName(CoordinateSystem)),
            //filepath = Path.Combine(folder, File),
            //filepathnew = Path.Combine(foldernew, File);

            //try
            //{
            //    Directory.CreateDirectory(foldernew);
            //    SaveLayerWithNewCoordinateSystem(filepath, filepathnew, CoordinateSystem);
            //}
            //catch (Exception exception)
            //{
            //    throw new Exception(exception.ToString(), exception.InnerException);
            //}
        }

        public int GetRasterBandsCount(string FilePath)
        {
            try
            {
                return Convert.ToInt32(PythonExecute("GetRasterBandsCount", FilePath));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public string GetLayerCoordinateSystemName(string FilePath)
        {
            try
            {
                return PythonExecute("GetLayerCoordinateSystemName", FilePath).Trim();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public void SaveLayerWithNewCoordinateSystem(string FilePathFrom, string FilePathTo, string CoordinateSystem)
        {
            try
            {
                GDALShellExecute(Startup.Configuration["GDAL:gdalwarpFullPath"], FilePathFrom, FilePathTo, "-t_srs " + CoordinateSystem);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public int QGISGetRasterBandsCount(string FilePath)
        {
            try
            {
                return Convert.ToInt32(QGISPythonExecute("QGISGdalTest", FilePath));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public void HdfToGeoTIFF(string FilePath, string CoordinateSystem)
        {
            try
            {
                //GDALShellExecute(Startup.Configuration["GDAL:gdaltranslateFullPath"], $"-of GTiff -projwin_srs \"+init={CoordinateSystem.ToLower()}\" -sds {FilePath} {Path.ChangeExtension(FilePath, "tif")}");
                GDALShellExecute(Startup.Configuration["GDAL:gdaltranslateFullPath"], $"-of GTiff -sds {FilePath} {Path.ChangeExtension(FilePath, "tif")}");
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public void MergeTifs(string MergedFilePath, params string[] FilesToMerge)
        {
            try
            {
                FilesToMerge = Array.ConvertAll(FilesToMerge, f => $"\"{f}\"");
                QGISShellExecute("gdal_merge.py", "-o", MergedFilePath, string.Join(' ', FilesToMerge));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        //===========================================================================================================

        public IActionResult DownloadModis()
        {
            ViewBag.ModisSpan = new MultiSelectList(_context.ModisSpan.OrderBy(m => m.Name), "Name", "Name", _context.ModisSpan.Select(m => m.Name));
            ViewBag.ModisSource = new SelectList(_context.ModisSource.OrderBy(m => m.Name), "Name", "Name");
            ViewBag.ModisProduct = new SelectList(_context.ModisProduct.Where(m => m.ModisSourceId == _context.ModisSource.OrderBy(ms => ms.Name).FirstOrDefault().Id).OrderBy(m => m.Name), "Name", "Name");
            ViewBag.DateStart = DateTime.Now;
            ViewBag.DateFinish = DateTime.Now;
            return View();
        }

        [HttpPost]
        public IActionResult DownloadModis(string[] ModisSpan,
            string ModisSource,
            string ModisProduct,
            DateTime DateStart,
            DateTime DateFinish)
        {
            //ModisDownload(ModisSpan, ModisSource, ModisProduct, DateStart, DateFinish);
            Task t = new Task(() => { ModisDownload(ModisSpan, ModisSource, ModisProduct, DateStart, DateFinish); });
            t.Start();
            ViewBag.Message = "Operation started!";
            ViewBag.ModisSpan = new MultiSelectList( _context.ModisSpan.OrderBy(m => m.Name), "Name", "Name", ModisSpan);
            ViewBag.ModisSource = new SelectList(_context.ModisSource.OrderBy(m => m.Name), "Name", "Name", ModisSource);
            ViewBag.ModisProduct = new SelectList(_context.ModisProduct.Include(m => m.ModisSource).Where(m => m.ModisSource.Name == ModisSource).OrderBy(m => m.Name), "Name", "Name", ModisProduct);
            ViewBag.DateStart = DateStart;
            ViewBag.DateFinish = DateFinish;
            return View();
        }

        public IActionResult DownloadModisDelta()
        {
            ViewBag.ModisSpan = new MultiSelectList(_context.ModisSpan.OrderBy(m => m.Name), "Name", "Name", _context.ModisSpan.Select(m => m.Name));
            ViewBag.ModisSource = new SelectList(_context.ModisSource.OrderBy(m => m.Name), "Name", "Name");
            ViewBag.ModisProduct = new SelectList(_context.ModisProduct.Where(m => m.ModisSourceId == _context.ModisSource.OrderBy(ms => ms.Name).FirstOrDefault().Id).OrderBy(m => m.Name), "Name", "Name");
            ViewBag.Delta = 1;
            ViewBag.DateFinish = DateTime.Now;
            return View();
        }

        [HttpPost]
        public IActionResult DownloadModisDelta(string[] ModisSpan,
            string ModisSource,
            string ModisProduct,
            int Delta,
            DateTime DateFinish)
        {

            DateTime DateStart = DateFinish.AddDays(-Delta);
            Task t = new Task(() => { ModisDownload(ModisSpan, ModisSource, ModisProduct, DateStart, DateFinish); });
            t.Start();
            ViewBag.Message = "Operation started!";
            ViewBag.ModisSpan = new MultiSelectList(_context.ModisSpan.OrderBy(m => m.Name), "Name", "Name", ModisSpan);
            ViewBag.ModisSource = new SelectList(_context.ModisSource.OrderBy(m => m.Name), "Name", "Name", ModisSource);
            ViewBag.ModisProduct = new SelectList(_context.ModisProduct.Include(m => m.ModisSource).Where(m => m.ModisSource.Name == ModisSource).OrderBy(m => m.Name), "Name", "Name", ModisProduct);
            ViewBag.Delta = Delta;
            ViewBag.DateFinish = DateFinish;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult GetModisProductByModisSource(string ModisSource)
        {
            var modisProducts = _context.ModisProduct
                .Where(m => m.ModisSource.Name == ModisSource);
            JsonResult result = new JsonResult(modisProducts);
            return result;
        }

        public IActionResult ModisMosaic()
        {
            var modisSources = _context.ModisSource.OrderBy(m => m.Name);
            ViewBag.ModisSource = new SelectList(modisSources, "Name", "Name");
            var modisProducts = _context.ModisProduct.Where(m => m.ModisSourceId == _context.ModisSource.OrderBy(ms => ms.Name).FirstOrDefault().Id).OrderBy(m => m.Name);
            ViewBag.ModisProduct = new SelectList(modisProducts, "Name", "Name");
            ViewBag.File = new SelectList(GetModisListFiles(modisSources.FirstOrDefault().Name, modisProducts.FirstOrDefault().Name));
            //ViewBag.ModisDataSet = new MultiSelectList(_context.ModisDataSet.Where(m => m.ModisProductId == modisProducts.FirstOrDefault().Id).OrderBy(m => m.Index), "Id", "IndexName");
            ViewBag.ModisDataSet = new SelectList(_context.ModisDataSet.Where(m => m.ModisProductId == modisProducts.FirstOrDefault().Id).OrderBy(m => m.Index), "Id", "IndexName");
            return View();
        }

        [HttpPost]
        public IActionResult ModisMosaic(string ModisSource,
            string ModisProduct,
            int ModisDataSet,
            string File,
            string FileName)
        {

            Task t = new Task( () => { MosaicModis(ModisSource, ModisProduct, ModisDataSet, File, FileName); });
            t.Start();
            ViewBag.Message = "Operation started!";
            var modisSources = _context.ModisSource.OrderBy(m => m.Name);
            ViewBag.ModisSource = new SelectList(modisSources, "Name", "Name", ModisSource);
            var modisProducts = _context.ModisProduct.Include(m => m.ModisSource).Where(m => m.ModisSource.Name == ModisSource).OrderBy(m => m.Name);
            ViewBag.ModisProduct = new SelectList(modisProducts, "Name", "Name", ModisProduct);
            ViewBag.File = new SelectList(GetModisListFiles(ModisSource, ModisProduct), File);
            //ViewBag.ModisDataSet = new MultiSelectList(_context.ModisDataSet.Include(m => m.ModisProduct).Where(m => m.ModisProduct.Name == ModisProduct).OrderBy(m => m.Index), "Id", "IndexName", ModisDataSet);
            ViewBag.ModisDataSet = new SelectList(_context.ModisDataSet.Include(m => m.ModisProduct).Where(m => m.ModisProduct.Name == ModisProduct).OrderBy(m => m.Index), "Id", "IndexName", ModisDataSet);
            ViewBag.FileName = FileName;
            return View();
        }

        public IActionResult Reproject()
        {
            var modisSources = _context.ModisSource.OrderBy(m => m.Name);
            ViewBag.ModisSource = new SelectList(modisSources, "Name", "Name");
            var modisProducts = _context.ModisProduct.Where(m => m.ModisSourceId == _context.ModisSource.OrderBy(ms => ms.Name).FirstOrDefault().Id).OrderBy(m => m.Name);
            ViewBag.ModisProduct = new SelectList(modisProducts, "Name", "Name");
            ViewBag.File = new SelectList(GetReprojectVrtFiles(modisSources.FirstOrDefault().Name, modisProducts.FirstOrDefault().Name));
            var coordinateSystem = _context.CoordinateSystems.OrderBy(m => m.Name);
            ViewBag.CoordinateSystem = new SelectList(coordinateSystem, "Name", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Reproject(string ModisSource,
            string ModisProduct,
            //string[] File,
            string File,
            string CoordinateSystem)
        {
            //foreach(string file in File)
            //{
            //    ReprojectGeoTif(ModisSource, ModisProduct, file, CoordinateSystem);
            //}
            ReprojectVrt(ModisSource, ModisProduct, File, CoordinateSystem);
            ViewBag.Message = "Operation started!";
            var modisSources = _context.ModisSource.OrderBy(m => m.Name);
            ViewBag.ModisSource = new SelectList(modisSources, "Name", "Name", ModisSource);
            var modisProducts = _context.ModisProduct.Include(m => m.ModisSource).Where(m => m.ModisSource.Name == ModisSource).OrderBy(m => m.Name);
            ViewBag.ModisProduct = new SelectList(modisProducts, "Name", "Name", ModisProduct);
            ViewBag.File = new SelectList(GetReprojectVrtFiles(ModisSource, ModisProduct), File);
            var coordinateSystem = _context.CoordinateSystems.OrderBy(m => m.Name);
            ViewBag.CoordinateSystem = new SelectList(coordinateSystem, "Name", "Name", CoordinateSystem);
            return View();
        }

        private List<string> GetModisListFiles(string ModisSource,
            string ModisProduct)
        {
            string folder = Path.Combine(Startup.Configuration["Modis:ModisPath"], ModisSource, ModisProduct);
            if(Directory.Exists(folder))
            {
                return Directory.GetFiles(folder, "*.txt").Select(f => Path.GetFileName(f)).ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        private List<string> GetReprojectFiles(string ModisSource,
            string ModisProduct)
        {
            string folder = Path.Combine(Startup.Configuration["Modis:ModisPath"], ModisSource, ModisProduct);
            if (Directory.Exists(folder))
            {
                return Directory.GetFiles(folder, "*.tif").Select(f => Path.GetFileName(f)).ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        private List<string> GetReprojectVrtFiles(string ModisSource,
            string ModisProduct)
        {
            string folder = Path.Combine(Startup.Configuration["Modis:ModisPath"], ModisSource, ModisProduct);
            if (Directory.Exists(folder))
            {
                return Directory.GetFiles(folder, "*.vrt").Select(f => Path.GetFileName(f)).ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult GetModisTextFiles(string ModisSource,
            string ModisProduct)
        {
            JsonResult result = new JsonResult(GetModisListFiles(ModisSource, ModisProduct));
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult GetModisGeoTifFiles(string ModisSource,
            string ModisProduct)
        {
            JsonResult result = new JsonResult(GetReprojectFiles(ModisSource, ModisProduct));
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult GetModisVrtFiles(string ModisSource,
            string ModisProduct)
        {
            JsonResult result = new JsonResult(GetReprojectVrtFiles(ModisSource, ModisProduct));
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult GetModisDataSets(string ModisProduct)
        {
            JsonResult result = new JsonResult(_context.ModisDataSet.Include(m => m.ModisProduct).Where(m => m.ModisProduct.Name == ModisProduct).OrderBy(m => m.Index).ToArray());
            return result;
        }
    }
}