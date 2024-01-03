using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Utilities.Auditing;
using MMSINC.Utilities.Permissions;

namespace MapCall.Modules.Data.Valves
{
    public partial class Mismatches : MvpPage
    {
        #region Private Members

        public enum Actions
        {
            Ignore = 1,
            FurtherReview = 2
        }

        private bool? _canAdministrate;
        private bool? _canEdit;

        #endregion

        #region Properties

        private IModulePermissions ModulePermissions
        {
            get { return Common.Utility.Permissions.Modules.FieldServices.Services; }
        }

        private bool CanActuallyUseThisPage
        {
            get { return (CanAdministrate || CanEdit); }
        }

        private bool CanEdit
        {
            get
            {
                if (_canEdit == null)
                    _canEdit =
                        IUser.CanEdit(ModulePermissions).InAny();
                return _canEdit.Value || CanAdministrate;
            }
        }

        private bool CanAdministrate
        {
            get
            {
                if (_canAdministrate == null)
                    _canAdministrate = IUser.CanAdministrate(ModulePermissions).InAny();
                return _canAdministrate.Value;
            }
        }

        protected bool HasImagesToLink
        {
            get { return ((gvImagesByAddress.Rows.Count + gvImagesByValveNumber.Rows.Count) > 0); }
        }

        #endregion

        #region Private Methods

        private IEnumerable<int> GetSelectedValveImageIds()
        {
            var hash = new HashSet<int>();
            GetSelectedValveImageIdsByGrid(gvImagesByAddress, hash);
            GetSelectedValveImageIdsByGrid(gvImagesByValveNumber, hash);

            Debug.Print(hash.Count.ToString());
            return hash;
        }

        private static void GetSelectedValveImageIdsByGrid(GridView grid, HashSet<int> selectedValveImageIds)
        {
            foreach (GridViewRow row in grid.Rows)
            {
                var chkMate = (CheckBox)row.FindControl("chkMate");
                if (chkMate.Checked)
                {
                    selectedValveImageIds.Add((int)grid.DataKeys[row.RowIndex].Value);
                }
            }
        }

        private void UpdateValveImageId(int valveImageId)
        {
            dsValveImageUpdate.UpdateParameters["ValveImageID"].DefaultValue = valveImageId.ToString();
            dsValveImageUpdate.Update();

            var a = new Auditor();
            a.SqlConnectionString = ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString;
            a.Insert(AuditCategory.DataUpdate, User.Identity.Name, "Updated ValveImages ID:" + valveImageId);
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!CanActuallyUseThisPage)
            {
                Response.Clear();
                Response.Write(String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module));
                Response.End();
            }
        }

        private void BindRelevantControls()
        {
            detailsView.DataBind();
            gvImagesByAddress.DataBind();
            gvImagesByValveNumber.DataBind();
        }
        
        #endregion

        #region Event Handlers

        protected void onBtnFilterClick(object sender, EventArgs e)
        {
            // Call DataBind cause otherwise nothing shows up when you click the button.
            BindRelevantControls();
        }

        protected void OnBtnLinkImagesClicked(object sender, EventArgs e)
        {
            var selected = GetSelectedValveImageIds();
            foreach(var id in selected)
            {
                UpdateValveImageId(id);
            }
            BindRelevantControls();
        }

        protected void OnBtnNextClicked(object sender, EventArgs e)
        {
            detailsView.UpdateItem(false);
            var a = new Auditor();
            a.SqlConnectionString = ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString;
            a.Insert(AuditCategory.DataUpdate, User.Identity.Name, "Updated Valves ID:" + detailsView.SelectedValue);

            BindRelevantControls();
        }

        #endregion
     }
}