#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.DataPages.Permissions;
using MMSINC.Utilities;
using MMSINC.Utilities.Auditing;
using MMSINC.Utilities.Permissions;

#endregion

namespace MMSINC.DataPages
{
    /// <remarks>
    /// This class is meant to replace the DataElementPage by providing a more basic and efficient implementation.
    /// 
    /// Instead of it working specifically with a DataElement control, and then having weird overrides for working
    /// with DetailsView controls, this requires that a second class be created to add the needed functionality.
    /// Ex: There'd need to be a DetailsViewDataPageBase and a DataElementDataPageBase. 
    /// 
    /// I've commented the ever living crap out of this. Don't hate me for it!
    /// 
    /// </remarks>
    public abstract class DataPageBase : MvpPage, IDataPageBase
    {
        // At the moment there's a mishmash of querystrings and ControlState that are storing
        // the same info, but they're both used depending on when there's a postback and 
        // when there's a redirect. Might be nice to get that a little bit combined. 

        // TODO: Add "edit" querystring. 
        // TODO: Add "delete" querystring.
        //       - Cause if both of these get added then we won't need to deal with ControlState anymore
        //       - Actually, no, a delete querystring would be terrible cause then someone could just
        //         start loading up values and deleting crap. So let's not!

        // TODO: Use Response.IsClientConnected to check if a transaction should rollback. This would be useful for preventing data
        //       being saved if a user hits the back button or otherwise regrets their decision.

        // TODO: The Search/Results/Details panels could really be set to IControl. We don't care what control it is here. 

        // TODO: Lets allow for multiple DataPagePermissions objects. We can have a main one that wraps around a
        //       collection of DataPagePermissions. This would allow for multiple roles. Then the wrapper would
        //       check for any of them equaling to true to come up with the culminating permission. 

        // CONTEMPLATE: Moving page redirection to RouteHelper, or make a RouteRedirecter class.

        // TODO TODO TODO: Searches with the LIKE operator don't necessarily bring back orders in the same
        //                  order every time. Need to add a default order by to the filter builder command.

        // TODO: Stop caching permissions so they can be modified later in the page cycle. 

        #region Enums

        private enum Panels
        {
            Search,
            List,
            Detail,
            Home
        }

        #endregion

        #region Private Members

        private IEnumerable<IDataLink> _dataLinkControls;
        private PageModes _pageMode;
        private AggregatedDataPagePermissions _permissions;
        private int _currentDataRecordId;
        private ViewLinkField _viewLinkColumn;
        private Guid _cachedFilterKey;

        #endregion

        #region Properties

        public virtual bool AutoAddDataKeyToGridView
        {
            get { return true; }
        }

        public virtual bool AutoGenerateViewColumnInResultsGridView
        {
            get { return true; }
        }

        // When working on mapping, add a check to see if PreviousPage is
        // an instance of this type and have it return the filter via
        // this property. There's no reason to viewstate it in a hidden input
        // when it's already being stored in ControlState here.
        public Guid CachedFilterKey
        {
            get { return _cachedFilterKey; }
            set
            {
                _cachedFilterKey = value;
                if (_viewLinkColumn != null)
                {
                    _viewLinkColumn.SearchQueryKey = value;
                }
            }
        }

        public IDataPagePath PathHelper { get; private set; }
        public IDataPageRenderHelper RenderHelper { get; protected set; }
        public IDataPageRouteHelper RouteHelper { get; protected set; }

        /// <summary>
        /// Gets/sets whether this page is capable of creating/editing data. 
        /// Defaults to false. Should set to true for things like reports pages. 
        /// </summary>
        public virtual bool IsReadOnlyPage { get; protected set; }

        public int ResultCount
        {
            get { return ResultsGridView.IRows.Count; }
        }

        /// <summary>
        /// Gets the page permissions object for this page.
        /// </summary>
        public virtual IDataPagePermissions Permissions
        {
            get { return PermissionsInternal; }
        }

        /// <summary>
        /// This is so we can expose some implementation details to inheritors.
        /// </summary>
        protected internal virtual AggregatedDataPagePermissions PermissionsInternal
        {
            get
            {
                if (_permissions == null)
                {
                    _permissions = CreatePermissions();
                }

                return _permissions;
            }
        }

