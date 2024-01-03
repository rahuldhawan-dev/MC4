using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Modules.Data.Manholes
{
    /// <summary>
    /// Summary description for Manholes
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Manholes : WebService
    {
        internal struct SQL
        {
            internal const string TOWN_ABBREVIATION =
                "select ab from Towns where TownID = {0}";
            internal const string TOWN_SECTION_ABBREVIATION =
                "select Abbreviation from TownSections where TownSectionID = {0}";
            internal const string TOP_MANHOLE_SUFFIX_BY_TOWN =
                "select top 1 manholeSuffix from sewermanholes where townId = {0} order by manholesuffix desc";
            internal const string TOP_MANHOLE_SUFFIX_BY_TOWN_SECTION =
                "select top 1 manholeSuffix from sewermanholes where townSectionId = {0} order by manholesuffix desc";
            internal const string MANHOLE_NUMBER_CHECK =
                "select * from sewermanholes where manholeNumber = '{0}'";
        }

        private const string MANHOLE_PREFIX = "M";
        private const string SEPARATOR = "-";

        #region Private Members

        private string _connectionString = ConfigurationManager.ConnectionStrings["MCProd"].ToString();

        #endregion


        #region Town
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetNewManholeNumberForTown(string townRecId)
        {
            StringBuilder sb = new StringBuilder();
            ManholeNumber manholeNumber = new ManholeNumber();
                 
            sb.Append(MANHOLE_PREFIX);

            sb.Append(getAbbreviationForTown(townRecId));
            sb.Append(SEPARATOR);
            manholeNumber.Suffix = getNextManholeSuffixForTown(townRecId);
            sb.Append(manholeNumber.Suffix);

            manholeNumber.Number = sb.ToString();
            return manholeNumber;
        }

        private string getNextManholeSuffixForTown(string townRecId)
        {
            var ds = new SqlDataSource(_connectionString,
                                       string.Format(
                                            SQL.TOP_MANHOLE_SUFFIX_BY_TOWN,
                                            townRecId));
            var dv = (DataView)ds.Select(DataSourceSelectArguments.Empty);
            if (dv.Table.Rows.Count != 1)
                return "1"; //if nothing came back, default to 1.
            int suffix;
            int.TryParse(dv.Table.Rows[0]["manholeSuffix"].ToString(), out suffix);

            return (suffix + 1).ToString();
        }
        
        private string getAbbreviationForTown(string townRecId)
        {
            var ds = new SqlDataSource(_connectionString,
                                       string.Format(
                                            SQL.TOWN_ABBREVIATION,
                                            townRecId));
            var dv = (DataView)ds.Select(DataSourceSelectArguments.Empty);
            if (dv.Table.Rows.Count != 1)
                return null;
            return dv.Table.Rows[0]["ab"].ToString();
        }
        #endregion

        #region Town Section
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetNewManholeNumberForTownSection(string townSectionRecId, string townRecId)
        {
            StringBuilder sb = new StringBuilder();
            ManholeNumber manholeNumber = new ManholeNumber();

            sb.Append(MANHOLE_PREFIX);
            var ab = getAbbreviationForTownSection(townSectionRecId);
            if (!string.IsNullOrEmpty(ab)) //some sections dont have abbreviations
            {
                sb.Append(ab);
                sb.Append(SEPARATOR);
                manholeNumber.Suffix = getNextManholeSuffixForTownSection(townSectionRecId);
                sb.Append(manholeNumber.Suffix);
            }
            else //go by town instead
            {
                sb.Append(getAbbreviationForTown(townRecId));
                sb.Append(SEPARATOR);
                manholeNumber.Suffix = getNextManholeSuffixForTown(townRecId);
                sb.Append(manholeNumber.Suffix);
            }

            manholeNumber.Number = sb.ToString();
            
            return manholeNumber;
        }

        private string getNextManholeSuffixForTownSection(string townSectionRecId)
        {
            var ds = new SqlDataSource(_connectionString,
                           string.Format(
                                SQL.TOP_MANHOLE_SUFFIX_BY_TOWN_SECTION,
                                townSectionRecId));
            var dv = (DataView)ds.Select(DataSourceSelectArguments.Empty);
            if (dv.Table.Rows.Count != 1)
                return "1"; //if nothing came back, default to 1.
            int suffix;
            int.TryParse(dv.Table.Rows[0]["manholeSuffix"].ToString(), out suffix);

            return (suffix + 1).ToString();
        }

        private string getAbbreviationForTownSection(string townSectionRecId)
        {
            var ds = new SqlDataSource(_connectionString,
                                       string.Format(
                                            SQL.TOWN_SECTION_ABBREVIATION,
                                            townSectionRecId));
            var dv = (DataView)ds.Select(DataSourceSelectArguments.Empty);
            if (dv.Table.Rows.Count != 1)
                return null;
            return dv.Table.Rows[0]["Abbreviation"].ToString();
        }
        #endregion

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ManholeNumberExists(string manholeNumber)
        {
            var ds = new SqlDataSource(_connectionString,
                           string.Format(
                                SQL.MANHOLE_NUMBER_CHECK,
                                manholeNumber));
            var dv = (DataView)ds.Select(DataSourceSelectArguments.Empty);
            return dv.Table.Rows.Count > 0;
        }
    }
    
    public class ManholeNumber
    {
        public string Number { get; set; }
        public string Suffix { get; set; }
    }
}
