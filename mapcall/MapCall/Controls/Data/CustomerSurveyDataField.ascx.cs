using System;
using System.Web.UI;
using MMSINC.DataPages;

namespace MapCall.Controls.Data
{
    public interface ICustomerSurveyDataField : IDataField
    {

    }

    public partial class CustomerSurveyDataField1 : UserControl, ICustomerSurveyDataField
    {
        #region Properties
        
        public string HeaderText
        {
            get { return lblHeaderText.Text; }
            set { lblHeaderText.Text = value; }
        }

        public string DataFieldName { get; set; }
        public DataTypes DataType { get; set; }
        public string QuestionID { get; set; }
        public string SelectCommand { get; set; }
        public string ConnectionString { get; set; }

        #endregion

        #region Exposed Methods

        public string FilterExpression()
        {
            var returnString = string.Empty;
            
            switch (DataType)
            {
                case DataTypes.DropDownList:
                    if (ddlDataField.SelectedIndex > 0)
                    {
                        var value = ddlDataField.SelectedValue.Replace("'", "''");
                        returnString = String.Format(" OR ([{0}] = '{1}' AND CustomerSurveyQuestionID = {2})",
                                                     DataFieldName,
                                                     value,
                                                     QuestionID);
                    }
                    break;
                default: 
                    if (txtDataField.Text.Length > 0)
                        returnString = String.Format(" OR ([{0}] like '%{1}%' AND CustomerSurveyQuestionID = {2})", 
                                            DataFieldName,
                                            txtDataField.Text,
                                            QuestionID);
                    break;
            }
            return returnString;
        }

        public void FilterExpression(IFilterBuilder builder)
        {
            
        }

        #endregion

        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ddlDataField.Visible = txtDataField.Visible = false;

            dsDataField.SelectCommand = SelectCommand;
            dsDataField.ConnectionString = ConnectionString;

            switch (DataType)
            {
                case DataTypes.DropDownList:
                    ddlDataField.Visible = true;
                    break;
                default:
                    txtDataField.Visible = true;
                    break;
            }
        }

        #endregion
    }
}