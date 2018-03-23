try:
        import gdal
        file = raw_input()
        ds = gdal.Open(file)
        print ds.RasterCount
except Exception as exception:
        raise ValueError(exception)
