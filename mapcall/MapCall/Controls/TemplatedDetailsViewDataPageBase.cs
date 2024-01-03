using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.DataPages;
using MapCall.Common;
using MapCall.Controls.Data;

namespace MapCall.Controls
{
    // TODO: This should automatically delete Documents/Notes attached to a record when a
    //       record is deleted. Orphans! BOOOO!

    /// <summary>
    /// DetailsViewDataPageBase class that's used with the DetailsViewDataPageTemplate control.
    /// </summary>
    public abstract class TemplatedDetailsViewDataPageBase : DetailsViewDataPageBase
    {
        #region Properties

        protected abstract DetailsViewDataPageTemplate Template { get; }

        public override string Theme
        {
            get
            {
                // Overriding this so we don't have to set this in markup all the time. Also
                // be easier to change the theme for all pages in one spot if necessary.
                return "bender";
            }
            set
            {
                base.Theme = value;
            }
        }

        #region From DetailsViewDataPageBase

        protected override PageModes DefaultPageMode
        {
            get
            {
                return Template.DefaultPageMode;
            }
        }


        protected override string DataElementTableName
        {
            get { return Template.DataElementTableName; }
        }

        protected override string DataElementPrimaryFieldName
        {
            get { return Template.DataElementPrimaryFieldName; }
        }

        public override bool IsReadOnlyPage
        {
            get
            {
                return Template.IsReadOnlyPage;
            }
            protected set
            {
                base.IsReadOnlyPage = value;
            }
        }

        protected override IDetailsView DetailsView
        {
            get { return Template.DetailsView; }
        }

        protected override IButton SearchButton
        {
            get { return Template.SearchButton; }
        }

        protected override IGridView ResultsGridView
        {
            get { return Template.ResultsGridView; }
        }

        protected override IControl DetailPanel
        {
            get { return Template.DetailsPanel; }
        }

        protected override IControl HomePanel
        {
            get { return Template.HomePanel; }
        }

        protected override IControl ResultsPanel
        {
            get { return Template.ResultsPanel; }
        }

        protected override IControl SearchPanel
        {
            get { return Template.SearchPanel; }
        }

        #endregion

        #endregion

        #region Private Methods

        protected override void AddExpressionsToFilterBuilder(IFilterBuilder builder)
        {
            base.AddExpressionsToFilterBuilder(builder);

            // Automatically gets any IDataField controls and adds their
            // FilterBuilderExpressions. 
            Template.AddExpressionsToFilterBuilder(builder);

        }

        protected override IEnumerable<IDataLink> GetIDataLinkControls()
        {
            var controls = Template.GetIDataLinkControls();
            var baseControls = base.GetIDataLinkControls();

            if (baseControls != null)
            {
                controls = baseControls.Union(controls);
            }
            return controls;

        }

        private static void HideTabs(IEnumerable<Tab> tabs)
        {
            if (tabs != null)
            {
                foreach (var t in tabs) { t.Visible = false; }
            }
        }

        private void SetTabVisibility()
        {
            // This is to hide the notes/docs/other tabs in situations where they aren't editable
            // but are still visible. If other tabs become necessary we'll 
            var noData = (Template.DetailsView.DataItemCount == 0);
            var isInserting = (PageMode == PageModes.RecordInsert);
            var isUpdating = (PageMode == PageModes.RecordUpdate);
            var isEditing = (isInserting || isUpdating);

            IEnumerable<Tab> tabs = null;

            if (noData && !isEditing)
            {
                tabs = Template.Tabs;
            }
            else if (isInserting)
            {
                tabs = (from t in Template.Tabs where !t.VisibleDuringInsert select t);
            }
            else if (isUpdating)
            {
                tabs = (from t in Template.Tabs where !t.VisibleDuringUpdate select t);
            }

            HideTabs(tabs);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetTabVisibility();
        }

        internal MultiCheckBoxList FindCheckBoxList(string checkBoxListId)
        {
            var dv = (DetailsView)this.DetailsView;
            var multi = (MultiCheckBoxList)dv.FindControl(checkBoxListId);
            if (multi == null)
            {
                throw new NullReferenceException("MultiCheckBoxList with ID " + checkBoxListId + " not found.");
            }

            return multi;
        }

        #endregion
    }
}
