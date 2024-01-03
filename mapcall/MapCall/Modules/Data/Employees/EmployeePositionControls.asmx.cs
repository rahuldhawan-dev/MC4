using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;
using AjaxControlToolkit;

namespace MapCall.Modules.Data.Employees
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
     [ScriptService]
    public class EmployeePositionControls : BaseService 
    {
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetPositionsByOpCode(string knownCategoryValues, string category)
        {
            var kv =
                CascadingDropDown.ParseKnownCategoryValuesString(
                    knownCategoryValues);
            var opCode = kv.ContainsKey("OpCode")
                             ? kv["OpCode"]
                             : kv["undefined"];

            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                // QUOTENAME lovingly casts our value to string and wraps it in brackets. Now if only it'd add the space.
                com.CommandText = @"select PositionID, QUOTENAME(PositionID) + ' ' + QUOTENAME(Category) + ' ' + QUOTENAME(Position) as [Position]
                     from tblPositions_Classifications 
                     join [LocalBargainingUnits] ON [LocalBargainingUnits].[LocalID] = [tblPositions_Classifications].[LocalID]
                     join [OperatingCenters] on OperatingCenters.OperatingCenterCode = [tblPositions_Classifications].OpCode
                     where [OperatingCenters].[OperatingCenterId] = @OpCode
                     order by PositionID";
                com.Parameters.AddWithValue("OpCode", opCode);
                return GetNameValues(com, "Position", "PositionID");
            }
        }
    }
}
