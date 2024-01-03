using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

namespace MapCall.Modules.Data
{

    /// <summary>
    /// Summary description for DropDowns
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class DropDowns : BaseService
    {
        #region Constants

        internal struct SQL
        {
            // TODO: Fix the SQL injection in here!
            internal const string TOWNS_BY_STATE =
                "SELECT towns.TownID as RecID, towns.Town FROM	[Towns] towns LEFT JOIN [States] states ON states.Abbreviation = towns.State WHERE states.StateID = {0} order by Town";

            internal const string TOWNS_BY_OPCNTR =
                "select T.TownID as recID, Town from Towns T join OperatingCentersTowns OCT on OCT.TownID = T.TownID where OperatingCenterID =  {0} order by Town";

            internal const string TOWNS_BY_OPCNTR_TEXT =
                "select T.TownID as recID, Town from Towns T join OperatingCentersTowns OCT on OCT.TownID = T.TownID join OperatingCenters OC on OC.OperatingCenterID = OCT.OperatingCenterID where OC.OperatingCenterCode = '{0}' order by Town";

            internal const string STREETS_BY_TOWN =
                "select s.StreetID, s.fullstname from Streets s left join Towns t on t.TownID = s.TownID WHERE t.TownID = {0} order by s.fullstname";

            internal const string FUNCTIONAL_LOCATIONS_BY_TOWN =
                "Select FunctionalLocationID, Description from FunctionalLocations where TownID = {0} order by s.Description";

            internal const string FUNCTIONAL_LOCATIONS_BY_TOWN_FOR_MANHOLES =
                "Select FunctionalLocationID, Description from FunctionalLocations where TownID = {0} AND AssetTypeID in (Select AssetTypeID from AssetTypes where Description Like 'Sewer%') order by s.Description";

            internal const string TOWN_SECTIONS_BY_TOWN =
                "select TownSectionID, Name from TownSections ts where ts.TownID = {0}";

            internal const string HYDRANTS_BY_STREET =
                "select a.Id, a.HydrantSuffix from Hydrants a where streetId = {0} order by a.HydrantSuffix";

            internal const string MANHOLES_BY_STREET =
                "select sewerManholeID, manholeNumber from Sewermanholes where streetID = {0} order by manholeNumber";

            internal const string MANHOLESS_BY_TOWN =
                "select sewerManholeID, manholeNumber from Sewermanholes where townID = {0} order by manholeNumber";

            internal const string OVERFLOWS_BY_STREET =
                "select seweroverflowid, seweroverflowid from seweroverflows where streetid = {0}";

            internal const string OPERATINGCENTERS_BY_TOWN =
                "select distinct OperatingCenterCode, OperatingCenterCode from OperatingCenters OC left join OperatingCentersTowns OCT on OCT.OperatingCenterID = OC.OperatingCenterID where TownID = {0}";

            internal const string OPERATING_CENTERS_BY_ID_BY_TOWN =
                "select distinct OC.OperatingCenterID, OperatingCenterCode from OperatingCenters OC left join OperatingCentersTowns OCT on OCT.OperatingCenterID = OC.OperatingCenterID where TownID = {0}";

            // TODO: Remove this buried logic when mvc'n
            internal const string PIPE_DATA_VALUES_BY_PIPE_DATA_TYPE =
                @"select PipeDataLookupValueID, Description from PipeDataLookupValues LV where PipeDataLookupTypeID = @PipeDataLookupTypeID AND
	                NOT EXISTS (
		                SELECT 
			                1
		                FROM 
			                PipeDataLookupValues pdlv
		                JOIN
			                PipeDataLookupTypes pdlt ON pdlt.PipeDataLookupTypeID = pdlv.PipeDataLookupTypeID
		                WHERE 
			                pdlt.Description IN ('Pipe Diameter', 'Decade Installed', 'Pipe Material', 'Residential Customers Affected', 'Commercial Customers Affected')
		                AND
			                pdlv.Description IN ('None Selected', 'None')
		                AND
			                pdlv.PipeDataLookupValueID = LV.PipeDataLookupValueID 
                )";
        }

        #endregion

        #region Web Methods

        private CascadingDropDownNameValue[] GetNameValuesOld(string command)
        {
            using (var ds = new SqlDataSource(McProdConnectionString, command))
            using (var dv = (DataView) ds.Select(DataSourceSelectArguments.Empty))
            {
                var values = new List<CascadingDropDownNameValue>();
                foreach (DataRow row in dv.Table.Rows)
                {
                    values.Add(new CascadingDropDownNameValue(row[1].ToString(),
                                                              row[0].ToString()));
                }
                return values.ToArray();
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetContractors(string knownCategoryValues, string category)
        {
            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = "SELECT ContractorID, Name FROM Contractors";
                return GetNameValues(com, "Name", "ContractorID");
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetInsuranceByContractor(string knownCategoryValues, string category)
        {
            var contractorId = ParseKnownCategory(knownCategoryValues, "Contractor");

            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = "SELECT ContractorInsuranceID, PolicyNumber FROM ContractorInsurance"
                                  + " WHERE ContractorID = @ContractorID";

                com.Parameters.AddWithValue("ContractorID", contractorId);
                return GetNameValues(com, "PolicyNumber", "ContractorInsuranceID");
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetCountiesByState(string knownCategoryValues, string category)
        {
            var state = ParseKnownCategory(knownCategoryValues, "State");

            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = "SELECT Name, CountyID FROM Counties where StateID = @StateID Order By Counties.Name";

                com.Parameters.AddWithValue("StateID", state);
                return GetNameValues(com, "Name", "CountyID");
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetTownsByCounty(string knownCategoryValues, string category)
        {
            var county = ParseKnownCategory(knownCategoryValues, "County");
            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = "SELECT Town, TownID FROM Towns where CountyID = @CountyID order by Town";

                com.Parameters.AddWithValue("CountyID", county);
                return GetNameValues(com, "Town", "TownID");
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetTownsByState(string knownCategoryValues, string category)
        {
            var state = ParseKnownCategory(knownCategoryValues, "State");
            return GetNameValuesOld(string.Format(SQL.TOWNS_BY_STATE, state));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetTownsByOperatingCenter(string knownCategoryValues, string category)
        {
            var opCntr = ParseKnownCategory(knownCategoryValues, "OpCntr");
            return GetNameValuesOld(string.Format(SQL.TOWNS_BY_OPCNTR, opCntr));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetTownsByOperatingCenterID(string knownCategoryValues, string category)
        {
            return GetTownsByOperatingCenter(knownCategoryValues, category);
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetTownsByOperatingCenterText(string knownCategoryValues, string category)
        {
            var opCntr = ParseKnownCategory(knownCategoryValues, "OpCntr");
            return GetNameValuesOld(string.Format(SQL.TOWNS_BY_OPCNTR_TEXT, opCntr));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetStreetsByTown(string knownCategoryValues, string category)
        {
            var town = ParseKnownCategory(knownCategoryValues, "Town");
            return GetNameValuesOld(string.Format(SQL.STREETS_BY_TOWN, town));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetStreetsByNameWithNoStreetOption(string knownCategoryValues,
                                                                               string category)
        {
            var results = GetStreetsByTown(knownCategoryValues, category);
            var resList = results.ToList();
            resList.Insert(0, new CascadingDropDownNameValue("STREET UNKNOWN", "0"));
                // string.Empty would be better, but it won't work
            //  with CascadingDropDowns. 
            return resList.ToArray();
        }
        
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetHydrantsByStreet(string knownCategoryValues, string category)
        {
            var street = ParseKnownCategory(knownCategoryValues, "Street");
            return GetNameValuesOld(string.Format(SQL.HYDRANTS_BY_STREET, street));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetManholesByStreet(string knownCategoryValues, string category)
        {
            var street = ParseKnownCategory(knownCategoryValues, "Street");
            return GetNameValuesOld(string.Format(SQL.MANHOLES_BY_STREET, street));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetManholesByTown(string knownCategoryValues, string category)
        {
            var town = ParseKnownCategory(knownCategoryValues, "Town");
            return GetNameValuesOld(string.Format(SQL.MANHOLESS_BY_TOWN, town));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetTownSectionsByTownDefined(string knownCategoryValues, string category)
        {
            var town = ParseKnownCategory(knownCategoryValues, "Town");
            return GetNameValuesOld(string.Format(SQL.TOWN_SECTIONS_BY_TOWN, town));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetOverflowsByStreet(string knownCategoryValues, string category)
        {
            var street = ParseKnownCategory(knownCategoryValues, "Street");
            return GetNameValuesOld(string.Format(SQL.OVERFLOWS_BY_STREET, street));
        }
        
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetServiceNumbersByStreetID(string knownCategoryValues, string category)
        {
            var streetId = int.Parse(ParseKnownCategory(knownCategoryValues, "Street"));

            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = "SELECT RecID, ServNum, PremNum from tblNJAWService where StName = @StreetID";
                com.Parameters.AddWithValue("StreetID", streetId);

                conn.Open();
                var results = new List<CascadingDropDownNameValue>();
                using (var reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var val = reader["RecID"].ToString();
                        var name = string.Format("Service [{0}] : Premise [{1}]", reader["ServNum"], reader["PremNum"]);
                        results.Add(new CascadingDropDownNameValue(name, val));
                    }
                }
                return results.ToArray();
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetOperatingCentersByTownID(string knownCategoryValues, string category)
        {
            var townID = ParseKnownCategory(knownCategoryValues, "Town");
            return GetNameValuesOld(string.Format(SQL.OPERATINGCENTERS_BY_TOWN, townID));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetOperatingCentersByIDByTownID(string knownCategoryValues, string category)
        {
            var townID = ParseKnownCategory(knownCategoryValues, "Town");
            return GetNameValuesOld(string.Format(SQL.OPERATING_CENTERS_BY_ID_BY_TOWN, townID));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetFacilitiesByOperatingCenter(string knownCategoryValues, string category)
        {
            const string sql =
                @"SELECT [RecordId], isNull([FacilityID], '') + ' - ' + isNull([FacilityName], '') as Facility
                    FROM [tblFacilities] f
                    JOIN [OperatingCenters] opc ON opc.[OperatingCenterID] = f.OperatingCenterID
                    WHERE opc.[OperatingCenterID] = @OperatingCenterID
                    AND f.Interconnection = 1
                    ORDER BY len(f.[FacilityId]), f.[FacilityId]";

            var opCenterId = int.Parse(ParseKnownCategory(knownCategoryValues, "OperatingCenterID"));

            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = sql;
                com.Parameters.AddWithValue("OperatingCenterID", opCenterId);
                return GetNameValues(com, "Facility", "RecordId");
            }
        }
        
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetStockLocationsByOperatingCenter(string knownCategoryValues, string category)
        {
            const string sql =
                @"SELECT StockLocationID, Description 
                    FROM StockLocations 
                    WHERE OperatingCenterID = @OperatingCenterID";
            var operatingCenterId = ParseKnownCategory(knownCategoryValues, "OperatingCenterID");
            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = sql;
                com.Parameters.AddWithValue("OperatingCenterID", operatingCenterId);
                return GetNameValues(com, "Description", "StockLocationID");
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetContractorsByOperatingCenter(string knownCategoryValues, string category)
        {
            const string sql =
                @"SELECT DISTINCT
	                    c.ContractorID, c.Name
                    FROM 
	                    Contractors C
                    JOIN
	                    ContractorsOperatingCenters coc
                    on
	                    coc.ContractorID = c.ContractorID
                    WHERE 
	                    OperatingCenterID = @OperatingCenterID";
            var operatingCenterId = ParseKnownCategory(knownCategoryValues, "OperatingCenterID");
            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = sql;
                com.Parameters.AddWithValue("OperatingCenterID", operatingCenterId);
                return GetNameValues(com, "Name", "ContractorID");
            }
        }
        
        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetPipeDataLookupValueByPipeDataLookupType(string knownCategoryValues, string category)
        {
            var pipeDataLookupTypeID = ParseKnownCategory(knownCategoryValues, "PipeDataLookupTypeID");
            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = SQL.PIPE_DATA_VALUES_BY_PIPE_DATA_TYPE;
                com.Parameters.AddWithValue("PipeDataLookupTypeID", pipeDataLookupTypeID);
                return GetNameValues(com, "Description", "PipeDataLookupValueID");
            }
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetFunctionalLocationsByTown(string knownCategoryValues, string category)
        {
            var town = ParseKnownCategory(knownCategoryValues, "Town");
            return GetNameValuesOld(string.Format(SQL.FUNCTIONAL_LOCATIONS_BY_TOWN, town));
        }

        [WebMethod, ScriptMethod]
        public CascadingDropDownNameValue[] GetFunctionalLocationsByTownForManholes(string knownCategoryValues, string category)
        {
            var town = ParseKnownCategory(knownCategoryValues, "Town");
            return GetNameValuesOld(string.Format(SQL.FUNCTIONAL_LOCATIONS_BY_TOWN_FOR_MANHOLES, town));
        }
        
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<MaterialLookup> LookupMaterials(string search, string operatingCenterID)
        {
            var ret = new List<MaterialLookup>();
            const string sql =
                @"SELECT M.MaterialID, M.Description, M.PartNumber 
                    FROM 
	                    OperatingCenterStockedMaterials ocsm
                    JOIN
	                    Materials M
                    ON
	                    M.MaterialID = ocsm.MaterialID
                    WHERE 
                        OperatingCenterID = @OperatingCenterID
                    AND
                        (M.PartNumber LIKE @partNumber
                        OR
                        M.Description LIKE @partNumber)";
            using (var conn = new SqlConnection(McProdConnectionString))
            using (var com = conn.CreateCommand())
            {
                com.CommandText = sql;
                com.Parameters.AddWithValue("OperatingCenterID", operatingCenterID);
                com.Parameters.AddWithValue("partNumber", string.Format("%{0}%", search));
                if (com.Connection.State != ConnectionState.Open)
                {
                    com.Connection.Open();
                }
                using (var reader = com.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ret.Add(new MaterialLookup {
                                MaterialID = reader["MaterialID"].ToString(),
                                Description = reader["Description"].ToString(),
                                PartNumber = reader["PartNumber"].ToString()
                            });
                        }
                    }
                }
                return ret;
            }
        }
        
        #endregion
    }

    public class MaterialLookup
    {
        public string MaterialID { get; set; }
        public string Description { get; set; }
        public string PartNumber { get; set; }
    }
}
