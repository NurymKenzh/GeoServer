using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoServer.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace GeoServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoServerController _GeoServer;
        private readonly GDALController _GDAL;

        public HomeController(GeoServerController GeoServer,
            GDALController GDAL)
        {
            _GeoServer = GeoServer;
            _GDAL = GDAL;
        }

        public IActionResult Index()
        {
            return View();
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
            //    _GeoServer.PublishGeoTIFF("Test", "kz.tif");
            //    ViewData["Message"] = "GeoServer workspace Test layer kz published";
            //}
            //catch (Exception exception)
            //{
            //    ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            //}

            //try
            //{
            //    _GDAL.SaveLayerWithNewCoordinateSystem(@"D:\Documents\New\maps\randomkz_250_3857_1b.tif", @"D:\\Documents\New\maps\randomkz_250_4326_1b.tif", "EPSG:4326");
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

            try
            {
                _GeoServer.CreateWorkspaceStyle("Test", "my_style",
                    new string[]
                        {
                            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
                            "<StyledLayerDescriptor xmlns=\"http://www.opengis.net/sld\" xmlns:ogc=\"http://www.opengis.net/ogc\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.opengis.net/sld",
                            "http://schemas.opengis.net/sld/1.0.0/StyledLayerDescriptor.xsd\" version=\"1.0.0\">",
                            "  <NamedLayer>",
                            "    <Name>my_style</Name>",
                            "    <UserStyle>",
                            "      <Title>my_style</Title>",
                            "      <FeatureTypeStyle>",
                            "        <Rule>",
                            "          <RasterSymbolizer>",
                            "            <Opacity>1.0</Opacity>",
                            "            <ColorMap>",
                            "              <ColorMapEntry color=\"#267300\" quantity=\"10\" opacity=\"0\"/>",
                            "              <ColorMapEntry color=\"#267300\" quantity=\"20\" opacity=\"1\"/>",
                            "              <ColorMapEntry color=\"#38a800\" quantity=\"30\" opacity=\"1\"/>",
                            "              <ColorMapEntry color=\"#4ce600\" quantity=\"40\" opacity=\"1\"/>",
                            "              <ColorMapEntry color=\"#a3ff73\" quantity=\"50\" opacity=\"1\"/>",
                            "              <ColorMapEntry color=\"#e9ffbe\" quantity=\"60\" opacity=\"1\"/>",
                            "              <ColorMapEntry color=\"#ffff73\" quantity=\"70\" opacity=\"1\"/>",
                            "              <ColorMapEntry color=\"#ffd37f\" quantity=\"80\" opacity=\"1\"/>",
                            "              <ColorMapEntry color=\"#ffaa00\" quantity=\"90\" opacity=\"1\"/>",
                            "              <ColorMapEntry color=\"#e64c00\" quantity=\"100\" opacity=\"1\"/>",
                            "            </ColorMap>",
                            "          </RasterSymbolizer>",
                            "        </Rule>",
                            "      </FeatureTypeStyle>",
                            "    </UserStyle>",
                            "  </NamedLayer>",
                            "</StyledLayerDescriptor>"
                        }
                    );
                ViewData["Message"] = "GeoServer create style";
            }
            catch (Exception exception)
            {
                ViewData["Message"] = $"{exception.ToString()}. {exception.InnerException?.Message}";
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
