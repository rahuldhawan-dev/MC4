using System;
using System.IO;
using System.Net;
using System.Web.UI.WebControls;

namespace MMSINC.GIS
{
    public class MapScript
    {
        #region Private Members

        private static string scriptFormatString = "<script type=\"text/javascript\" src=\"{0}\"></script>";

        #endregion

    }

    public enum Ellipsoid : int
    {
        Airy = 0,
        Australian_National,
        Bessel_1841,
        Bessel_1841_Nambia,
        Clarke_1866,
        Clarke_1880,
        Everest,
        Fischer_1960,
        Fischer_1968,
        GRS_1967,
        GRS_1980,
        Helmert_1906,
        Hough,
        International,
        Krassovsky,
        Modified_Airy,
        Modified_Everest,
        Modified_Fischer_1960,
        South_American_1969,
        WGS_60,
        WGS_66,
        WGS_72,
        WGS_84,
        Length
    }

    internal class EllipsoidConsts
    {
        internal EllipsoidConsts()
        {
            a = 0.0;
            b = 0.0;
            eccSquared = 0.0;
            secEccSquared = 0.0;
        }

        internal void setValues(double a, double b, double eccSquared)
        {
            this.a = a;
            this.b = b;
            this.eccSquared = eccSquared;
            this.secEccSquared = (a * a - b * b) / (b * b);
        }

        internal double a;     //Semi-major axis (equatorial semi-axis) of ellipsoid.
        internal double b;     //Semi-minor axis (polar semi-axis) of ellipsoid. 
        internal double eccSquared;  //Eccentricity squared. (a*a -b*b)/(a*a)
        internal double secEccSquared;    //2nd eccentricity squared.	  (a*a - b*b)/(b*b)	
    }
    public class PositionConv
    {
        #region PositionConv internal stuff
        static PositionConv()
        {
            // Create lookup-table of ellipspoid-values...
            ellipTable = new EllipsoidConsts[(int)Ellipsoid.Length];
            int i;
            for (i = 0; i < (int)Ellipsoid.Length; i++)
            {
                ellipTable[i] = new EllipsoidConsts();
            }

            ellipTable[(int)Ellipsoid.Airy].setValues(6377563.0, 6356752.3142, 0.00667054);
            ellipTable[(int)Ellipsoid.Australian_National].setValues(6378160, 6356752.3142, 0.006694542);
            ellipTable[(int)Ellipsoid.Bessel_1841].setValues(6377397, 6356752.3142, 0.006674372);
            ellipTable[(int)Ellipsoid.Bessel_1841_Nambia].setValues(6377484, 6356752.3142, 0.006674372);
            ellipTable[(int)Ellipsoid.Clarke_1866].setValues(6378206, 6356752.3142, 0.006768658);
            ellipTable[(int)Ellipsoid.Clarke_1880].setValues(6378249, 6356752.3142, 0.006803511);
            ellipTable[(int)Ellipsoid.Everest].setValues(6377276, 6356752.3142, 0.006637847);
            ellipTable[(int)Ellipsoid.Fischer_1960].setValues(6378166, 6356752.3142, 0.006693422);
            ellipTable[(int)Ellipsoid.Fischer_1968].setValues(6378150, 6356752.3142, 0.006693422);
            ellipTable[(int)Ellipsoid.GRS_1967].setValues(6378160, 6356752.3142, 0.006694605);
            ellipTable[(int)Ellipsoid.GRS_1980].setValues(6378137, 6356752.3142, 0.00669438);
            ellipTable[(int)Ellipsoid.Helmert_1906].setValues(6378200, 6356752.3142, 0.006693422);
            ellipTable[(int)Ellipsoid.Hough].setValues(6378270, 6356752.3142, 0.00672267);
            ellipTable[(int)Ellipsoid.International].setValues(6378388, 6356752.3142, 0.00672267);
            ellipTable[(int)Ellipsoid.Krassovsky].setValues(6378245, 6356752.3142, 0.006693422);
            ellipTable[(int)Ellipsoid.Modified_Airy].setValues(6377340, 6356752.3142, 0.00667054);
            ellipTable[(int)Ellipsoid.Modified_Everest].setValues(6377304, 6356752.3142, 0.006637847);
            ellipTable[(int)Ellipsoid.Modified_Fischer_1960].setValues(6378155, 6356752.3142, 0.006693422);
            ellipTable[(int)Ellipsoid.South_American_1969].setValues(6378160, 6356752.3142, 0.006694542);
            ellipTable[(int)Ellipsoid.WGS_60].setValues(6378165, 6356752.3142, 0.006693422);
            ellipTable[(int)Ellipsoid.WGS_66].setValues(6378145, 6356752.3142, 0.006694542);
            ellipTable[(int)Ellipsoid.WGS_72].setValues(6378135, 6356752.3142, 0.006694318);
            ellipTable[(int)Ellipsoid.WGS_84].setValues(6378137.0, 6356752.3142, 0.00669438);
        }

