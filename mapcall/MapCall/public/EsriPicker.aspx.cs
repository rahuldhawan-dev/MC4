using System.Web.Mvc;
using System.Web.UI;
using MapCall.Common.Controls;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.NHibernate;
using StructureMap;

namespace MapCall.public1
{
	public partial class EsriPicker : AssetLatLonPage
	{
	    protected string GetDefaultIconImage()
	    {
            const int squareSize = 40;

            var icon = DependencyResolver.Current.GetService<IconSetRepository>().GetDefaultIconSet(DependencyResolver.Current.GetService<IRepository<MapIcon>>()).DefaultIcon;
            var height = icon.Height > squareSize ? squareSize : icon.Height;
            var width = icon.Width > squareSize ? squareSize : icon.Width;
            var style = string.Format("height: {0}px; width: {1}px; visibility:hidden;", height, width);
            var url = ResolveUrl("~/images/" + icon.FileName);

            return string.Format("<img src=\"{0}\" style=\"{1}\" data-default-icon=\"true\" data-icon-offset=\"{2}\" />", url, style, icon.Offset.Description);
	    }

	    protected void Page_Load()
	    {
            DataBind();
	    }
	}
}