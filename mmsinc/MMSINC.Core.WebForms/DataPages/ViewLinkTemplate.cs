using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.DataPages
{
    public class ViewLinkField : TemplateField
    {
        private readonly string _primaryKeyFieldName;
        private readonly ViewLinkTemplate _template;
        private Guid _searchQueryKey;

        #region Properties

        public Guid SearchQueryKey
        {
            get { return _searchQueryKey; }
            set
            {
                _searchQueryKey = value;
                _template.SearchQueryKey = value;
            }
        }

        #endregion

        public ViewLinkField(string primaryKeyFieldName)
        {
            if (String.IsNullOrWhiteSpace(primaryKeyFieldName))
            {
                throw new ArgumentNullException("primaryKeyFieldName");
            }

            _primaryKeyFieldName = primaryKeyFieldName;
            _template = new ViewLinkTemplate(_primaryKeyFieldName);

            ItemTemplate = _template;
        }
    }

    internal class ViewLinkTemplate : ITemplate
    {
        private readonly string _primaryKeyFieldName;
        public Guid SearchQueryKey { get; set; }

        public ViewLinkTemplate(string primaryKeyFieldName)
        {
            if (String.IsNullOrWhiteSpace(primaryKeyFieldName))
            {
                throw new ArgumentNullException("primaryKeyFieldName");
            }

            _primaryKeyFieldName = primaryKeyFieldName;
        }

        public void InstantiateIn(Control container)
        {
            // Can't store instance because one template
            // will instantiate multiple controls. 
            var link = new DataPageViewRecordLink();
            link.LinkText = "View";
            link.DataBinding += OnLinkDataBinding;

            container.Controls.Add(link);
        }

        protected void OnLinkDataBinding(object sender, EventArgs e)
        {
            var link = (DataPageViewRecordLink)sender;
            // I don't like this because it's sloppy and there's no other
            // way to definitely know what type the NamingContainer is. 
            var grid = (IDataItemContainer)link.NamingContainer;
            link.ReturnSearchGuid = SearchQueryKey;
            link.DataRecordId = DataBinder.Eval(grid.DataItem, _primaryKeyFieldName).ToString();
        }
    }
}
