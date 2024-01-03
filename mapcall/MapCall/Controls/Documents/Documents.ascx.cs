using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Utilities.Documents;
using MMSINC.Controls;
using Document = MapCall.Common.Model.Entities.Document;
using MMSINC.Authentication;
using MMSINC.Utilities;

namespace MapCall.Controls.Documents
{
    public partial class Documents : UserControl, IDataLink
    {
        #region Private Members

        public Documents()
        {
            AllowAdd = true;
            AllowEdit = true;
            AllowDelete = true;
        }

        #endregion

        #region Properties

        public int DataLinkID
        {
            get
            {
                if (this.ViewState["DataLinkID"] == null)
                    return 0;
                else
                    return Int32.Parse(this.ViewState["DataLinkID"].ToString());
            }
            set { this.ViewState["DataLinkID"] = value.ToString(); }
        }
        public int DataTypeID
        {
            get
            {
                if (this.ViewState["DataTypeID"] == null)
                    return 0;
                else
                    return Int32.Parse(this.ViewState["DataTypeID"].ToString());
            }
            set { this.ViewState["DataTypeID"] = value.ToString(); }
        }
        public int DocumentID
        {
            get
            {
                if (this.ViewState["DocumentID"] != null)
                    return Int32.Parse(this.ViewState["DocumentID"].ToString());
                else
                    return 0;
            }
            set { this.ViewState["DocumentID"] = value; }
        }

        public bool AllowDelete { get; set; }

        public bool AllowEdit { get; set; }

        public bool AllowAdd { get; set; }

        #endregion

        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                DS_DocumentType.SelectParameters["DataTypeID"].DefaultValue = DataTypeID.ToString();
            }
            if (DataLinkID != 0 && DataTypeID != 0)
            {
                //loadgvDocuments();
            }
            else
            {
                gvDocuments.Visible = false;
            }
            lblDocResults.Visible = false;
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            FileDocumentUpload1.Attributes.Add("onchange", String.Format("document.getElementById('{0}').value = getFilename(document.getElementById('{1}').value);", txtDocumentFileName.ClientID, FileDocumentUpload1.ClientID));
            ScriptManager.RegisterStartupScript(Page, typeof(string), "aScript", "function getFilename(strFullFileName)\n{\nvar lastSlash = strFullFileName.lastIndexOf('\\\\')+1;\n var filename = strFullFileName.substring(lastSlash, strFullFileName.length);\n return filename;\n}\n", true);

