try:
        import sys
        import gdal
        if __name__ == '__main__':
                file = sys.argv[1]
                ds = gdal.Open(file)
                print ds.RasterCount
except Exception as exception:
        raise ValueError(exception)