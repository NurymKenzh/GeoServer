using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GeoServer.Controllers
{
    public class GeoServerController : Controller
    {
        private Process CurlExecute(string Arguments)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = Startup.Configuration["GeoServer:CurlFullPath"];
                process.StartInfo.Arguments = Arguments;
                process.Start();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
            return process;
        }

        private string GetWorkspaceDirectoryPath(string WorkspaceName)
        {
            try
            {
                return Path.Combine(Path.Combine(Startup.Configuration["GeoServer:DataDir"], "data"), WorkspaceName);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public string[] GetWorkspaces()
        {
            try
            {
                Process process = CurlExecute($" -u " +
                $"{Startup.Configuration["GeoServer:User"]}:" +
                $"{Startup.Configuration["GeoServer:Password"]}" +
                $" -XGET" +
                $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces");
                string html = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                HtmlNode root = htmlDocument.DocumentNode;
                List<string> workspaces = new List<string>();
                foreach (HtmlNode node in root.Descendants())
                {
                    if (node.Name == "title" && node.InnerText.ToLower().Contains("error"))
                    {
                        throw new Exception(node.InnerText);
                    }
                    if (node.Name == "a")
                    {
                        workspaces.Add(node.InnerText);
                    }
                }
                return workspaces.ToArray();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public void CreateWorkspace(string WorkspaceName)
        {
            try
            {
                Process process = CurlExecute($" -v -u " +
                $"{Startup.Configuration["GeoServer:User"]}:" +
                $"{Startup.Configuration["GeoServer:Password"]}" +
                $" -POST -H \"Content-type: text/xml\"" +
                $" -d \"<workspace><name>{WorkspaceName}</name></workspace>\"" +
                $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces");
                string output = process.StandardOutput.ReadToEnd();
                if (!string.IsNullOrEmpty(output))
                {
                    throw new Exception(output);
                }
                process.WaitForExit();
                Directory.CreateDirectory(GetWorkspaceDirectoryPath(WorkspaceName));
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public void DeleteWorkspace(string WorkspaceName)
        {
            try
            {
                Process process = CurlExecute($" -v -u " +
                $"{Startup.Configuration["GeoServer:User"]}:" +
                $"{Startup.Configuration["GeoServer:Password"]}" +
                $" -XDELETE" +
                $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}");
                string output = process.StandardOutput.ReadToEnd();
                if (!string.IsNullOrEmpty(output))
                {
                    throw new Exception(output);
                }
                process.WaitForExit();
                Directory.Delete(GetWorkspaceDirectoryPath(WorkspaceName), true);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }
        
        public string[] GetWorkspaceLayerFiles(string WorkspaceName)
        {
            try
            {
                string[] files = Directory
                    .GetFiles(GetWorkspaceDirectoryPath(WorkspaceName), "*.tif", SearchOption.TopDirectoryOnly)
                    .Select(Path.GetFileName)
                    .ToArray();
                return files;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        [DisableRequestSizeLimit]
        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult UploadWorkspaceLayerFile()
        {
            ViewData["Workspaces"] = new SelectList(GetWorkspaces());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableRequestSizeLimit]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> UploadWorkspaceLayerFile(string WorkspaceName, List<IFormFile> Files)
        {
            foreach (IFormFile file in Files)
            {
                var filePath = Path.Combine(GetWorkspaceDirectoryPath(WorkspaceName), Path.GetFileName(file.FileName));
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return View();
        }

        //public string[] GetWorkspaceLayers(string WorkspaceName)
        //{
        //    try
        //    {
        //        if(!string.IsNullOrEmpty(WorkspaceName))
        //        {
        //            // curl -u admin:geoserver -XGET http://localhost:8080/geoserver/rest/workspaces/Pastures/layers.xml
        //            Process process = CurlExecute($" -u " +
        //            $"{Startup.Configuration["GeoServer:User"]}:" +
        //            $"{Startup.Configuration["GeoServer:Password"]}" +
        //            $" -XGET" +
        //            $" http://{Startup.Configuration["GeoServer:Address"]}:" +
        //            $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/" +
        //            $"{WorkspaceName}");
        //            string html = process.StandardOutput.ReadToEnd();
        //            process.WaitForExit();

        //            HtmlDocument htmlDocument = new HtmlDocument();
        //            htmlDocument.LoadHtml(html);
        //            HtmlNode root = htmlDocument.DocumentNode;
        //            List<string> workspaces = new List<string>();
        //            foreach (HtmlNode node in root.Descendants())
        //            {
        //                if (node.Name == "title" && node.InnerText.ToLower().Contains("error"))
        //                {
        //                    throw new Exception(node.InnerText);
        //                }
        //                if (node.Name == "a")
        //                {
        //                    workspaces.Add(node.InnerText);
        //                }
        //            }
        //            return workspaces.ToArray();
        //        }
        //        else
        //        {
        //            return new string[0];
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        throw new Exception(exception.ToString(), exception.InnerException);
        //    }
        //}

        public string[] GetLayers()
        {
            try
            {
                Process process = CurlExecute($" -u " +
                $"{Startup.Configuration["GeoServer:User"]}:" +
                $"{Startup.Configuration["GeoServer:Password"]}" +
                $" -XGET" +
                $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/layers");
                string html = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                HtmlNode root = htmlDocument.DocumentNode;
                List<string> layers = new List<string>();
                foreach (HtmlNode node in root.Descendants())
                {
                    if (node.Name == "title" && node.InnerText.ToLower().Contains("error"))
                    {
                        throw new Exception(node.InnerText);
                    }
                    if (node.Name == "a")
                    {
                        layers.Add(node.InnerText);
                    }
                }
                return layers.ToArray();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }
    }
}