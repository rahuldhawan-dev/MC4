using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls
{
    /// <summary>
    /// Base class for creating search fields for DataPageBase pages. 
    /// DataField will get merged into this, and then probably blown up or something.
    /// 
    /// Or I might just make this implement IDataField and then have it
    /// so an event gets fired when AddFilterExpression gets called. 
    /// 
    /// </summary>
    [Obsolete("Stop using DataField and this")]
    public class TemplatedSearchField : PlaceHolder
    {
        #region Fields

        private string _label;

        #endregion

        #region Properties

        public string Label
        {
            get { return (_label ?? string.Empty); }
            set { _label = value; }
        }

        #endregion

        #region Private methods

        protected override void Render(HtmlTextWriter writer)
        {
            const string labelFormat = @"<td class=""label"">{0}</td>";
            writer.WriteLine("<tr>");
            writer.WriteLine(string.Format(labelFormat, this.Label));
            writer.WriteLine("<td class=\"field\">");
            base.Render(writer);
            writer.WriteLine("</td>");
            writer.WriteLine("</tr>");
        }

        #endregion
    }

   


}