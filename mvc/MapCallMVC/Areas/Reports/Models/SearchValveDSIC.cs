using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchValveDSIC : SearchSet<ValveDSICReportItem>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [SearchAlias("OperatingCenter", "oc", "IsContractedOperations", Required = true)]
        public bool? IsContractedOperations => false;

        public DateRange DateInstalled { get; set; }

        public bool? MatchesWBSPattern => true;

        public bool? IsInstalled => true;

        // The SearchAliases here are to force the joins to occur, keeping the results
        // from having to execute a select on each record for each of these relationships
        [SearchAlias("Town", "Id", Required = true)]
        public int? Town { get; set; }
        [SearchAlias("Street", "Id", Required = true)]
        public int? Street { get; set; }
        [SearchAlias("Coordinate", "Id", Required = true)]
        public int? Coordinate { get; set; }
    }
}