        /// <summary>
        /// Gets the IDataLink controls that are dependent on this page's CurrentDataRecordId property.
        /// </summary>
        protected IEnumerable<IDataLink> DataLinkControls
        {
            get
            {
                if (_dataLinkControls == null)
                {
                    // Pass an empty array to this if GetIDataLinkControls returns null.
                    // This is to prevent null errors when iterating through this property.
                    _dataLinkControls = (GetIDataLinkControls() ?? new IDataLink[0]);
                }

                return _dataLinkControls;
            }
        }

        #region Abstract Properties

        protected abstract IButton SearchButton { get; }
        protected abstract IGridView ResultsGridView { get; }
        protected abstract IControl HomePanel { get; }
        protected abstract IControl DetailPanel { get; } // TODO: Change to IControl
        protected abstract IControl ResultsPanel { get; } // TODO: Change to IControl
        protected abstract IControl SearchPanel { get; } // TODO: Change to IControl
        protected abstract string DataElementTableName { get; }
        protected abstract string DataElementPrimaryFieldName { get; }
        protected abstract IModulePermissions ModulePermissions { get; }

        #endregion

        #region Protected Virtual Properties

        protected SqlDataSource ResultsDataSource
        {
            get
            {
                // TODO: Redo testing on this property. 

                var rgv = ResultsGridView;
                if (rgv == null)
                {
                    return null;
                }

                // This is only valid when a DataSource is set explicitly. 
                if (rgv.DataSource != null)
                {
                    return (SqlDataSource)rgv.DataSource;
                }

                // DataSourceObject is only set when DataSourceID is set.
                // NOTE: If a DataSource is set explicitly, this will return
                //       a ReadOnlyDataSource object instead. It's an internal
                //       type we don't have access to. 
                if (rgv.DataSourceObject != null)
                {
                    return (SqlDataSource)rgv.DataSourceObject;
                }

                throw new NullReferenceException("ResultsGridView has no accessible data source.");
            }
        }

        /// <summary>
        /// Sets the ID of the current data record. This gets passed down to any child controls
        /// that need it. Needs to be reset to 0 when the page is not in RecordReadOnly or RecordUpdate PageMode.
        /// </summary>
        internal protected virtual int CurrentDataRecordId
        {
            get { return _currentDataRecordId; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var oldId = _currentDataRecordId;
                _currentDataRecordId = value;

                // TODO: Add testing.
                OnCurrentDataRecordIdChanged(value, oldId);
            }
        }

        /// <summary>
        /// Gets whether any Documents/Notes/Things With DataLinkIDs should be visible. Default's to false. 
        /// </summary>
        protected bool DocNotesVisible { get; set; }

        /// <summary>
        /// Gets or sets what the current page mode is. If overriding, make sure to call the base.
        /// </summary>
        protected PageModes PageMode
        {
            get { return _pageMode; }
            set
            {
                _pageMode = value;
                OnPageModeChanged(_pageMode);
            }
        }

        /// <summary>
        /// Gets the default PageMode to use when a page first loads and there's no matching querystring.
        /// </summary>
        protected virtual PageModes DefaultPageMode
        {
            get { return PageModes.Home; }
        }

        #endregion

        #endregion

        #region Constructors

        public DataPageBase()
        {
            PathHelper = new DataPagePath(this);
            RenderHelper = new DataPageRenderHelper(this);
        }

        #endregion

        #region Private Methods

        #region Page Lifecycle

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            RouteHelper = CreateRouteHelper();

            // If the page already enables this, then
            // it needs to be disabled when rendering
            // out the gridview to excel file. 
            if (this.EnableEventValidation)
            {
                this.EnableEventValidation = !RouteHelper.IsExcelExportRoute;
            }

            //if (RouteHelper.IsExcelExportRoute)
            //{
            //    // Setting this to false doesn't work because
            //    // it throws an exception about needing to be done
            //    // prior to or during OnPreInit. So why it throws
            //    // during OnPreInit is beyond me. Leaving this
            //    // here as a reminder. 
            //    //this.EnableTheming = false;
            //    this.Theme = string.Empty; 
            //}
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // TODO: Add test that verifies this is called here. Any later and the 
            //       LoadControlState method will be called too late. 
            RegisterRequiresControlState(this);