        private static EllipsoidConsts[] ellipTable;

        #endregion
        public const double deg2rad = Math.PI / 180.0;
        public const double rad2deg = 180.0 / Math.PI;

        public static void latLongtoUTM(double lat, double lon, ref double utmEasting, ref double utmNorthing, int zoneNumber)
        {
            EllipsoidConsts ellip = ellipTable[(int)Ellipsoid.WGS_84];

            double k0 = 0.9996;
            double longOrigin;
            double eccPrimeSquared;
            double N, T, C, A, M;

            //Make sure the longitude is between -180.00 .. 179.9
            double longTemp = (lon + 180.0) - (int)((lon + 180.0) / 360.0) * 360.0 - 180.0; // -180.00 .. 179.9;
            double latRad = lat * deg2rad;
            double longRad = longTemp * deg2rad;
            double longOriginRad;

            longOrigin = (zoneNumber - 1) * 6.0 - 180.0 + 3.0;  //+3 puts origin in middle of zone
            longOriginRad = longOrigin * deg2rad;

            eccPrimeSquared = (ellip.eccSquared) / (1.0 - ellip.eccSquared);

            N = ellip.a / Math.Sqrt(1.0 - ellip.eccSquared * Math.Sin(latRad) * Math.Sin(latRad));
            T = Math.Tan(latRad) * Math.Tan(latRad);
            C = eccPrimeSquared * Math.Cos(latRad) * Math.Cos(latRad);
            A = Math.Cos(latRad) * (longRad - longOriginRad);

            M = ellip.a * ((1.0 - ellip.eccSquared / 4.0 - 3.0 * ellip.eccSquared * ellip.eccSquared / 64.0 - 5.0 * ellip.eccSquared * ellip.eccSquared * ellip.eccSquared / 256.0) * latRad
              - (3.0 * ellip.eccSquared / 8.0 + 3.0 * ellip.eccSquared * ellip.eccSquared / 32 + 45.0 * ellip.eccSquared * ellip.eccSquared * ellip.eccSquared / 1024.0) * Math.Sin(2.0 * latRad)
              + (15.0 * ellip.eccSquared * ellip.eccSquared / 256.0 + 45.0 * ellip.eccSquared * ellip.eccSquared * ellip.eccSquared / 1024.0) * Math.Sin(4.0 * latRad)
              - (35.0 * ellip.eccSquared * ellip.eccSquared * ellip.eccSquared / 3072.0) * Math.Sin(6.0 * latRad));

            utmEasting = (k0 * N * (A + (1 - T + C) * A * A * A / 6
              + (5 - 18 * T + T * T + 72 * C - 58 * eccPrimeSquared) * A * A * A * A * A / 120.0)
              + 500000.0);

            utmNorthing = (k0 * (M + N * Math.Tan(latRad) * (A * A / 2.0 + (5 - T + 9 * C + 4 * C * C) * A * A * A * A / 24
              + (61.0 - 58.0 * T + T * T + 600.0 * C - 330.0 * eccPrimeSquared) * A * A * A * A * A * A / 720.0)));
            if (lat < 0)	//southern hemisphere
                utmNorthing += 10000000.0; //10000000 meter offset for southern hemisphere
        }


