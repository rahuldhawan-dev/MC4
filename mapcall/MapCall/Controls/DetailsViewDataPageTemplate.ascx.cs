using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.DataPages;
using MMSINC.Utilities;
using MapCall.Common;

namespace MapCall.Controls
{

    // You know what'd be pretty sweet?
    // Using the same SqlDataSource/BoundFields for
    // both the ResultsView and the DetailsView so it only has
    // to be set once. Then it can be overridable as needed. 


    // TODO: Create a "Details" class so that the template markup can be logically organized a little better. 

    [ParseChildren(true)]
    public partial class DetailsViewDataPageTemplate : MvpUserControl
    {
        #region Fields

        private IDataPageBase _dataPageParent;
        private int _dataTypeId;
        private string _label;
        private IEnumerable<IDataLink> _iDataLinks;

        #endregion

        #region Control Declarations

        protected internal HR.Employees Employee1;

        #endregion

        #region Properties

        protected IDataPageRenderHelper RenderHelper
        {
            get
            {
                return _dataPageParent.RenderHelper;
            }
        }

        protected IDataPagePath PathHelper { get { return _dataPageParent.PathHelper; } }

        public string DataElementTableName { get; set; }
        public string DataElementPrimaryFieldName { get; set; }
        public int DataTypeId
        {
            get { return _dataTypeId; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "DataTypeID must be a value greater than or equal to 0. Set to 0 if there is no DataType.");
                }
                _dataTypeId = value;
            }
        }
        public PageModes DefaultPageMode { get; set; }

        public bool IsReadOnlyPage { get; set; }
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                tabView.FindTabById("details").Label = value;
            }
        }


        public int ResultCount { get { return _dataPageParent.ResultCount; } }

        public bool ShowMapButton { get; set; }

        public bool ShowEmployees { get; set; }

        #region Control reference properties

        public MvpPanel DetailsPanel { get { return pnlDetails; } }
        public MvpPanel ResultsPanel { get { return pnlResults; } }
        public MvpPanel SearchPanel { get { return pnlSearch; } }
        public MvpPlaceHolder HomePanel { get { return placeHome; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder SearchHelpPlaceHolder { get { return phSearchHelp; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public SearchBox SearchBox { get { return searchBox; } }

        /// <summary>
        /// Gets the default Tab that the DetailsView is inside of. 
        /// </summary>
        public Tab DetailsTab { get { return details; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder DetailsViewPlaceHolder { get { return phDetailsView; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public MvpPlaceHolder HomePlaceHolder { get { return phHome; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder ResultsPlaceHolder { get { return phResultsPlaceHolder; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder ResultsButtons { get { return phResultsButtons; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder DetailsButtonsLeft { get { return phDetailsButtonsLeft; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public MvpDetailsView DetailsView { get { return detailView; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public SqlDataSource DetailsDataSource { get { return detailDataSource; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public SqlDataSource ResultsDataSource { get { return resDataSource; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public MvpGridView ResultsGridView { get { return resultsGrid; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public MvpButton SearchButton { get { return btnSearch; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder SearchButtonsRight { get { return phSearchButtonsRight; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Collection<Tab> Tabs
        {
            get { return tabView.Tabs; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Label ErrorMessageLabel { get { return lblErrorMessage; } }

        #endregion

        #endregion

        #region Constructors

        public DetailsViewDataPageTemplate()
        {
            // Set to Search by default as to not scare the good people of AMWater.
            this.DefaultPageMode = PageModes.Search;
        }

        #endregion

        #region Private Methods


        protected string GetBaseUrl()
        {
            return _dataPageParent.PathHelper.GetBaseUrl();
        }

        private string GetMapsUrl()
        {
            return ResolveUrl("~/Modules/Maps/Maps.aspx");
        }

        protected string RenderMapLinkButton()
        {
            if (ShowMapButton)
            {
                var mapUrl = QueryStringHelper.BuildFromKeyValuePair(GetMapsUrl(), DataPageUtility.QUERY.SEARCH, _dataPageParent.CachedFilterKey.ToString());
                return RenderHelper.RenderLinkButton(mapUrl, "Map");
            }
            return string.Empty;
        }

#pragma warning disable 612,618
        // This is suppressing the "Page is obsolete" error.
        // I need to use Page, not the PageWrapper. 

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!(Page is IDataPageBase))
            {
                throw new NotSupportedException();
            }

            _dataPageParent = (IDataPageBase)Page;
            SetupIDataLinkControls();
            ReorderNotesDocsTabs();
        }
#pragma warning restore 612,618

        // This is so they get set as the last two tabs, to keep
        // with the old way they rendered. This only orders the *tabs*
        // not the rendering of their control containers. 
        private void ReorderNotesDocsTabs()
        {
            Tabs.Remove(notes);
            Tabs.Add(notes);
            Tabs.Remove(documents);
            Tabs.Add(documents);
        }

        private void SetupIDataLinkControls()
        {
            if (DataTypeId > 0)
            {
                _iDataLinks = new IDataLink[] {Notes1, Documents1, Employee1};
                Notes1.DataTypeID = DataTypeId;
                Documents1.DataTypeID = DataTypeId;
                Employee1.DataTypeID = DataTypeId;
                employees.Visible = ShowEmployees;
            }
            else
            {
                notes.Visible = false;
                documents.Visible = false;
                employees.Visible = false;
            }
        }

        #endregion

        #region Public Methods

        public void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            if (SearchBox != null)
            {
                foreach (var field in SearchBox.Fields)
                {
                    field.FilterExpression(builder);
                }
            }
        }

        public IEnumerable<IDataLink> GetIDataLinkControls()
        {
            return _iDataLinks;
        }

        #endregion
    }
}