using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

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

        public string[] GetWorkspaces()
        {
            try
            {
                Process process = CurlExecute($" -u " +
                $"{Startup.Configuration["GeoServer:User"]}:" +
                $"{Startup.Configuration["GeoServer:Password"]} -XGET \"http://" +
                $"{Startup.Configuration["GeoServer:Address"]}:" +
                $"{Startup.Configuration["GeoServer:Port"]}/geoserver/rest/workspaces\"");
                string html = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);
                HtmlNode root = htmlDocument.DocumentNode;
                List<string> workspaces = new List<string>();
                foreach (HtmlNode node in root.Descendants())
                {
                    if (node.Name == "p" && node.InnerText.Contains("No AuthenticationProvider found for org.springframework.security.authentication.UsernamePasswordAuthenticationToken"))
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
    }
}