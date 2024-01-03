using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using MMSINC.Controls;
using MapCall.Controls.SearchFields;

namespace MapCall.Controls
{
    // TODO: Columns should go top bottom, not left to right.

    public interface ISearchBox
    {
        Collection<BaseSearchField> Fields { get; }
        int NumberOfColumns { get; set; }
    }

    /// <summary>
    /// Control that deals with rendering a collection of SearchFields
    /// </summary>
    [ParseChildren(true)]
    public class SearchBox : MvpPlaceHolder, ISearchBox
    {
        #region Constants

        private const int MINIMUM_NUMBER_OF_COLUMNS = 1;

        #endregion

        #region Fields

        private readonly Collection<BaseSearchField> _fields = new Collection<BaseSearchField>();
        private readonly Dictionary<ISearchField, Control> _fieldTemplates = new Dictionary<ISearchField, Control>();
        private int _numberOfColumns = 1;

        #endregion

        #region Properties

        public string CssClass { get; set; }

        /// <remarks>
        /// This has to be a Collection, and it has to be BaseSearchField(not ISearchField)
        /// in order for us to be able to add fields in the designer. It will not work otherwise.
        /// </remarks>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Collection<BaseSearchField> Fields
        {
            get { return _fields; }
        }

        public int NumberOfColumns
        {
            get { return _numberOfColumns; }
            set
            {
                if (value < MINIMUM_NUMBER_OF_COLUMNS)
                {
                    throw new ArgumentOutOfRangeException("value", "SearchBox.NumberOfColumns has to be atleast 1. How else would it render columns?");
                }
                _numberOfColumns = value;
            }
        }

        #endregion

        #region Private Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            foreach (var f in Fields)
            {
                var template = new MvpPlaceHolder();
                // The template has to be added to our controls
                // *before* calling the field initializer. Otherwise
                // the DateTimeSearchField can't use the Page.LoadControl
                // method(Page will be null). 
                Controls.Add(template);
                _fieldTemplates.Add(f, template);
                f.AddControlsToTemplate(template);
            }

        }

        protected override void Render(HtmlTextWriter writer)
        {
            // NOTE: DO NOT CALL BASE! Otherwise all the fields will render twice! 
            if (!string.IsNullOrWhiteSpace(CssClass))
            {
                writer.AddAttribute("class", CssClass);
            }
            // Write attributes and crap
            writer.RenderBeginTag("table");
          
            foreach (var row in GetRows())
            {
                writer.WriteLine("<tr>");

                foreach (var field in row)
                {
                    RenderField(writer, field);
                }
         
                writer.WriteLine("</tr>");
            }

            writer.RenderEndTag();
        }

        // Call to render a field without needing to know information about its 
        // containing column.
        protected void RenderField(HtmlTextWriter writer, BaseSearchField field)
        {
            writer.Write(@"<td class=""label"">");
            writer.Write(GetOutputLabel(field.Label));
            if (!string.IsNullOrWhiteSpace(field.HelpText))
            {
                writer.Write(@"<span class=""help"">");
                writer.Write(field.HelpText);
                writer.Write("</span>");
            }

            writer.WriteLine(@"</td>");
            writer.WriteLine("<td class=\"field\">");
            _fieldTemplates[field].RenderControl(writer);
            writer.WriteLine("</td>");
        }

        /// <summary>
        /// Returns the fields divided into the number of columns needed. 
        /// Each collection should be one row in the table. 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IEnumerable<BaseSearchField>> GetRows()
        {
            // Copied this from here:
            // http://stackoverflow.com/questions/438188/split-a-collection-into-n-parts-with-linq
            return GetVisibleFields().Select((value, index) => new { Index = index, Value = value })
                         .GroupBy(i => i.Index / NumberOfColumns)
                         .Select(i => i.Select(i2 => i2.Value));
        }

        private IEnumerable<BaseSearchField> GetVisibleFields()
        {
            return (from f in Fields where f.Visible select f);
        }

        private static string GetOutputLabel(string label)
        {
            if (label != string.Empty) // Don't want no &nbsp; !
            {
                label = HttpUtility.HtmlEncode(label);
            }
            return label;
        }


        #endregion
    }
}