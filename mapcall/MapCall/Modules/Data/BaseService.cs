using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;

namespace MapCall.Modules.Data
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class BaseService : WebService
    {

        #region Private Members

        private string _mcprodConnectionString;

        #endregion

        #region Properties

        public string McProdConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_mcprodConnectionString))
                    _mcprodConnectionString =
                        ConfigurationManager.ConnectionStrings["MCProd"].ToString();
                return _mcprodConnectionString;
            }
        }

        #endregion


        protected static string ParseKnownCategory(string knownCategoryValues, string catName)
        {
            var kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            return (kv.ContainsKey(catName) ? kv[catName] : kv["undefined"]);
        }

        protected static CascadingDropDownNameValue[] GetNameValues(SqlCommand comm, string nameField, string valueField)
        {
            var result = new List<CascadingDropDownNameValue>();

            if (comm.Connection.State != ConnectionState.Open)
            {
                comm.Connection.Open();
            }

            using (var reader = comm.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var cddnv = GetNameValue(reader[nameField].ToString(), reader[valueField].ToString());
                        result.Add(cddnv);
                    }
                }
            }
            return result.ToArray();
        }

        protected static CascadingDropDownNameValue GetNameValue(string name, string value)
        {
            return new CascadingDropDownNameValue()
                       {
                           name = name,
                           value = value
                       };
        }
    }
}