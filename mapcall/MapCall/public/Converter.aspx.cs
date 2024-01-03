using System;
using System.Web.UI;
using MMSINC.GIS;

namespace MapCall.public1
{
	public partial class Converter : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
        protected void btnConvertLL_Click(object sender, EventArgs e)
		{
            double lat = 0.0, lon = 0.0;
            PositionConv.UTMtoLatLong(double.Parse(txtEasting.Text) * 3.2808, double.Parse(txtNorthing.Text) * 3.2808, 18, 'N', ref lat, ref lon);
            txtLat.Text = lat.ToString();
            txtLon.Text = lon.ToString();
		}
        protected void btnConvertNE_Click(object sender, EventArgs e)
        {
            double northing = 0.0, easting = 0.0;
            PositionConv.latLongtoUTM(double.Parse(txtLat.Text), double.Parse(txtLon.Text), ref easting, ref northing, 18);
            txtEasting.Text = easting.ToString();
            txtNorthing.Text = northing.ToString();
        }
	}
}
