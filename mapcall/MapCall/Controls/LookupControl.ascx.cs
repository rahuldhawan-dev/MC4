using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;

namespace MapCall.Controls
{
    public partial class LookupControl : UserControl
    {

        #region Fields

        private string _descriptionFieldName;
        private string _tableName;
        private string _tablePrimaryKeyFieldName;

        #endregion

        #region Properties

        public IDataPageRenderHelper RenderHelper
        {
            get { return ((IDataPageBase)Page).RenderHelper; }
        }

        public string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                template.DataElementTableName = value;
            }
        }
        public string TablePrimaryKeyFieldName
        {
            get { return _tablePrimaryKeyFieldName; }
            set
            {
                _tablePrimaryKeyFieldName = value;
                template.DataElementPrimaryFieldName = value;
            }
        }
        public string DescriptionFieldName
        {
            get
            {
                return (!string.IsNullOrEmpty(_descriptionFieldName) ? _descriptionFieldName : "Description");
            }
            set
            {
                _descriptionFieldName = value;
            }
        }

        public DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        public string Label
        {
            get { return Template.Label; }
            set { Template.Label = value; }
        }

        #endregion

        #region Private Methods


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Page.InitComplete += OnPageInitComplete;

        }

        private void OnPageInitComplete(object sender, EventArgs e)
        {
            this.Page.InitComplete -= OnPageInitComplete;

//            detailsView.DataKeyNames = new string[] { TablePrimaryKeyFieldName };
  //          gvResults.DataKeyNames = new string[] { TablePrimaryKeyFieldName };

            // These need to be set here(not in the markup), otherwise the stupid GridView databinds inside
            // its own Init call and throws an invalid sql exception. 
          Template.ResultsDataSource.SelectCommand = FormatSql("SELECT lookup.[{TablePrimaryKeyFieldName}], lookup.[{DescriptionFieldName}] as Description FROM [{TableName}] lookup");



            // Init DetailsView DataSource

            // This needs to select the description field specifically as "Description" in order to bind
            // from the markup. There's no other way to dynamically bind a field. 
            var dsDetails = Template.DetailsDataSource;
            dsDetails.SelectCommand = FormatSql(@"SELECT lookup.[{TablePrimaryKeyFieldName}], lookup.[{DescriptionFieldName}] as Description
                                                  FROM [{TableName}] lookup WHERE [{TablePrimaryKeyFieldName}] = @{TablePrimaryKeyFieldName}");
            dsDetails.SelectParameters.Add(new Parameter(this.TablePrimaryKeyFieldName, DbType.Int32));

            dsDetails.InsertCommand = FormatSql("INSERT INTO [{TableName}] ([{DescriptionFieldName}], [CreatedBy]) VALUES(@Description, @CreatedBy); SELECT @{TablePrimaryKeyFieldName} = (Select @@IDENTITY)");
            dsDetails.InsertParameters.Add(new Parameter(this.TablePrimaryKeyFieldName, DbType.Int32) { Direction = ParameterDirection.Output });
            dsDetails.InsertParameters.Add(new Parameter("CreatedBy", DbType.String));

            dsDetails.UpdateCommand = FormatSql("UPDATE [{TableName}] SET [{DescriptionFieldName}] = @Description WHERE [{TablePrimaryKeyFieldName}] = @{TablePrimaryKeyFieldName}");
            dsDetails.UpdateParameters.Add(new Parameter(this.TablePrimaryKeyFieldName, DbType.Int32));
            dsDetails.UpdateParameters.Add(new Parameter("Description", DbType.String));

            dsDetails.DeleteCommand = FormatSql("DELETE FROM [{TableName}] WHERE [{TablePrimaryKeyFieldName}] = @{TablePrimaryKeyFieldName}");
            dsDetails.DeleteParameters.Add(new Parameter(this.TablePrimaryKeyFieldName, DbType.Int32));
        }

        private string FormatSql(string statement)
        {
            var sb = new StringBuilder(statement);

            // Verify that all parameters are set, don't want to accidently delete 
            // things somehow. 
            VerifyParameterName(TableName);
            VerifyParameterName(TablePrimaryKeyFieldName);
            VerifyParameterName(DescriptionFieldName);

            sb.Replace("{TableName}", TableName);
            sb.Replace("{TablePrimaryKeyFieldName}", TablePrimaryKeyFieldName);
            sb.Replace("{DescriptionFieldName}", DescriptionFieldName);

            return sb.ToString();
        }

        private static void VerifyParameterName(string paramName)
        {
            if (String.IsNullOrWhiteSpace(paramName))
            {
                throw new NullReferenceException("Parameter can not be null, empty, or whitespace.");
            }
        }



        #endregion



    }
}