            // This is disabled by default to reduce gigantic viewstates from
            // being created when users search for things returning thousands of
            // results. If viewstate is required, it can be reenabled by the inheriting page.
            ResultsGridView.EnableViewState = false;

            // Disable ViewState on the ResultsDataSource because asp parameters are stored
            // in ViewState by default. This causes a duplicate-parameter error to throw
            // when a FilterBuilder search is done twice in a row. It also breaks resetting
            // the search as the parameters will still be in there, even if you're trying
            // to remove them client-side. 
            ResultsDataSource.EnableViewState = false;

            // Setting the event handlers that are required. Doing it this way
            // to reduce having to put the handlers in each page's markup over 
            // and over. 

            if (SearchButton != null) SearchButton.Click += OnSearchButtonClicked;

            // Don't call any methods that need to DataBind here. Custom controls that have
            // GridViews/DetailViews/whatever that do binding during their own Init calls
            // won't have the proper values set since their Init fires before this Init. 
            //
            // TODO: Maybe throw an error if certain properties try to be set here? Like PageMode
            // shouldn't be changed until OnInitComplete. 

            if (AutoAddDataKeyToGridView)
            {
                SetDataKeyNamesOnResultsGridView();
            }

            // Columns would never be null except during testing.
            if (AutoGenerateViewColumnInResultsGridView && ResultsGridView.Columns != null)
            {
                _viewLinkColumn = new ViewLinkField(DataElementPrimaryFieldName);
                ResultsGridView.Columns.Insert(0, _viewLinkColumn);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);

            ValidateUserRights();
            ValidateDataLinkControls();

            // Run querystring stuff after all event listeners have been hooked 
            // up and after validation has occurred. Nothing should be called
            // after here if page access validation has failed. 

            // Checks for the ViewRecordQueryParameter and attempts to view that
            // record. This should only occur on initial page loads and can be 
            // ignored afterwards.
            if (!IsPostBack && !IsCallback)
            {
                InitializeRoute(this.RouteHelper);
            }
        }

        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(savedState);

            var cs = new ControlState(savedState);
            CachedFilterKey = cs.Get<Guid>("CachedFilterKey");
            PageMode = cs.Get<PageModes>("PageMode");
            CurrentDataRecordId = cs.Get<int>("CurrentDataRecordId");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var dnv = DocNotesVisible;

