using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GeoServer.Controllers
{
    public class GDALController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public GDALController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
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
                return PythonExecute("GetLayerCoordinateSystemName", FilePath);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }
    }
}