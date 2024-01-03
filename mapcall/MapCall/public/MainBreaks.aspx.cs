using System;
using System.Data;
using System.Text;
using System.Web.UI;

namespace MapCall.Argh
{
	public partial class MainBreaks : Page
    {
        #region Constants

	    private const string ENTRY_FORMATSTRING =
	        "<entry><title>{0}</title><link href=\"https://mapcall.amwater.com/wo.aspx?ID={1}\"></link><id>https://mapcall.amwater.com/wo.aspx?ID={1}</id><updated>{2:s}Z</updated><summary>{3}</summary><georss:point>{4} {5}</georss:point></entry>";
        private const string ENTRY_DESCRIPTION_FORMATSTRING = "Work Description: {2} &lt;br/&gt;Date Received: {0:d} &lt;br/&gt; Date Completed: {1:d} &lt;br/&gt; ";

        private const string ENTRY_DESCRIPTION_FORMATSTRING_W3C = "<item><pubDate>{0:r}</pubDate><title>{1}</title><description>{2}</description><link>{3}</link><geo:lat>{4}</geo:lat><geo:long>{5}</geo:long><dc:subject>{6}</dc:subject><dc:subject>{7}</dc:subject><dc:subject>{8}</dc:subject><guid isPermaLink=\"false\">{9}</guid></item>\n";
	    private const string ENTRY_LINK = "https://mapcall.amwater.com/wo.aspx?ID={0}";
        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            //SimpleGeoRSS();
            W3CGeoRSS();
        }

        #endregion

        #region Private Methods
        public void W3CGeoRSS()
        {
            Response.Clear();
            Response.ContentType = "application/xml";
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\"?>\n");
            sb.Append("<?xml-stylesheet href=\"/public/mainbreaks.aspx\" type=\"text/xsl\" media=\"screen\"?>\n");
            sb.Append("<rss version=\"2.0\" xmlns:geo=\"http://www.w3.org/2003/01/geo/wgs84_pos#\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\">\n");
            sb.Append("<channel>\n");
            sb.Append("<title>Work Orders - Main Breaks</title>\n");
            sb.Append("<description>Main Breaks from MapCall.net</description>\n");
            sb.Append("<link>http://mapcall.amwater.com</link>\n");
            sb.Append("<dc:publisher>MapCall</dc:publisher>\n");
            sb.AppendFormat("<pubDate>{0:r}</pubDate>\n", DateTime.Now);

            var dv =
                (DataView)dsWorkOrders.Select(DataSourceSelectArguments.Empty);
            foreach (DataRow dr in dv.Table.Rows)
            {
                sb.AppendFormat(ENTRY_DESCRIPTION_FORMATSTRING_W3C,
                    DateTime.Now,   //pubDate
                    string.Format("WorkOrderID: {0}",dr[0]),          //title
                    dr[5],          //descript
                    string.Format(ENTRY_LINK, dr[0]),          //link
                    dr[3],          //lat
                    dr[4],          //lon
                    dr[5],          //subject
                    "",          //subject
                    "",          //subject
                    dr[0]           //guid
                );
            }
            sb.Append("</channel>\n");
            sb.Append("</rss>\n");

            Response.Write(sb.ToString());
            Response.End();    
        }

        public void SimpleGeoRSS()
        {
            Response.Clear();
            Response.ContentType = "application/xml";
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append(
                "<feed xmlns=\"http://www.w3.org/2005/Atom\" xmlns:georss=\"http://www.georss.org/georss\" xml:lang=\"en\">");
            sb.Append("<title>Work Orders</title>");
            sb.Append("<subtitle>Main Breaks</subtitle>");
            sb.Append("<link href=\"http://mapcall.amwater.com/\"/>    ");
            sb.AppendFormat("<updated>{0:s}Z</updated>", DateTime.Now);
            sb.Append("<author>");
            sb.Append("<name>MapCall.net</name>");
            sb.Append("<email></email>");
            sb.Append("</author>");
            sb.Append("<id>http://mapcall.amwater.com/</id>");
            //sb.AppendFormat(ENTRY_FORMATSTRING, Title, WorkOrderID, UpdatedOn, SummaryText, Lat, Lon)
            var dv =
                (DataView)dsWorkOrders.Select(DataSourceSelectArguments.Empty);
            foreach (DataRow dr in dv.Table.Rows)
            {
                sb.AppendFormat(ENTRY_FORMATSTRING,
                    dr[5],
                    dr[0],
                    DateTime.Now,
                    string.Format(ENTRY_DESCRIPTION_FORMATSTRING,
                        dr[1], dr[2], dr[5]
                    ),
                    dr[3],
                    dr[4]
                );
            }
            sb.Append("</feed>");
            Response.Write(sb.ToString());
            Response.End();
        }
        #endregion
    }
}
