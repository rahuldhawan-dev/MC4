using System;
using System.Data;
using System.Reflection;
using System.Web.UI;
using MMSINC.Controls;
using MMSINC.DataPages;

namespace MapCall.Controls.SearchFields
{
  

    /// <remarks>
    /// 
    /// Here's how to use one of these when the FilterMode is set to Automatic.
    /// 
    /// <search:TemplatedSearchField 
    ///     Label="State" 
    ///     BindingControlID="ddlStateSearch"
    ///     BindingPropertyName="SelectedValue" 
    ///     BindingDataType="Int32" 
    ///     DataFieldName="h.StateID">
    ///     <Template>
    ///         <mapcall:StatesDropDownList ID="ddlStateSearch" runat="server" />
    ///     </Template>
    /// </search:TemplatedSearchField>
    /// 
    /// </remarks>
    [ParseChildren(true)]
    public class TemplatedSearchField : BaseSearchField
    {
        #region Fields

        private readonly MvpPlaceHolder _template = new MvpPlaceHolder();

        #endregion

        #region Properties


        public string BindingControlID { get; set; }
        public string BindingPropertyName { get; set; }
        public DbType BindingDataType { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public MvpPlaceHolder Template { get { return _template; } }

        #endregion

        #region Private Methods

        #region Getting the value

        private Control GetBoundControl()
        {
            if (string.IsNullOrWhiteSpace(BindingControlID))
            {
                throw new NullReferenceException(
                    "BindingControlID can not be null when FilterMode is set to Automatic.");
            }
            var c = this.Template.FindControl(BindingControlID);

            if (c == null)
            {
                throw new NullReferenceException("Unable to find binding control with id '" + BindingControlID + "'");
            }
            return c;
        }

        private object GetBoundControlValue()
        {
            var c = GetBoundControl();
            var pi = GetPropertyInfo(c);
            return pi.GetValue(c, null);
        }

        private PropertyInfo GetPropertyInfo(Control c)
        {
            if (string.IsNullOrWhiteSpace(BindingPropertyName))
            {
                throw new NullReferenceException(
                    "BindingPropertyName can not be null when FilterMode is set to Automatic");
            }

            var t = c.GetType();
            var pi = t.GetProperty(BindingPropertyName);

            if (pi == null)
            {
                throw new NullReferenceException(string.Format("Unable to find property '{0}' on type '{1}'",
                                                               BindingPropertyName, t.FullName));
            }
            return pi;
        }

        #endregion


        #endregion

        #region Public Methods

        public override void AddControlsToTemplate(Control t)
        {
            t.Controls.Add(this.Template);
        }

        public override void SetValue(object value)
        {
            throw new NotImplementedException();
        }

        protected override void AddExpressions(IFilterBuilder builder)
        {
            var value = GetBoundControlValue();
            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) { return; }
            builder.AddExpression(new FilterBuilderExpression(DataFieldName, this.BindingDataType, value));
        }


        #endregion
    }
}