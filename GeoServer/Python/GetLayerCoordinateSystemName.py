try:
        import gdal
        import osr
        file = raw_input()
        ds = gdal.Open(file)
        print osr.SpatialReference(wkt=ds.GetProjection()).GetAttrValue("AUTHORITY", 0) + ":" + osr.SpatialReference(wkt=ds.GetProjection()).GetAttrValue("PROJCS|AUTHORITY", 1)
except Exception as exception:
        raise ValueError(exception)
