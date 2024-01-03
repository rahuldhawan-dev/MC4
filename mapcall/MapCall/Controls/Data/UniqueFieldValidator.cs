using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;

namespace MapCall.Controls.Data
{
    // TODO: Make this less crappy. -Ross 7/29/2011

    [ParseChildren(true)]
    public class UniqueFieldValidator : CustomValidator
    {
        #region Properties

        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the primary key column parameter.
        /// </summary>
        public string PrimaryKeyName { get; set; }

        /// <summary>
        /// Gets or sets the Primary Key value. This is for binding purposes. Requires ViewState to work.
        /// </summary>
        public string PrimaryKeyValue
        {
            get
            {
                return (string)ViewState["PrimaryKeyValue"];
            }
            set
            {
                ViewState["PrimaryKeyValue"] = value;
            }
        }

        public string SelectCommand { get; set; }

        /// <summary>
        /// Gets or sets the name of the unique constrained datbase column parameter.
        /// </summary>
        public string UniqueFieldName { get; set; }

        public override ClientIDMode ClientIDMode
        {
            get
            {
                // Must be AutoID!
                return ClientIDMode.AutoID;
            }
            set
            {
                // note: this is silly, but Sonar will complain if we don't use `value` in this setter
                base.ClientIDMode = value == ClientIDMode.AutoID ? value : ClientIDMode.AutoID;
            }
        }

        #endregion

        #region Private Methods

        private Control GetControlToValidate()
        {
            Control c = null;

            if (Parent != null)
            {
                c = Parent.FindControl(ControlToValidate);
            }

            if (c == null)
            {
                c = Utility.GetFirstControlInstance(Page, ControlToValidate);
            }

            return c;
        }

        /// <summary>
        /// Parses the ControlToValidate and returns the parameter value(Key) and TypeCode(Value) necessary.
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<String, TypeCode> GetParameterInfo()
        {
            var c = GetControlToValidate();

            if (c == null)
            {
                throw new NullReferenceException("UniqueFieldValidator requires a control to validate.");
            }

            if (c is TextBox)
            {
                var tb = (TextBox)c;
                return new KeyValuePair<string, TypeCode>(tb.Text.Trim(), TypeCode.String);
            }

            throw new NotSupportedException("UniqueFieldValidator doesn't support controls of type: " + c.GetType().Name);
        }

        #endregion

        #region Virtual Methods

        protected override bool OnServerValidate(string value)
        {
            // MSDN says the base should be called. So uh, there.
            if (!base.OnServerValidate(value))
            {
                return false;
            }

            using (var ds = new SqlDataSource())
            {
                ds.ConnectionString = ConnectionString;
                ds.SelectCommand = SelectCommand;

                var sp = ds.SelectParameters;

                // Insert pages have null values for the primary key, so they need to be replaced with a 0.
                sp.Add(PrimaryKeyName, TypeCode.Int32, (String.IsNullOrEmpty(PrimaryKeyValue) ? "0" : PrimaryKeyValue));

                var p = GetParameterInfo();
                sp.Add(UniqueFieldName, p.Value, p.Key);

                using (var dv = (DataView)ds.Select(new DataSourceSelectArguments()))
                {
                  //  System.Diagnostics.Debug.Print("Count: " + dv.Count);
                    return (dv.Count == 0);
                }
            }
        }

        #endregion
    }
}
