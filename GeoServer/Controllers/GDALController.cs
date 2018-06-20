using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        private void ModisDownload(string[] ModisSpan,
            string ModisSource,
            string ModisProduct,
            DateTime DateStart,
            DateTime DateFinish)
        {
            try
            {
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

                var jobId = BackgroundJob.Enqueue(
                    () => PythonExecuteWithParameters("modis_download.py", $"-r -s {ModisSource} -p {ModisProduct} -t {string.Join(',', ModisSpan)} -f {DateStart.Year}-{DateStart.Month}-{DateStart.Day} -e {DateFinish.Year}-{DateFinish.Month}-{DateFinish.Day} {folder}"));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        private void MosaicModis(string ModisSource,
            string ModisProduct,
            string[] ModisDataSet,
            string File,
            string FileName)
        {
            string folder = Path.Combine(Startup.Configuration["Modis:ModisPath"], ModisSource, ModisProduct),
                batfile = Path.Combine(folder, "bat.bat");
            using (var sw = new StreamWriter(batfile))
            {
                sw.WriteLine("modis_mosaic.py -o 250m_16_days_EVI.tif -s \"0 1\"  D:\\GeoServer\\MODIS\\MOLT\\MOD13Q1\\listfileMYD09A1.006.txt");
            }

            Process process = new Process();
            try
            {
                //process.StartInfo.UseShellExecute = false;

                //process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.RedirectStandardInput = true;
                //process.StartInfo.RedirectStandardError = true;

                process.StartInfo.WorkingDirectory = folder;
                process.StartInfo.FileName = batfile;
                process.Start();

                //string pyhonOutput = process.StandardOutput.ReadToEnd();
                //string pyhonError = process.StandardError.ReadToEnd();
                process.WaitForExit();
                //if (!string.IsNullOrEmpty(pyhonError))
                //{
                //    throw new Exception(pyhonError);
                //}
                //else
                //{
                //    //return pyhonOutput;
                //}

                System.IO.File.Delete(batfile);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
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
            ModisDownload(ModisSpan, ModisSource, ModisProduct, DateStart, DateFinish);
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
            ModisDownload(ModisSpan, ModisSource, ModisProduct, DateStart, DateFinish);
            ViewBag.Message = "Operation started!";
            ViewBag.ModisSpan = new MultiSelectList(_context.ModisSpan.OrderBy(m => m.Name), "Name", "Name", ModisSpan);
            ViewBag.ModisSource = new SelectList(_context.ModisSource.OrderBy(m => m.Name), "Name", "Name", ModisSource);
            ViewBag.ModisProduct = new SelectList(_context.ModisProduct.Include(m => m.ModisSource).Where(m => m.ModisSource.Name == ModisSource).OrderBy(m => m.Name), "Name", "Name", ModisProduct);
            ViewBag.DateStart = DateStart;
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
            ViewBag.ModisDataSet = new MultiSelectList(_context.ModisDataSet.Where(m => m.ModisProductId == modisProducts.FirstOrDefault().Id).OrderBy(m => m.Index), "Id", "IndexName");
            return View();
        }

        [HttpPost]
        public IActionResult ModisMosaic(string ModisSource,
            string ModisProduct,
            string[] ModisDataSet,
            string File,
            string FileName)
        {

            MosaicModis(ModisSource, ModisProduct, ModisDataSet, File, FileName);
            ViewBag.Message = "Operation started!";
            var modisSources = _context.ModisSource.OrderBy(m => m.Name);
            ViewBag.ModisSource = new SelectList(modisSources, "Name", "Name", ModisSource);
            var modisProducts = _context.ModisProduct.Include(m => m.ModisSource).Where(m => m.ModisSource.Name == ModisSource).OrderBy(m => m.Name);
            ViewBag.ModisProduct = new SelectList(modisProducts, "Name", "Name", ModisProduct);
            ViewBag.File = new SelectList(GetModisListFiles(ModisSource, ModisProduct), File);
            ViewBag.ModisDataSet = new MultiSelectList(_context.ModisDataSet.Include(m => m.ModisProduct).Where(m => m.ModisProduct.Name == ModisProduct).OrderBy(m => m.Index), "Id", "IndexName", ModisDataSet);
            ViewBag.FileName = FileName;
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
        public JsonResult GetModisDataSets(string ModisProduct)
        {
            JsonResult result = new JsonResult(_context.ModisDataSet.Include(m => m.ModisProduct).Where(m => m.ModisProduct.Name == ModisProduct).OrderBy(m => m.Index).ToArray());
            return result;
        }
    }
}