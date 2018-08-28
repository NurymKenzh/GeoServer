using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GeoServer.Controllers
{
    public class GeoServerController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public GeoServerController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

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
        public void UploadWorkspaceLayerFile(string WorkspaceName, List<IFormFile> Files)
        {
            foreach (IFormFile file in Files)
            {
                var filePath = Path.Combine(GetWorkspaceDirectoryPath(WorkspaceName), Path.GetFileName(file.FileName));
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
        }

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

        public string GetLayerWorkspace(string LayerName)
        {
            try
            {
                Process process = CurlExecute($" -u " +
                    $"{Startup.Configuration["GeoServer:User"]}:" +
                    $"{Startup.Configuration["GeoServer:Password"]}" +
                    $" -XGET -H \"Content-type: text/xml\"" +
                    $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                    $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/layers/{LayerName}.xml");
                string html = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                HtmlNode root = htmlDocument.DocumentNode;
                string layer = string.Empty;
                foreach (HtmlNode node in root.Descendants())
                {
                    if (node.Name == "resource")
                    {
                        foreach (HtmlNode nodeatom in node.Descendants())
                        {
                            if (nodeatom.Name == "atom:link")
                            {
                                string[] href = nodeatom.Attributes.First(a => a.Name == "href").Value.Split('/');
                                layer = href[Array.IndexOf(href, "workspaces") + 1];
                            }
                        }
                    }
                }
                if(string.IsNullOrEmpty(layer))
                {
                    throw new Exception(html);
                }
                return layer;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public string[] GetWorkspaceStores(string WorkspaceName)
        {
            try
            {
                if (!GetWorkspaces().Contains(WorkspaceName))
                {
                    throw new Exception($"No workspace {WorkspaceName}!");
                }
                if (!string.IsNullOrEmpty(WorkspaceName))
                {
                    Process process = CurlExecute($" -u " +
                        $"{Startup.Configuration["GeoServer:User"]}:" +
                        $"{Startup.Configuration["GeoServer:Password"]}" +
                        $" -XGET" +
                        $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                        $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}");
                    string html = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);
                    HtmlNode root = htmlDocument.DocumentNode;
                    List<string> stores = new List<string>();
                    foreach (HtmlNode node in root.Descendants())
                    {
                        if (node.Name == "a")
                        {
                            stores.Add(node.InnerText);
                        }
                    }
                    return stores.ToArray();
                }
                else
                {
                    throw new Exception("WorkspaceName must be non-empty!");
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public string[] GetStoreLayers(string WorkspaceName, string StoreName)
        {
            try
            {
                if (!GetWorkspaces().Contains(WorkspaceName))
                {
                    throw new Exception($"No workspace {WorkspaceName}!");
                }
                if (!GetWorkspaceStores(WorkspaceName).Contains(StoreName))
                {
                    throw new Exception($"No store {WorkspaceName} in workspace {WorkspaceName}!");
                }
                if (string.IsNullOrEmpty(WorkspaceName))
                {
                    throw new Exception("WorkspaceName must be non-empty!");
                }
                if (string.IsNullOrEmpty(StoreName))
                {
                    throw new Exception("StoreName must be non-empty!");
                }
                Process process = CurlExecute($" -u " +
                    $"{Startup.Configuration["GeoServer:User"]}:" +
                    $"{Startup.Configuration["GeoServer:Password"]}" +
                    $" -XGET" +
                    $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                    $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}.html");
                string html = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                HtmlNode root = htmlDocument.DocumentNode;
                List<string> layers = new List<string>();
                foreach (HtmlNode node in root.Descendants())
                {
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

        public string[] GetWorkspaceLayers(string WorkspaceName)
        {
            try
            {
                List<string> layers = new List<string>();
                foreach(string store in GetWorkspaceStores(WorkspaceName))
                {
                    layers.AddRange(GetStoreLayers(WorkspaceName, store));
                }
                return layers.ToArray();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        //public void PublishGeoTIFF(string WorkspaceName, string FileName, string Style)
        //{
        //    try
        //    {
        //        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FileName);
        //        Process process1 = CurlExecute($" -u " +
        //            $"{Startup.Configuration["GeoServer:User"]}:" +
        //            $"{Startup.Configuration["GeoServer:Password"]}" +
        //            $" -v -XPOST" +
        //            $" -H \"Content-type: text/xml\"" +
        //            $" \\ -d \"<coverageStore><name>{fileNameWithoutExtension}</name>" +
        //            $"<workspace>{WorkspaceName}</workspace>" +
        //            $"<enabled>true</enabled>" +
        //            $"<type>GeoTIFF</type>" +
        //            $"<url>/data/{WorkspaceName}/{FileName}</url></coverageStore>\"" +
        //            $" \\ http://{Startup.Configuration["GeoServer:Address"]}:" +
        //            $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}/coveragestores?configure=all");
        //        process1.WaitForExit();
        //        Process process2 = CurlExecute($" -u " +
        //            $"{Startup.Configuration["GeoServer:User"]}:" +
        //            $"{Startup.Configuration["GeoServer:Password"]}" +
        //            $" -v -XPOST" +
        //            $" -H \"Content-type: text/xml\"" +
        //            $" -d \"<coverage><name>{fileNameWithoutExtension}</name>" +
        //            $"<title>{fileNameWithoutExtension}</title>" +
        //            $"<nativeCRS>EPSG:3857</nativeCRS>" +
        //            $"<srs>EPSG:3857</srs>" +
        //            $"<projectionPolicy>FORCE_DECLARED</projectionPolicy>" +
        //            $"<defaultInterpolationMethod><name>nearest neighbor</name></defaultInterpolationMethod></coverage>\"" +
        //            $" \\ \"http://{Startup.Configuration["GeoServer:Address"]}" +
        //            $":{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}/coveragestores/{fileNameWithoutExtension}/coverages?recalculate=nativebbox\"");
        //        process2.WaitForExit();
        //        Process process3 = CurlExecute($" -v -u " +
        //            $"{Startup.Configuration["GeoServer:User"]}:" +
        //            $"{Startup.Configuration["GeoServer:Password"]}" +
        //            $" -XPUT" +
        //            $" -H \"Content-type: text/xml\"" +
        //            $" -d \"<layer><defaultStyle><name>{WorkspaceName}:{Style}</name></defaultStyle></layer>\"" +
        //            $" http://{Startup.Configuration["GeoServer:Address"]}" +
        //            $":{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/layers/{WorkspaceName}:{fileNameWithoutExtension}");
        //        process3.WaitForExit();
        //    }
        //    catch (Exception exception)
        //    {
        //        throw new Exception(exception.ToString(), exception.InnerException);
        //    }
        //}

        public string[] GetWorkspaceStyles(string WorkspaceName)
        {
            try
            {
                if (!GetWorkspaces().Contains(WorkspaceName))
                {
                    throw new Exception($"No workspace {WorkspaceName}!");
                }
                if (!string.IsNullOrEmpty(WorkspaceName))
                {
                    Process process = CurlExecute($" -u " +
                        $"{Startup.Configuration["GeoServer:User"]}:" +
                        $"{Startup.Configuration["GeoServer:Password"]}" +
                        $" -XGET" +
                        $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                        $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}/styles");
                    string html = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);
                    HtmlNode root = htmlDocument.DocumentNode;
                    List<string> stores = new List<string>();
                    foreach (HtmlNode node in root.Descendants())
                    {
                        if (node.Name == "a")
                        {
                            stores.Add(node.InnerText);
                        }
                    }
                    return stores.ToArray();
                }
                else
                {
                    throw new Exception("WorkspaceName must be non-empty!");
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        public void CreateWorkspaceStyle(string WorkspaceName, string StyleName, string[] StyleText)
        {
            try
            {
                if (!GetWorkspaces().Contains(WorkspaceName))
                {
                    throw new Exception($"No workspace {WorkspaceName}!");
                }
                string styleFileName = Path.Combine(
                    _hostingEnvironment.ContentRootPath,
                    Path.Combine(
                        Startup.Configuration["GeoServer:SLDDir"],
                        Path.ChangeExtension(
                            StyleName,
                            "sld")));
                System.IO.File.WriteAllLines(styleFileName, StyleText);
                if (!string.IsNullOrEmpty(WorkspaceName))
                {
                    Process process1 = CurlExecute($" -v -u " +
                        $"{Startup.Configuration["GeoServer:User"]}:" +
                        $"{Startup.Configuration["GeoServer:Password"]}" +
                        $" -XPOST" +
                        $" -H \"Content-type: text/xml\" +" +
                        $" -d \"<style><name>{StyleName}</name><filename>{StyleName}.sld</filename></style>\"" +
                        $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                        $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}/styles");
                    process1.WaitForExit();

                    Process process2 = CurlExecute($" -v -u " +
                        $"{Startup.Configuration["GeoServer:User"]}:" +
                        $"{Startup.Configuration["GeoServer:Password"]}" +
                        $" -XPUT" +
                        $" -H \"Content-type: application/vnd.ogc.sld+xml\" +" +
                        $" -d @{styleFileName}" +
                        $" http://{Startup.Configuration["GeoServer:Address"]}:" +
                        $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}/styles/{StyleName}");
                    process2.WaitForExit();
                }
                else
                {
                    throw new Exception("WorkspaceName must be non-empty!");
                }
                try
                {
                    System.IO.File.Delete(styleFileName);
                }
                catch
                {

                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }

        /// <summary>
        /// Публикация GeoTIFF-файла в GeoServer
        /// </summary>
        /// <remarks>
        /// Будет опубликован слой в GeoServer с именем, совпадающим с именем файла (без расширения)
        /// Будет создано новое хранилище в GeoServer с именем, совпадающим с именем файла (без расширения)
        /// Если существует опубликованный слой с именем, совпадающим с именем файла (без расширения), в рабочей области с именем, совпадающим с именем файла (без расширения), будет выдано исключение
        /// </remarks>
        /// <param name="WorkspaceName">
        /// Рабочая область GeoServer, в которой публикуется слой
        /// </param>
        /// <param name="FileName">
        /// Имя публикуемого GeoTIFF-файла с расширением и путем (в папке data) в папке данных GeoServer
        /// </param>
        /// <param name="Style">
        /// Стиль GeoServer, с которым будет опубликован слой. Должен быть в рабочей области "WorkspaceName"
        /// </param>
        public void PublishGeoTIFF(string WorkspaceName, string FileName, string Style)
        {
            try
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FileName);

                //if (GetWorkspaceLayers(WorkspaceName).Contains(fileNameWithoutExtension))
                //{
                //    throw new Exception($"Layer {fileNameWithoutExtension} is already exist in {WorkspaceName} workspace!");
                //}

                if (Path.GetExtension(FileName).ToLower() != ".tif" || Path.GetExtension(FileName).ToLower() != ".tif")
                {
                    throw new Exception("File extension must be \"tif\" or \"tiff\"!");
                }

                FileName = FileName.Replace('\\', '/');

                Process process1 = CurlExecute($" -u " +
                    $"{Startup.Configuration["GeoServer:User"]}:" +
                    $"{Startup.Configuration["GeoServer:Password"]}" +
                    $" -v -XPOST" +
                    $" -H \"Content-type: text/xml\"" +
                    $" \\ -d \"<coverageStore><name>{fileNameWithoutExtension}</name>" +
                    $"<workspace>{WorkspaceName}</workspace>" +
                    $"<enabled>true</enabled>" +
                    $"<type>GeoTIFF</type>" +
                    $"<url>/data/{WorkspaceName}/{FileName}</url></coverageStore>\"" +
                    $" \\ http://{Startup.Configuration["GeoServer:Address"]}:" +
                    $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}/coveragestores?configure=all");
                process1.WaitForExit();
                Process process2 = CurlExecute($" -u " +
                    $"{Startup.Configuration["GeoServer:User"]}:" +
                    $"{Startup.Configuration["GeoServer:Password"]}" +
                    $" -v -XPOST" +
                    $" -H \"Content-type: text/xml\"" +
                    $" -d \"<coverage><name>{fileNameWithoutExtension}</name>" +
                    $"<title>{fileNameWithoutExtension}</title>" +
                    $"<nativeCRS>EPSG:3857</nativeCRS>" +
                    $"<srs>EPSG:3857</srs>" +
                    $"<projectionPolicy>FORCE_DECLARED</projectionPolicy>" +
                    $"<defaultInterpolationMethod><name>nearest neighbor</name></defaultInterpolationMethod></coverage>\"" +
                    $" \\ \"http://{Startup.Configuration["GeoServer:Address"]}" +
                    $":{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces/{WorkspaceName}/coveragestores/{fileNameWithoutExtension}/coverages?recalculate=nativebbox\"");
                process2.WaitForExit();
                Process process3 = CurlExecute($" -v -u " +
                    $"{Startup.Configuration["GeoServer:User"]}:" +
                    $"{Startup.Configuration["GeoServer:Password"]}" +
                    $" -XPUT" +
                    $" -H \"Content-type: text/xml\"" +
                    $" -d \"<layer><defaultStyle><name>{WorkspaceName}:{Style}</name></defaultStyle></layer>\"" +
                    $" http://{Startup.Configuration["GeoServer:Address"]}" +
                    $":{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/layers/{WorkspaceName}:{fileNameWithoutExtension}");
                process3.WaitForExit();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception.InnerException);
            }
        }
    }
}