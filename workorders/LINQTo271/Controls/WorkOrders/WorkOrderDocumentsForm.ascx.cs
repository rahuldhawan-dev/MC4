using System;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using MMSINC.Controls;
using MMSINC.Exceptions;
using MMSINC.Interface;
using Microsoft.Practices.Web.UI.WebControls;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderDocumentsForm : WorkOrderDetailControlBase
    {
        #region Constants

        public struct SessionParameters
        {
            public const string DOCUMENT_DATA_ID = "DocumentDataId";
        }

        public struct ControlIDs
        {
            public const string EDIT_LINK = "lbEdit",
                                ADD_BUTTON = "btnToggleDetail",
                                ASYNC_FILE_UPLOAD = "afuDocument";
        }

        public struct DocumentParameterNames
        {
            public const string WORK_ORDER_ID = "WorkOrderID",
                                DOCUMENT_ID = "DocumentID",
                                FILE_NAME = "FileName",
                                DOCUMENT_TYPE_ID = "DocumentTypeID",
                                MODIFIED_ON = "ModifiedOn",
                                MODIFIED_BY_ID = "ModifiedByID",
                                CREATED_BY_ID = "CreatedByID",
                                FILE_SIZE = "FileSize";
        }

        #endregion

        #region Control Declarations

        protected IGridView gvDocuments;
        protected IButton btnToggleDetail;
        protected IObjectDataSource odsDocuments;
        protected IDetailControl dvDocument;
        protected IAsyncFileUpload afuDocument;
        protected IPanel pnlDetailsView;

        #endregion

        #region Private Members

        protected IDocumentRepository _documentRepository;
        protected IDocumentDataRepository _documentDataRepository;

        #endregion

        #region Properties

        public IDocumentRepository DocumentRepository
        {
            get
            {
                if (_documentRepository == null)
                    _documentRepository =
                        DependencyResolver.Current.GetService<IDocumentRepository>();
                return _documentRepository;
            }
        }

        public IDocumentDataRepository DocumentDataRepository
        {
            get
            {
                if (_documentDataRepository == null)
                    _documentDataRepository =
                        DependencyResolver.Current.GetService<IDocumentDataRepository>();
                return _documentDataRepository;
            }
        }

        public int DocumentDataId
        {
            get { return (int)ISession[SessionParameters.DOCUMENT_DATA_ID]; }
            set { ISession[SessionParameters.DOCUMENT_DATA_ID] = value; }
        }

        #endregion

        #region Private Methods

        protected override void SetDataSource(int workOrderID)
        {
            odsDocuments.SelectParameters["WorkOrderID"].DefaultValue =
                workOrderID.ToString();
        }

        #endregion

        #region Event Handlers

        #region Control Events

        /// <summary>
        /// This makes any buttons named lbView in the row not act_as_ajaxy. It won't use
        /// them for UpdatePanel postbacks, but as normal postbacks allowing the Response.
        /// BinaryWrite to function properly.
        /// Just needed :  ChildrenAsTriggers="true" UpdateMode="Conditional"
        /// on the UpdatePanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocuments_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            var row = new GridViewRowWrapper(e.Row);
            var lb = row.FindIControl<MvpLinkButton>("lbView");
            var sm = (ScriptManager)IPage.FindControl("smMain");
            if (sm != null && lb != null)
            {
                sm.RegisterPostBackControl(lb);
            }
        }

        protected void btnToggleDetail_Click(object sender, EventArgs e)
        {
            // This is toggling the DetailsView visibility.
            var newMode = DetailViewMode.Insert;
            CurrentMvpMode = newMode;
            dvDocument.ChangeMvpMode(newMode);
            
            // This is being called via the update panel asyncronously 
            // nothing tells the page to re-render the DetailsView. so 
            // this is here to do that.
            dvDocument.DataBind();
            
            //Hide the detailsview by default.
            pnlDetailsView.IStyle.Remove("display");
        }

        protected void odsDocuments_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if(!SecurityService.IsAdmin)
                throw new DomainLogicException("Can't update unless you are a supervisor.");

            e.InputParameters[DocumentParameterNames.MODIFIED_ON] = DateTime.Now;
            e.InputParameters[DocumentParameterNames.MODIFIED_BY_ID] =
                SecurityService.GetEmployeeID();
        }

        protected void ods_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            var document = (Document)e.Instance;
            document.CreatedByID = SecurityService.GetEmployeeID();
            document.DocumentData = DocumentDataRepository.Get(DocumentDataId);
            
            DocumentRepository.InsertDocumentForWorkOrder(document,WorkOrderID);

            gvDocuments.DataBind();
            
            RemoveSessionVars();
        }

        /// <summary>
        /// Removes the session vars for the File Uploader
        /// </summary>
        protected void RemoveSessionVars()
        {
            ISession.Remove(SessionParameters.DOCUMENT_DATA_ID);
        }

        protected void afuDocument_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            var file = dvDocument.FindIControl<IAsyncFileUpload>(ControlIDs.ASYNC_FILE_UPLOAD);

            var docData =
                DocumentDataRepository.SaveOrGetExisting(file.FileBytes);
            DocumentDataId = docData.Id;
        }

        #endregion

        #endregion
    }
}
