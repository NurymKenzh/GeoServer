using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoServer.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace GeoServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoServerController _GeoServer;
        private readonly GDALController _GDAL;
        private readonly ModisController _Modis;

        public HomeController(GeoServerController GeoServer,
            GDALController GDAL,
            ModisController Modis)
        {
            _GeoServer = GeoServer;
            _GDAL = GDAL;
            _Modis = Modis;
        }

        public IActionResult Index()
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("ru")),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return View();
            //return RedirectToAction("ViewModis", "OL", null);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Administrator()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Directories()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Analize()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Test()
        {
            //try
            //{
            //    ViewData["Message"] = "GeoServer workspaces: " + string.Join(", ", _GeoServer.GetWorkspaces());
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = "Raster bands count: " + _GDAL.GetRasterBandsCount(@"D:\Documents\MailCloud\Work\AtlasSolar\Maps\LandCover\glckz.tif").ToString();
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    string workspaceName = "Test";
            //    _GeoServer.CreateWorkspace(workspaceName);
            //    ViewData["Message"] = "Create GeoServer workspace: " + workspaceName;
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    string workspaceName = "Test";
            //    _GeoServer.DeleteWorkspace(workspaceName);
            //    ViewData["Message"] = "Delete GeoServer workspace: " + workspaceName;
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = "Test workspace files: " + string.Join(", ", _GeoServer.GetWorkspaceLayerFiles("Test"));
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //return RedirectToAction("UploadWorkspaceLayerFile", "GeoServer");

            //try
            //{
            //    ViewData["Message"] = "GeoServer layers: " + string.Join(", ", _GeoServer.GetLayers());
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = "adm1pol layer workspace: " + _GeoServer.GetLayerWorkspace("adm1pol");
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = "GeoServer topp layers: " + string.Join(", ", _GeoServer.GetWorkspaceLayers("topp"));
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = "GeoServer tiger stores: " + string.Join(", ", _GeoServer.GetWorkspaceStores("tiger1"));
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = "GeoServer topp workspace taz_shapes store layers: " + string.Join(", ", _GeoServer.GetStoreLayers("topp", "taz_shapes"));
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _GeoServer.PublishGeoTIFF("Test", "randomkz_10000_3857_1b.tif", "my_style");
            //    ViewData["Message"] = "GeoServer workspace Test layer randomkz_10000_3857_1b published";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _GDAL.SaveLayerWithNewCoordinateSystem(@"C:\Users\N\Documents\New\MODIS\MOD13Q1.A2007097.h23v04.006.2015161232334_01.tif", @"C:\Users\N\Documents\New\MODIS\MOD13Q1.A2007097.h23v04.006.2015161232334_01_3857.tif", "EPSG:3857");
            //    ViewData["Message"] = "SaveLayerWithNewCoordinateSystem";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = "Raster bands count: " + _GDAL.QGISGetRasterBandsCount(@"D:\\Documents\\New\\MOD13Q1\\MOD13Q1.A2017225.h21v03.006.2017250141803.hdf").ToString();
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = "GeoServer Eco styles: " + string.Join(", ", _GeoServer.GetWorkspaceStyles("Eco"));
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _GeoServer.CreateWorkspaceStyle("Test", "my_style",
            //        new string[]
            //            {
            //                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
            //                "<StyledLayerDescriptor xmlns=\"http://www.opengis.net/sld\" xmlns:ogc=\"http://www.opengis.net/ogc\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.opengis.net/sld",
            //                "http://schemas.opengis.net/sld/1.0.0/StyledLayerDescriptor.xsd\" version=\"1.0.0\">",
            //                "  <NamedLayer>",
            //                "    <Name>my_style</Name>",
            //                "    <UserStyle>",
            //                "      <Title>my_style</Title>",
            //                "      <FeatureTypeStyle>",
            //                "        <Rule>",
            //                "          <RasterSymbolizer>",
            //                "            <Opacity>1.0</Opacity>",
            //                "            <ColorMap>",
            //                "              <ColorMapEntry color=\"#267300\" quantity=\"10\" opacity=\"0\"/>",
            //                "              <ColorMapEntry color=\"#267300\" quantity=\"20\" opacity=\"1\"/>",
            //                "              <ColorMapEntry color=\"#38a800\" quantity=\"30\" opacity=\"1\"/>",
            //                "              <ColorMapEntry color=\"#4ce600\" quantity=\"40\" opacity=\"1\"/>",
            //                "              <ColorMapEntry color=\"#a3ff73\" quantity=\"50\" opacity=\"1\"/>",
            //                "              <ColorMapEntry color=\"#e9ffbe\" quantity=\"60\" opacity=\"1\"/>",
            //                "              <ColorMapEntry color=\"#ffff73\" quantity=\"70\" opacity=\"1\"/>",
            //                "              <ColorMapEntry color=\"#ffd37f\" quantity=\"80\" opacity=\"1\"/>",
            //                "              <ColorMapEntry color=\"#ffaa00\" quantity=\"90\" opacity=\"1\"/>",
            //                "              <ColorMapEntry color=\"#e64c00\" quantity=\"100\" opacity=\"1\"/>",
            //                "            </ColorMap>",
            //                "          </RasterSymbolizer>",
            //                "        </Rule>",
            //                "      </FeatureTypeStyle>",
            //                "    </UserStyle>",
            //                "  </NamedLayer>",
            //                "</StyledLayerDescriptor>"
            //            }
            //        );
            //    ViewData["Message"] = "GeoServer create style";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _GDAL.HdfToGeoTIFF(@"D:\Documents\New\MOD13Q1\MOD13Q1.A2017225.h21v03.006.2017250141803.hdf", "EPSG:3857");
            //    ViewData["Message"] = "HdfToGeoTIFF";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = string.Join("<br />", _Modis.GetHDFFilePaths().ToArray());
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    ViewData["Message"] = string.Join("<br />", _Modis.GetTifFileNamesOfHDF(@"C:\Users\N\Documents\New\MODIS\MOD13Q1.A2007097.h21v03.006.2015161233224.hdf").ToArray());
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    string message = "";
            //    List<KeyValuePair<int, int>> yearsdays = _Modis.GetHDFsYearsDaysOfYears();
            //    foreach(KeyValuePair<int, int> yearday in yearsdays)
            //    {
            //        message += $"\r\n{yearday.Key}.{yearday.Value}";
            //    }
            //    ViewData["Message"] = message;
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _GDAL.MergeTifs(@"C:\Users\N\Documents\New\tifs\merged.tif",
            //        @"C:\Users\N\Documents\New\tifs\MOD13Q1.A2007097.h21v03.006.2015161233224_01.tif",
            //        @"C:\Users\N\Documents\New\tifs\MOD13Q1.A2007097.h21v04.006.2015161232100_01.tif");
            //    ViewData["Message"] = "MergeTifs";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _Modis.ConvertHdfsToTifs("EPSG:3857");
            //    ViewData["Message"] = "Files converted";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _Modis.ReprojectTifs("EPSG:3857");
            //    ViewData["Message"] = "Files converted";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _Modis.MergeTifs();
            //    ViewData["Message"] = "Merge Modis Tifs";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _GDAL.ModisDownload();
            //    ViewData["Message"] = "ModisDownload";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
