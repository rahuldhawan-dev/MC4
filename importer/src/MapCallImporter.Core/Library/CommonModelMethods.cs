using System.Linq;
using System.Text.RegularExpressions;
using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Library.ClassExtensions;
using MMSINC.Data.V2;

namespace MapCallImporter.Library
{
    public static class CommonModelMethods
    {
        public static int FindOperatingCenter<T>(string operatingCenter, string fieldName, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<T> helper)
        {
            if (string.IsNullOrWhiteSpace(helper.RequiredStringValue(operatingCenter, index, fieldName)))
            {
                return -1;
            }

            Match codeMatch;

            if (!(codeMatch = new Regex(@"^([A-Z]+\d*) - .+").Match(operatingCenter)).Success ||
                codeMatch.Groups.Count < 2)
            {
                helper.AddFailure($"Row {index}: {fieldName} value '{operatingCenter}' does not match expected format '<OperatingCenterCode> - <OperatingCenterDescription>'");
                return -1;
            }

            var code = codeMatch.Groups[1].Value;
            var result = uow.GetRepository<OperatingCenter>().Where(oc => oc.OperatingCenterCode == code)
                            .SingleOrDefault();

            if (result == null)
            {
                helper.AddFailure($"Row {index}: Unable to find {fieldName} '{operatingCenter}'");
                return -1;
            }

            return result.Id;
        }

        public static int FindOperatingCenterByName<T>(string operatingCenter, string fieldName, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<T> helper)
        {
            if (string.IsNullOrWhiteSpace(helper.RequiredStringValue(operatingCenter, index, fieldName)))
            {
                return -1;
            }

            var result = uow.GetRepository<OperatingCenter>().Where(oc => oc.OperatingCenterName == operatingCenter)
                            .SingleOrDefault();

            if (result == null)
            {
                helper.AddFailure($"Row {index}: Unable to find {fieldName} '{operatingCenter}'");
                return -1;
            }

            return result.Id;
        }

        public static int? LookupTown<T>(int? operatingCenterId, string operatingCenter, string town, string fieldName, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<T> helper)
        {
            if (string.IsNullOrWhiteSpace(town) || operatingCenterId == null)
            {
                return null;
            }

            var ret = uow.GetRepository<OperatingCenterTown>()
                         .Where(t => t.OperatingCenter.Id == operatingCenterId.Value && t.Town.ShortName == town)
                         .SingleOrDefault()?.Town;

            if (ret == null)
            {
                helper.AddFailure($"Row {index}: Unable to find {fieldName} '{town}' within {nameof(OperatingCenter)} '{operatingCenter}'");
                return null;
            }

            return ret.Id;
        }

        public static int? LookupStreet<T>(int? townId, string town, string street, string fieldName, IUnitOfWork uow,
            int index, ExcelRecordItemHelperBase<T> helper)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                return null;
            }

            if (townId == null)
            {
                helper.AddFailure($"Row {index}: Cannot map a {fieldName} without a value for {nameof(Town)}");
                return null;
            }

            var ret = uow.GetRepository<Street>().Where(s => s.Town.Id == townId && s.FullStName == street)
                         .SingleOrDefault();

            if (ret == null)
            {
                helper.AddFailure(
                    $"Row {index}: Unable to find {fieldName} '{street}' within {nameof(Town)} '{town}'");
                return null;
            }

            return ret.Id;
        }

        public static int? LookupPlanningPlant<T>(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<T> helper, string fieldName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            Match codeMatch;

            if (!(codeMatch = new Regex(@"^([A-Z]+\d+)[^\d]+").Match(value)).Success ||
                codeMatch.Groups.Count < 2)
            {
                helper.AddFailure($"Row {index}: {fieldName} value '{value}' does not match expected format '<PlanningPlantCode> - <OperatingCenterCode> - <PlanningPlantDescription>'");
                return null;
            }

            var code = codeMatch.Groups[1].Value;
            var result = uow.GetRepository<PlanningPlant>().Where(pp => pp.Code == code).FirstOrDefault();

            if (result == null)
            {
                helper.AddFailure($"Row {index}: Unable to find {fieldName} '{value}'");
                return null;
            }

            return result.Id;
        }

        public static int? LookupFacilityArea(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<MaintenancePlan> helper, int facilityId, string fieldName, string value)
        {
            var (area, subarea) = SeparateAreaAndSubArea(value);
            var areas = uow.GetRepository<FacilityFacilityArea>()
                           .Where(x => x.Facility.Id == facilityId && x.FacilityArea.Description == area);
            
            var areaCount = areas.Count();
            
            if (areaCount == 1)
            {
                return areas.First().Id;
            }

            var selectedArea = null as FacilityFacilityArea;

            if (areaCount > 1)
            {
                selectedArea = areas.SingleOrDefault(x => x.FacilitySubArea.Description == subarea);
            }

            if (selectedArea is null)
            {
                helper.AddFailure($"Row {index}: Invalid data for the '{fieldName}' column. An Area with the Description '{value}' in Facility '{facilityId}' could not be found, or returned ambiguous results. A SubArea may need to be specified if this Area does exist.");
            }

            return selectedArea?.Id;
        }

        public static int? LookupEmployee(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<MaintenancePlan> helper, string fieldName, string value)
        {
            if (value == null)
            {
                return null;
            }

            var result = uow.GetRepository<Employee>().Where(u => u.EmployeeId == value).SingleOrDefault();

            if (result == null)
            {
                helper.AddFailure($"Row {index}: Invalid data for the '{fieldName}' column. An Employee with the EmployeeNumber '{value}' was found in the database.");
                return null;
            }

            return result.Id;
        }

        public static int[] LookupFacilityAreas(IUnitOfWork uow, int index, ExcelRecordItemHelperBase<MaintenancePlan> helper, int facilityId, string fieldName, string commaSeparatedValues)
        {
            if (string.IsNullOrEmpty(commaSeparatedValues))
            {
                return null;
            }
            
            return commaSeparatedValues.SplitCommaSeparatedValues()
                                       .Select(area => LookupFacilityArea(uow, index, helper, facilityId, fieldName, area))
                                       .Where(x => x.HasValue)
                                       .Select(x => x.Value).ToArray();
        }

        private static (string, string) SeparateAreaAndSubArea(string input)
        {
            var items = input.Split('-').Select(x => x.Trim()).ToArray();
            return items.Length == 1 ? (items[0], null) : (items[0], items[1]);
        }
    }
}