            foreach (var dLink in DataLinkControls)
            {
                dLink.Visible = dnv;
            }
        }

        /// <summary>
        /// This is sealed. Use SaveControlStateInternal to save additional values to the control state.
        /// </summary>
        /// <returns></returns>
        protected sealed override object SaveControlState()
        {
            var cs = new ControlState();
            SaveControlStateInternal(cs);

            return cs.GetControlStateObject();
        }

        /// <summary>
        /// Override this method to store values to ControlState properly. 
        /// </summary>
        /// <param name="state"></param>
        protected virtual void SaveControlStateInternal(ControlState state)
        {
            // I don't think CachedFilterKey needs to be persisted anymore since we changed
            // how search results are persisted. 
            state.Add("CachedFilterKey", CachedFilterKey, Guid.Empty);
            state.Add("PageMode", PageMode, PageModes.Search);
            state.Add("CurrentDataRecordId", CurrentDataRecordId, 0);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Method called when search expressions need to be added to a FilterBuilder. 
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AddExpressionsToFilterBuilder(IFilterBuilder builder) { }

        /// <summary>
        /// Returns a list of IDataLink controls the page uses. Ie: Documents, Notes, and Hyperlinks controls in MapCall.
        /// </summary>
        protected virtual IEnumerable<IDataLink> GetIDataLinkControls()
        {
            // For inheritors. 
            return null;
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Creates a new AggregatedDataPagePermissions object for this page to use.
        /// </summary>
        /// <returns></returns>
        protected virtual AggregatedDataPagePermissions CreatePermissions()
        {
            var a = new AggregatedDataPagePermissions();
            if (this.ModulePermissions != null)
            {
                a.Permissions.Add(new RoleBasedDataPagePermissions(this.ModulePermissions, this.IUser));
            }

            if (IsReadOnlyPage)
            {
                a.Permissions.Add(CreateReadOnlyPagePermissions());
            }

            return a;
        }

        protected virtual void OnCurrentDataRecordIdChanged(int newId, int oldId)
        {
            foreach (var dl in DataLinkControls)
            {
                dl.DataLinkID = newId;
            }
        }

        /// <summary>
        /// Method called prior to the record being inserted or updated.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRecordSaving(DataRecordSavingEventArgs e)
        {
            // Noop here.
        }

        /// <summary>
        /// Method called after a record has been inserted or updated.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRecordSaved(DataRecordSavedEventArgs e)
        {
            PerformAudit(e.SaveType, e.RecordId);

            switch (e.SaveType)
            {
                case DataRecordSaveTypes.Delete:
                    RedirectPageToDefault();
                    break;
                case DataRecordSaveTypes.Update:
                case DataRecordSaveTypes.Insert:
                    RedirectPageToRecord(e.RecordId);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        protected virtual void OnRecordDeleting(DetailsViewDeleteEventArgs e)
        {
            // Noop.
        }

        /// <summary>
        /// Method called when a data record needs to be loaded by the page's providing data source. 
        /// </summary>
        /// <param name="recordId"></param>
        protected virtual void LoadDataRecord(int recordId)
        {
            // There's an issue here with loading up records that don't
            // exist. Would lead to other controls(Notes/Docs) possibly
            // being allowed to add values for non-existant rows.

            CurrentDataRecordId = recordId;
            Audit(AuditCategory.DataView, recordId);
        }

        #region Auditing

        protected virtual void PerformAudit(DataRecordSaveTypes saveType, int recordId)
        {
            AuditCategory auditCat;

            switch (saveType)
            {
                case DataRecordSaveTypes.Insert:
                    auditCat = AuditCategory.DataInsert;
                    break;

                case DataRecordSaveTypes.Update:
                    auditCat = AuditCategory.DataUpdate;
                    break;

                case DataRecordSaveTypes.Delete:
                    auditCat = AuditCategory.DataDelete;
                    break;

                default:
                    throw new NotImplementedException();
            }

            Audit(auditCat, recordId);
        }

        internal protected virtual void Audit(AuditCategory category, int recordId)
        {
            string categoryName;

            switch (category)
            {
                case AuditCategory.DataView:
                    categoryName = "Viewed";
                    break;

                // Nothing uses AuditCategory.DataInsert 
                case AuditCategory.DataInsert:
                    categoryName = "Added";
                    break;

                case AuditCategory.DataUpdate:
                    categoryName = "Updated";
                    break;

                case AuditCategory.DataDelete:
                    categoryName = "Deleted";
                    break;

                default:
                    throw new NotSupportedException("Unsupported AuditCategory for use with recordId.");
            }

            Audit(category, String.Format("{0} {1} ID:{2}", categoryName, DataElementTableName, recordId));
        }

        protected virtual void Audit(AuditCategory category, string message)
        {
            var auditer = CreateAuditor();
            auditer.Insert(category, IUser.Name, message);
        }

        protected virtual IAuditor CreateAuditor()
        {
            var auditor = new Auditor();
            auditor.SqlConnectionString = ConfigurationManager.ConnectionStrings["MCProd"].ToString();
            return auditor;
        }

        #endregion

        protected virtual void OnPageModeChanged(PageModes newMode)
        {
            Permissions.PageAccess.Demand();

            // TODO: Not sure how to test that Permissions.Demand threw since
            // the tests require the mocked version. 

            switch (newMode)
            {
                case PageModes.Home:
                    SetVisible(Panels.Home);
                    CurrentDataRecordId = 0;
                    DocNotesVisible = false;

                    break;
                case PageModes.Search:
                    SetVisible(Panels.Search);
                    CurrentDataRecordId = 0;
                    DocNotesVisible = false;

                    break;

                case PageModes.Results:
                    SetVisible(Panels.List);
                    CurrentDataRecordId = 0;
                    DocNotesVisible = false;
                    ApplyFilter();

                    break;

                case PageModes.RecordReadOnly:
                    SetVisible(Panels.Detail);
                    DocNotesVisible = true;

                    break;

                case PageModes.RecordUpdate:
                    Permissions.EditAccess.Demand();
                    SetVisible(Panels.Detail);
                    DocNotesVisible = false;

                    break;

                case PageModes.RecordInsert:
                    Permissions.CreateAccess.Demand();
                    SetVisible(Panels.Detail);
                    CurrentDataRecordId = 0;
                    DocNotesVisible = false;

                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Method called on InitComplete that performs permission/authorization right checks. 
        /// </summary>
        protected virtual void ValidateUserRights()
        {
            var p = Permissions;

            if (!p.PageAccess.IsAllowed)
            {
                IResponse.Write("Access Denied - " + p.PermissionName);
                IResponse.End();
            }
        }

        protected virtual void ValidateDataLinkControls()
        {
            var p = Permissions;
            var canAdd = p.CreateAccess.IsAllowed;
            var canEdit = p.EditAccess.IsAllowed;
            var canDelete = p.DeleteAccess.IsAllowed;

            foreach (var dLink in DataLinkControls)
            {
                dLink.AllowAdd = canAdd;
                dLink.AllowEdit = canEdit;
                dLink.AllowDelete = canDelete;
            }
        }

        protected IDataPagePermissions CreateReadOnlyPagePermissions()
        {
            var p = new DataPagePermissions("ReadOnly Page");
            p.AdminAccess = new Permission("ReadOnly Page Admin") {Deny = true};
            p.DeleteAccess = new Permission("ReadOnly Page Delete") {Deny = true};
            p.EditAccess = new Permission("ReadOnly Page Edit") {Deny = true};
            p.CreateAccess = new Permission("ReadOnly Page Create") {Deny = true};
            return p;
        }

        protected virtual string RenderResultsGridViewToExcel()
        {
            return RenderGridViewToExcel(ResultsGridView);
        }

        protected virtual string RenderGridViewToExcel(IGridView gv)
        {
            //  gv.AllowSorting = false;
            // Hides the "View" fields since they're useless in an Excel sheet. 
            foreach (DataControlField col in gv.Columns)
            {
                col.SortExpression = "";
                if (col is CommandField)
                {
                    col.Visible = false;
                }

                if (col is ViewLinkField)
                {
                    col.Visible = false;
                }
            }

            using (var sw = new StringWriter())
            {
                using (var htw = new HtmlTextWriter(sw))
                {
                    gv.RenderControl(htw);
                }

                return sw.ToString();
            }
        }

        #region ParseQueryString related methods

        internal virtual IDataPageRouteHelper CreateRouteHelper()
        {
            return new DataPageRouteHelper(this);
        }

        protected void InitializeRoute(IDataPageRouteHelper routeHelp)
        {
            // NOTE: These should only be checked if IsPostBack = false.
            // OTHER NOTE: I feel this is kinda flakey. 

            if (routeHelp == null)
            {
                throw new ArgumentNullException("routeHelp");
            }

            if (!routeHelp.HasKnownRoute)
            {
                PageMode = DefaultPageMode;
            }
            else
            {
                if (routeHelp.IsHomeRoute)
                {
                    PageMode = PageModes.Home;
                }
                else if (routeHelp.IsCreateRoute)
                {
                    PageMode = PageModes.RecordInsert;
                }
                else if (routeHelp.IsSearchRoute)
                {
                    PageMode = PageModes.Search;
                }
                else
                {
                    CachedFilterKey = routeHelp.SearchKey;

                    if (routeHelp.IsResultsRoute)
                    {
                        PageMode = PageModes.Results;
                    }
                    else if (routeHelp.IsExcelExportRoute)
                    {
                        PageMode = PageModes.Results;
                        ApplyFilter(); // Otherwise the excel sheet will just have every row.
                        ExcelExport();
                    }
                    else if (routeHelp.IsViewRoute)
                    {
                        PageMode = PageModes.RecordReadOnly;
                        LoadDataRecord(routeHelp.ViewRecordId);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Filters

        /// <summary>
        /// Call this when the DataPageBase should perform a search based on DataFields and what not.
        /// </summary>
        internal protected virtual void Search()
        {
            var filter = CreateFilter();
            Search(filter);
        }

        /// <summary>
        /// Call this when the DataPageBase should perform a search using any arbitrary IFilterBuilder.
        /// </summary>
        /// <param name="fb"></param>
        internal protected virtual void Search(IFilterBuilder fb)
        {
            if (fb == null)
            {
                throw new NullReferenceException("filterbuilder");
            }

            // Create a new guid for the new filter builder, can't reuse existing ones. 
            CachedFilterKey = GetFilterCache().AddFilterBuilderToCache(fb);
            RedirectPageToSearchResults(CachedFilterKey);
        }

        internal virtual IFilterCache GetFilterCache()
        {
            return new FilterCache();
        }

        /// <summary>
        /// Override if you need to change how the cached FilterBuilder gets retrieved. 
        /// If the DefaultPageMode is set to Results, an infinite redirect loop will
        /// occur that blows everything up. This is really hacky and should be fixed instead
        /// of having LookUpDataPageBase overriding this.
        /// </summary>
        /// <returns></returns>
        internal protected virtual IFilterBuilder GetCachedFilterBuilder()
        {
            var fb = GetFilterCache().GetFilterBuilder(CachedFilterKey);

            // The filter will always be null if the DefaultPageMode is set to Results, because
            // no search will have actually occurred.
            if (fb == null && DefaultPageMode == PageModes.Results && PageMode == PageModes.Results)
            {
                fb = CreateFilterBuilder();
            }

            return fb;
        }

        protected virtual IFilterBuilder CreateFilterBuilder()
        {
            var fb = new FilterBuilder();
            InitializeFilterBuilder(fb);
            return fb;
        }

        internal void InitializeFilterBuilder(IFilterBuilder fb)
        {
            fb.ConnectionString = ResultsDataSource.ConnectionString;
            fb.SelectCommand = ResultsDataSource.SelectCommand;
            fb.OriginatingPageUrl = IRequest.Url;
        }

        /// <remarks>
        /// Creates and populates a FilterBuilder object to be used with the page's current search parameters.
        ///
        /// This method should be called only when submitting a search.
        /// 
        /// After creating the FilterBuilder, the instance is cached in the HttpContext's cache. This makes it safe
        /// to store various sql info without having to persist it client-side. It also makes it a lot easier for
        /// the old Maps page to grab its needed filter information instead of the hacky ViewState method of doing things
        /// currently. Might make sense to pass the Maps page the guid in the query string and go from there. Ross -12/30/2010
        /// </remarks>
        internal protected virtual IFilterBuilder CreateFilter()
        {
            // TODO: There needs to be a way to append extra information at the end of the
            //       WHERE filter, like "Order By" stuff. Alex suggested a custom SqlDataSource
            //       control that has an additional OrderBy property. It would have to be tested
            //       against passing the full select statement around to pages like Maps. 

            var fb = CreateFilterBuilder();
            AddExpressionsToFilterBuilder(fb);

            return fb;
        }

        // This internal only for testing purposes. 
        internal virtual void ApplyFilter()
        {
            var fb = GetCachedFilterBuilder();
            if (fb == null)
            {
                // It means our search results cache has expired or doesn't exist otherwise.
                // We wanna redirect so they don't end up doing a search that returns back all 
                // the table results.
                // TODO: Probably be nice to tell the user their search expired.
                RedirectPageToSearch();
            }
            else
            {
                if (fb.OriginatingPageUrl != IRequest.Url)
                {
                    // Otherwise sql exceptions will be thrown
                    RedirectPageToSearch();
                }
                else
                {
                    ApplyFilterBuilder(fb);
                }
            }
        }

        /// <summary>
        /// Called when a FilterBuilder needs to be applied. 
        /// </summary>
        /// <param name="fb"></param>
        internal protected virtual void ApplyFilterBuilder(IFilterBuilder fb)
        {
            if (fb == null)
            {
                throw new ArgumentNullException("fb");
            }

            // BuildCompleteCommand will always have the original SelectCommand at the beginning, so it
            // should never ever be null. 
            ResultsDataSource.SelectCommand = fb.BuildCompleteCommand();

            // Watch out here if ApplyFilter ever needs to be called
            // more than once. A duplicate parameter error will be
            // thrown.
            // TODO: Can we just overwrite them instead and avoid this whole deal?
            foreach (var p in fb.BuildParameters())
            {
                ResultsDataSource.SelectParameters[p.Name] = p;
                //ResultsDataSource.SelectParameters.Add(p);
            }

            // This needs to be called or else the grid won't update with the proper search parameters
            // on successive postbacks.

            if (PageMode == PageModes.Results)
            {
                ResultsGridView.DataBind();
            }
        }

        #endregion

        #region Redirect Methods

        // NOTE: ALL Redirects must have the endResponse parameter set to true!
        //       Otherwise page processing will continue and who knows what sort
        //       of bugs will arise there. It'll throw a ThreadAbortException. Deal with it.

        protected virtual void RedirectPage(string url)
        {
            // We want to end the response or else it's going to finish off any calls
            // that might come after, like saving data.
            IResponse.Redirect(url, true);
        }

        protected void RedirectPageToSearch()
        {
            var query = new Dictionary<string, object>();
            query.Add(DataPageUtility.QUERY.SEARCH, string.Empty);

            var url = QueryStringHelper.BuildFromDictionary(PathHelper.GetBaseUrl(), query);
            RedirectPage(url);
        }

        private void RedirectPageToSearchResults(Guid key)
        {
            var query = new Dictionary<string, object>();
            query.Add(DataPageUtility.QUERY.SEARCH, key.ToString());

            var url = QueryStringHelper.BuildFromDictionary(PathHelper.GetBaseUrl(), query);
            RedirectPage(url);
        }

        protected void RedirectPageToRecord(int recordId)
        {
            var query = new Dictionary<string, object>();
            query.Add(DataPageUtility.QUERY.VIEW, recordId.ToString());
            if (CachedFilterKey != Guid.Empty)
            {
                query.Add(DataPageUtility.QUERY.SEARCH, CachedFilterKey.ToString());
            }

            var redirectUrl = QueryStringHelper.BuildFromDictionary(PathHelper.GetBaseUrl(), query);

            // Redirect the page so that reloading doesn't cause it to re-insert/re-update.
            // IRequest.Url returns the root Url without any querystrings.
            RedirectPage(redirectUrl);
        }

        /// <summary>
        /// Basically, it renavigates to itself. Gets rid of the postback and any querystring stuff. 
        /// </summary>
        private void RedirectPageToDefault()
        {
            RedirectPage(IRequest.Url);
        }

        #endregion

        private void SetDataKeyNamesOnResultsGridView()
        {
            ResultsGridView.DataKeyNames = AddPrimaryKeyToDataKeyNames(ResultsGridView.DataKeyNames);
        }

        internal protected string[] AddPrimaryKeyToDataKeyNames(string[] currentNames)
        {
            var field = DataElementPrimaryFieldName;
            if (currentNames != null && !currentNames.Contains(field))
            {
                // If for some reason the DataKeyNames do not have the primary key
                // we need to add it to that array. 
                var newDataKeyNames = new List<string>();
                newDataKeyNames.AddRange(currentNames);
                newDataKeyNames.Add(field);
                return newDataKeyNames.ToArray();
            }

            return new[] {field};
        }

        private void ExcelExport()
        {
            IResponse.Clear();
            IResponse.AddHeader("content-disposition", "attachment;filename=Data.xls");
            IResponse.Write(RenderResultsGridViewToExcel());
            IResponse.End();
        }

        private void SetVisible(Panels pnl)
        {
            // Doing a null check because in some situations
            // each panel may not be necessary. 

            if (SearchPanel != null) SearchPanel.Visible = (pnl == Panels.Search);
            if (ResultsPanel != null) ResultsPanel.Visible = (pnl == Panels.List);
            if (DetailPanel != null) DetailPanel.Visible = (pnl == Panels.Detail);
            if (HomePanel != null) HomePanel.Visible = (pnl == Panels.Home);
        }

        #endregion

        #region Event Handlers

        protected virtual void OnSearchButtonClicked(object sender, EventArgs e)
        {
            Search();
        }

        #endregion

        #region Exposed Methods

        public override void VerifyRenderingInServerForm(Control control)
        {
            // This override is needed for rendering the ResultsGridView
            // without throwing an exception
            return;
        }

        #endregion
    }
}
