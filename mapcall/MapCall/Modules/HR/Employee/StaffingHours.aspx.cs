using System;
using System.Data;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MMSINC.DataPages.Permissions;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls;
using StructureMap;

namespace MapCall.Modules.HR.Employee
{
    /// <remarks>
    /// 
    /// Logic:
    ///     User can:
    ///         - Add/edit hours until they're approved.
    /// 
    ///     User can't:
    ///         - Edit hours after they're approved.
    ///         - View other user's hours
    ///         
    /// 
    /// </remarks>
    public partial class StaffingHours : TemplatedDetailsViewDataPageBase
    {
        #region Fields

        private AggregatedDataPagePermissions _perms;

        #endregion

        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get { return HumanResources.StaffingHours; }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        #endregion

        #region Private Methods

        protected override AggregatedDataPagePermissions CreatePermissions()
        {
            var p = base.CreatePermissions();
            _perms = p;
            return p;
        }

        private int GetUserId()
        {
            var repo = DependencyResolver.Current.GetService<IUserRepository>();
            return repo.GetUserByUserName(IUser.Name).RecId;
        }

        protected override void OnDetailsViewItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            base.OnDetailsViewItemInserting(sender, e);
            // TODO: Set UserID parameter. 
            e.Values["UserID"] = GetUserId();
        }

        protected override void OnDetailsViewItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            // Checking if the fields are read-only, we need to set their values
            // back because DetailsView doesn't populate them into the update
            // args when they're readonly. it's-a-stupid!
            if (WorkApprovedDateIsSet() && !Permissions.AdminAccess.IsAllowed)
            {
                var drv = GetDetailsDataItem();
                e.NewValues.Add(bfWorkDate.DataField, drv[bfWorkDate.DataField]);
                e.NewValues.Add(bfHoursWorked.DataField, drv[bfHoursWorked.DataField]);
                e.NewValues.Add(bfWorkDescription.DataField, drv[bfWorkDescription.DataField]);
            }
            base.OnDetailsViewItemUpdating(sender, e);
        }


        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);

            // Only admins can access these fields.
            bfWorkApprovedDate.ReadOnly = (!Permissions.AdminAccess.IsAllowed);
            bfInvoiceNumber.ReadOnly = (!Permissions.AdminAccess.IsAllowed);

            if (!CanViewOtherUsersRecords())
            {
                sfUserName.Visible = false;
                sfUserName.SetValue(IUser.Name);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (PageMode == PageModes.RecordReadOnly && WorkApprovedDateIsSet() && !Permissions.AdminAccess.IsAllowed)
            {
                DetailsViewCrudField.Visible = false;
            }
        }

        private DataRowView GetDetailsDataItem()
        {
            // No idea why this doesn't databind during postbacks.
            if (template.DetailsView.DataItem == null) { template.DetailsView.DataBind(); }
            return (DataRowView) template.DetailsView.DataItem;
        }

        private bool WorkApprovedDateIsSet()
        {
            return (GetDetailsDataItem()["WorkApprovedDate"] != DBNull.Value);
        }

        private bool CanViewOtherUsersRecords()
        {
            return (Permissions.AdminAccess.IsAllowed);
        }

        protected override void LoadDataRecord(int recordId)
        {
            base.LoadDataRecord(recordId);

            // If user is not StaffingHours Admin.
            // These fields can't be modified by a user after
            // the WorkApprovedDate is set.
            if (WorkApprovedDateIsSet() && !Permissions.AdminAccess.IsAllowed)
            {
                // Somehow they got to the edit mode, so we need
                // to get them out of here.
                if (PageMode == PageModes.RecordUpdate)
                {
                    RedirectPageToRecord(recordId);
                }
                
                bfWorkDate.ReadOnly = true;
                bfHoursWorked.ReadOnly = true;
                bfWorkDescription.ReadOnly = true;
            }
        }

        #endregion
    }
}