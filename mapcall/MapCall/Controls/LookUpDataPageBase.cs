using System;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;

namespace MapCall.Controls
{
    // TODO: Replace any un-needed controls with fields and junk. 

    public abstract class LookUpDataPageBase : TemplatedDetailsViewDataPageBase 
    {
        /// <summary>
        /// Gets the LookUpControl instance that should handle everything. 
        /// </summary>
        protected abstract LookupControl LookupControl { get; }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return LookupControl.Template; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LookupControl.TableName = DataElementTableName;
            LookupControl.TablePrimaryKeyFieldName = DataElementPrimaryFieldName;
        
        }

        protected override PageModes DefaultPageMode
        {
            get
            {
                return PageModes.Results;
            }
        }

        protected override void OnPageModeChanged(PageModes newMode)
        {
            // Because we're disabling the search page view, we wanna
            // forward any internal changes to the Search page back
            // to the Results page. 
            if (newMode == PageModes.Search)
            {
                PageMode = PageModes.Results;
            }
            else
            {
                base.OnPageModeChanged(newMode);
            }
        }

        protected override string DataElementTableName
        {
            get { return LookupControl.TableName; }
        }

        protected override string DataElementPrimaryFieldName
        {
            get { return LookupControl.TablePrimaryKeyFieldName; }
        }

        protected override IModulePermissions ModulePermissions
        {
            // This is hard-coded in at the moment because Alex
            // said to just use the same role for everyone. 
            get { return FieldServices.DataLookups; }
        }
    }
}
