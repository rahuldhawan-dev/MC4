using System;
using System.Globalization;
using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapEmployee
{
    public class SapEmployeeFileParser : SapDelimitedFileParserBase<SapEmployeeFileRecord>, ISapEmployeeFileParser
    {
        #region Private Methods

        private static void ParsePersonnelAreaId(SapEmployeeFileRecord r, string x)
        {
            try
            {
                r.PersonnelAreaId = int.Parse(x);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to parse PersonnelAreaId: {x}", e);
            }
        }

        private static string ParseEmployeeId(string value)
        {
            // SAP sends us employee numbers with leading zeros. MapCall employee numbers do not have these. SAP is bad and won't remove this themselves.
            return int.Parse(value).ToString();
        }

        protected override Action<SapEmployeeFileRecord, string>[] GetConversionTable()
        {
            return new Action<SapEmployeeFileRecord, string>[] {
                // CompanyCode
                (r, s) => r.PositionGroupCompanyCode = s,

                // CostCenter
                (r, s) => r.PositionGroupBusinessUnit = s,

                // CostCenterDesc
                (r, s) => r.PositionGroupBusinessUnitDescription = s,

                // PersNo
                (r, s) => r.EmployeeId = ParseEmployeeId(s), 

                // Lastname
                (r, s) => r.LastName = s,

                // Firstname
                (r, s) => r.FirstName = s,
                
                // POSITION_ID
                (r, s) => r.PositionGroupKey = s,

                // Position
                (r, s) => r.PositionGroupPositionDescription = s,

                // PSgroup
                (r, s) => r.PositionGroupGroup = s,

                // PositionGroupState
                (r, s) => r.PositionGroupState = s,

                // OriginalHireDate
                (r, s) => r.DateHired = string.IsNullOrWhiteSpace(s) ? (DateTime?)null : DateTime.ParseExact(s, "MM/dd/yyyy", CultureInfo.InvariantCulture),

                // EmployeeStatus
                (r, s) => r.Status = s,

                // PersonnelAreaId
                (r, s) => ParsePersonnelAreaId(r,s),

                // PersonnelAreaDesc
                (r, s) => r.PersonnelAreaDescription = s,

                // ManagerID
                (r, s) => r.ReportsTo = string.IsNullOrWhiteSpace(s) ? string.Empty : ParseEmployeeId(s),

                // AWHireDate
                (r, s) => { }, // Noop for this AWHireDate field that is apparently different from the DateHired field.

                // AddressLine1
                (r, s) => r.Address = s,

                // AddressLine2
                (r, s) => r.Address2 = s,

                // City_Country
                (r, s) => r.City = s,

                // State
                (r, s) => r.State = s,

                // ZIPCode
                (r, s) => r.ZipCode = s,

                // Telephone
                (r, s) => r.PhoneHome = s,

                // EmailId
                (r, s) => r.EmailAddress = s,

                // ReportingLocation
                (r, s) => r.ReportingLocation = s,

                // LocalEmployeeRelationsBusinessPartner
                (r, s) => r.LocalEmployeeRelationsBusinessPartner = string.IsNullOrWhiteSpace(s) ? string.Empty : ParseEmployeeId(s),

                // LocalEmployeeRelationsBusinessPartnerCell
                (r, s) => r.LocalEmployeeRelationsBusinessPartnerCell = s,

                // EmployeeCell
                (r, s) => r.PhoneCellular = s,

            };
        }

        #endregion
    }
}