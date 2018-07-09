import gdal, ogr, osr, numpy


def zonal_stats(feat, input_zone_polygon, input_value_raster):

    # Open data
    raster = gdal.Open(input_value_raster)
    shp = ogr.Open(input_zone_polygon)
    lyr = shp.GetLayer()

    # Get raster georeference info
    transform = raster.GetGeoTransform()
    xOrigin = transform[0]
    yOrigin = transform[3]
    pixelWidth = transform[1]
    pixelHeight = transform[5]

    # Get extent of feat
    geom = feat.GetGeometryRef()
    if (geom.GetGeometryName() == 'MULTIPOLYGON'):
        count = 0
        pointsX = []; pointsY = []
        for polygon in geom:
            geomInner = geom.GetGeometryRef(count)    
            ring = geomInner.GetGeometryRef(0)
            numpoints = ring.GetPointCount()
            for p in range(numpoints):
                    lon, lat, z = ring.GetPoint(p)
                    pointsX.append(lon)
                    pointsY.append(lat)    
            count += 1
    elif (geom.GetGeometryName() == 'POLYGON'):
        ring = geom.GetGeometryRef(0)
        numpoints = ring.GetPointCount()
        pointsX = []; pointsY = []
        for p in range(numpoints):
                lon, lat, z = ring.GetPoint(p)
                pointsX.append(lon)
                pointsY.append(lat)

    else:
        sys.exit()

    xmin = min(pointsX)
    xmax = max(pointsX)
    ymin = min(pointsY)
    ymax = max(pointsY)

    # Specify offset and rows and columns to read
    xoff = int((xmin - xOrigin)/pixelWidth)
    yoff = int((yOrigin - ymax)/pixelWidth)
    xcount = int((xmax - xmin)/pixelWidth)+1
    ycount = int((ymax - ymin)/pixelWidth)+1

    # Create memory target raster
    target_ds = gdal.GetDriverByName('MEM').Create('id', xcount, ycount, gdal.GDT_Byte)
    target_ds.SetGeoTransform((
        xmin, pixelWidth, 0,
        ymax, 0, pixelHeight,
    ))

    # Create for target raster the same projection as for the value raster
    raster_srs = osr.SpatialReference()
    raster_srs.ImportFromWkt(raster.GetProjectionRef())
    target_ds.SetProjection(raster_srs.ExportToWkt())

    # Rasterize zone polygon to raster
    gdal.RasterizeLayer(target_ds, [1], lyr, burn_values=[1])

    # Read raster as arrays
    banddataraster = raster.GetRasterBand(1)
    dataraster = banddataraster.ReadAsArray(xoff, yoff, xcount, ycount).astype(numpy.float)

    bandmask = target_ds.GetRasterBand(1)
    datamask = bandmask.ReadAsArray(0, 0, xcount, ycount).astype(numpy.float)

    # Mask zone of raster
    zoneraster = numpy.ma.masked_array(dataraster,  numpy.logical_not(datamask))

    # Calculate statistics of zonal raster
    return numpy.mean(zoneraster)


def loop_zonal_stats(input_zone_polygon, input_value_raster, input_field):

    shp = ogr.Open(input_zone_polygon)
    lyr = shp.GetLayer()
    featList = range(lyr.GetFeatureCount())
##    statDict = {}
##    statDict = "["
    statDict = []
    for FID in featList:
        feat = lyr.GetFeature(FID)
        kato = feat.GetField(input_field)
        meanValue = zonal_stats(feat, input_zone_polygon, input_value_raster)
##        statDict[kato] = meanValue
##        statDict += "\"" + kato + "\":" + str(meanValue) + ","
        statDict.append(kato + ":" + str(meanValue))
##    statDict = statDict[:-1]
##    statDict += "]"
    return statDict

# Vector dataset(zones)
##input_zone_polygon = 'D:/GeoServer/Base/adm1pol20180317.shp'
input_zone_polygon = raw_input()
    
# Raster dataset
##input_value_raster = 'D:/GeoServer/MODIS/MOLT/MOD13Q1/testA2018001_NDVI.vrt.tif'
input_value_raster = raw_input()

##input_field = 'kato_te'
input_field = raw_input()

# print pretty list
from pprint import pprint
pprint (loop_zonal_stats(input_zone_polygon, input_value_raster, input_field))
##import pprint
### write to a .txt file
##with open("C:/Users/X/Documents/ZonalStatTest/ZonalStatTest/file_out.txt", "w") as fout:
##     fout.write(pprint.pformat(loop_zonal_stats(input_zone_polygon, input_value_raster)))
### write to a .json file
##import json
##arr = [loop_zonal_stats(input_zone_polygon, input_value_raster)]
##data = json.dumps(arr, indent=4, )
### print array
##print (data)
##with open("C:/Users/X/Documents/ZonalStatTest/ZonalStatTest/2forces.json","wb") as f:
##  f.write(data)