            btnDocLinkToggle.Visible = btnDocAddToggle.Visible = AllowAdd;
            if (DataTypeID != 0)
            {
                loadgvDocuments();
            }
        }


        private void loadgvDocuments()
        {
            dsDocuments.SelectParameters["DataTypeID"].DefaultValue = DataTypeID.ToString();
            dsDocuments.SelectParameters["DataLinkID"].DefaultValue = DataLinkID.ToString();
            gvDocuments.DataBind();
            gvDocuments.Visible = true;
        }
        protected void gvDocuments_DataBound(object sender, EventArgs e)
        {
            dsDocuments.SelectParameters["DataTypeID"].DefaultValue = DataTypeID.ToString();
            dsDocuments.SelectParameters["DataLinkID"].DefaultValue = DataLinkID.ToString();
        }

        protected void gvDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TODO: Fragile code here. Position instead of direct lookup.
                var btnDelete = e.Row.Controls[2].Controls[3];
                if (btnDelete != null) btnDelete.Visible = AllowDelete;
            }
        }

        protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                var docLinkId = Int32.Parse(e.CommandArgument.ToString());
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "delete documentlink where documentLinkID = @documentLinkID";
                    cmd.Parameters.AddWithValue("documentLinkID", docLinkId);
                    cmd.ExecuteNonQuery();
                }

                Audit.Insert(
                      (int)AuditCategory.DataDelete,
                      Page.User.Identity.Name,
                      String.Format("Link Removed for documentLinkID:{0}", docLinkId),
                      ConfigurationManager.ConnectionStrings["MCProd"].ToString());

                gvDocuments.DataBind();
            }

            else if (e.CommandName == "Select")
            {
                var docId = Int32.Parse(e.CommandArgument.ToString());
                var docRepo = DependencyResolver.Current.GetService<IDocumentRepository>();
                var doc = docRepo.Find(docId);
                var hash = doc.DocumentData.Hash;
                var binaryData = DependencyResolver.Current.GetService<IDocumentService>().Open(hash);

                Response.AddHeader("Content-disposition", String.Format("attachment;filename={0}", Server.UrlEncode(doc.FileName)));
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(binaryData);
                Response.End();
            }

            // Pretty sure this is entirely obsolete code. Nothing in the ascx matches it. -Ross 12/9/2013
            // else if (e.CommandName == "Edit")
            //   {
            //DocumentID = (int)gvDocuments.DataKeys[Int32.Parse(e.CommandArgument.ToString())].Value;
            //var d = new Document(DocumentID);
            //d.ConnectionString = ConfigurationManager.ConnectionStrings["MCEnvironmentalCS"].ToString();
            //d.initializeObject();
            //txtDocumentFileName.Text = d.FileName;
            //ddlDocumentType.SelectedValue = d.DocumentTypeID.ToString();
            //  }
        }
        protected void btnAddDocument_Click(object sender, EventArgs e)
        {
            if (FileDocumentUpload1.FileName != "")
                if (FileDocumentUpload1.PostedFile.ContentLength > 154857640)
                {
                    lblDocResults.Text = "File must be less than 150MB";
                    lblDocResults.Visible = true;
                }
                else
                {
                    AddDocument(FileDocumentUpload1, txtDocumentFileName, ddlDocumentType, DataTypeID, DataLinkID);
                    gvDocuments.DataBind();
                }
            else
            {
                lblDocResults.Text = "No Document to update";
                lblDocResults.Visible = true;
            }

        }
        protected void btnDocumentLink_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ddlExistingDocuments.SelectedValue))
            {
                var docId = int.Parse(ddlExistingDocuments.SelectedValue);
                var docTypeId = int.Parse(ddlLinkDocumentType.SelectedValue);
                InsertDocumentLink(docId, DataTypeID, DataLinkID, docTypeId);
            }
        }

        private void InsertDocumentLink(int documentId, int dataTypeId, int dataLinkId, int documentTypeId)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "INSERT INTO DocumentLink(DocumentID, DataTypeID, DataLinkID, DocumentTypeID, UpdatedAt) Values(@DocumentID, @DataTypeId, @DataLinkId, @DocumentTypeId, @UpdatedAt)";
                cmd.Parameters.AddWithValue("DocumentId", documentId);
                cmd.Parameters.AddWithValue("DataTypeId", dataTypeId);
                cmd.Parameters.AddWithValue("DataLinkId", dataLinkId);
                cmd.Parameters.AddWithValue("DocumentTypeId", documentTypeId);
                cmd.Parameters.AddWithValue("UpdatedAt", DependencyResolver.Current.GetService<IDateTimeProvider>().GetCurrentDate());
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnSearchDocument_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtDocumentSearch.Text))
            {
                dsExistingDocuments.SelectParameters.Add("fileName", DbType.AnsiString, $"%{txtDocumentSearch.Text}%");
                dsExistingDocuments.SelectParameters.Add("dataTypeId", DbType.Int32, DataTypeID.ToString());
                dsExistingDocuments.SelectCommand = @"Select document.documentID, isNull(document_type, '') + ' -> ' + isNull([File_Name],'') as [filenm]
		                                                    from documentlink 
                                                            left join document on document.documentID = documentLink.documentID
		                                                    left join documentType on documentType.documentTypeID = document.documentTypeID
	                                                        where documentlink.dataTypeID = @dataTypeId AND [File_Name] like @fileName";
                ddlExistingDocuments.DataBind();
            }
        }

        #endregion

        #region Private Methods

        private void AddDocument(FileUpload fu, TextBox txtFleName, DropDownList ddlType, int DataTypeID, int DataLinkID)
        {
            var docDataRepo = DependencyResolver.Current.GetService<IDocumentDataRepository>();
            var docData = docDataRepo.FindByBinaryData(FileDocumentUpload1.FileBytes);
            if (docData == null)
            {
                docData = new DocumentData();
                docData.BinaryData = FileDocumentUpload1.FileBytes.ToArray();
                // FileSize and Hash are set by the repo.
            }

            var d = new Document();
            d.DocumentData = docData;
            d.FileName = txtFleName.Text;

            var docTypeId = Int32.Parse(ddlType.SelectedValue);
            d.DocumentType = DependencyResolver.Current.GetService<IDocumentTypeRepository>().Find(docTypeId);
            
            var currentUser = DependencyResolver.Current.GetService<IAuthenticationService<User>>().CurrentUser;
            d.CreatedByStr = currentUser.FullName;
            var docRepo = DependencyResolver.Current.GetService<IDocumentRepository>();
            docRepo.Save(d);

            InsertDocumentLink(d.Id, DataTypeID, DataLinkID, docTypeId);

            lblDocResults.Text = (String.Format("File Added: {0}", txtDocumentFileName.Text));
            Audit.Insert(
                (int)AuditCategory.DataInsert,
                Page.User.Identity.Name,
                String.Format("Added document:{0}", txtDocumentFileName.Text),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
            lblDocResults.Visible = true;
            txtDocumentFileName.Text = "";
        }

        #endregion
    }
}