        /// <summary>
        ///	Converts UTM coords to lat/long.  Equations from USGS Bulletin 1532 
        /// East Longitudes are positive, West longitudes are negative. 
        /// North latitudes are positive, South latitudes are negative
        /// Lat and Long are in decimal degrees. 
        /// Written by Chuck Gantz- chuck.gantz@globalstar.com 
        /// </summary>
        /// <param name="utmNorthing">UTMs north (y) coordinate.</param>
        /// <param name="utmEasting">UTMs east (x) coordinat.</param>
        /// <param name="zo">The UTM zone in which the point to be calculated lie.</param>
        /// <param name="letter">The UTM letter.</param>
        /// <param name="lat">The latitude to be calculated</param>
        /// <param name="lon">The longitude to be calculated</param>
        public static void UTMtoLatLong(double utmEasting, double utmNorthing, int zo, char letter, ref double lat, ref double lon)
        {
            //EllipsoidConsts ellip = ellipTable[(int)ellipsoid];
            EllipsoidConsts ellip = ellipTable[(int)Ellipsoid.WGS_84];

            double k0 = 0.9996;
            double eccPrimeSquared;
            double e1 = (1 - Math.Sqrt(1 - ellip.eccSquared)) / (1 + Math.Sqrt(1 - ellip.eccSquared));
            double N1, T1, C1, R1, D, M;
            double longOrigin;
            double mu, phi1, phi1Rad;
            double x, y;
            int zoneNumber;

            x = utmEasting - 500000.0; //remove 500,000 meter offset for longitude
            y = utmNorthing;


            zoneNumber = zo;
            longOrigin = (zoneNumber - 1) * 6 - 180 + 3;  //+3 puts origin in middle of zone

            if ((letter - 'N') >= 0) { }//point is in northern hemisphere
            else
            {
                //point is in southern hemisphere
                y -= 10000000.0;		//remove 10,000,000 meter offset used for southern hemisphere
            }


            eccPrimeSquared = (ellip.eccSquared) / (1 - ellip.eccSquared);

            M = y / k0;
            mu = M / (ellip.a * (1 - ellip.eccSquared / 4 - 3 * ellip.eccSquared * ellip.eccSquared / 64 - 5 * ellip.eccSquared * ellip.eccSquared * ellip.eccSquared / 256));

            phi1Rad = mu + (3 * e1 / 2 - 27 * e1 * e1 * e1 / 32) * Math.Sin(2 * mu)
              + (21 * e1 * e1 / 16 - 55 * e1 * e1 * e1 * e1 / 32) * Math.Sin(4 * mu)
              + (151 * e1 * e1 * e1 / 96) * Math.Sin(6 * mu);
            phi1 = phi1Rad * rad2deg;

            N1 = ellip.a / Math.Sqrt(1 - ellip.eccSquared * Math.Sin(phi1Rad) * Math.Sin(phi1Rad));
            T1 = Math.Tan(phi1Rad) * Math.Tan(phi1Rad);
            C1 = eccPrimeSquared * Math.Cos(phi1Rad) * Math.Cos(phi1Rad);
            R1 = ellip.a * (1 - ellip.eccSquared) / Math.Pow(1 - ellip.eccSquared * Math.Sin(phi1Rad) * Math.Sin(phi1Rad), 1.5);
            D = x / (N1 * k0);

            lat = phi1Rad - (N1 * Math.Tan(phi1Rad) / R1) * (D * D / 2 - (5 + 3 * T1 + 10 * C1 - 4 * C1 * C1 - 9 * eccPrimeSquared) * D * D * D * D / 24
              + (61 + 90 * T1 + 298 * C1 + 45 * T1 * T1 - 252 * eccPrimeSquared - 3 * C1 * C1) * D * D * D * D * D * D / 720);
            lat = lat * rad2deg;

            lon = (D - (1 + 2 * T1 + C1) * D * D * D / 6 + (5 - 2 * C1 + 28 * T1 - 3 * C1 * C1 + 8 * eccPrimeSquared + 24 * T1 * T1)
              * D * D * D * D * D / 120) / Math.Cos(phi1Rad);
            lon = longOrigin + lon * rad2deg;
            if (lon < -180)
            {
                lon = 360 + lon;
            }
            if (lat > 90 || lat < -90 || lon < -180 || lon > 180)
            {
                lat = 0;
                lon = 0;
            }
        }
    }
}
