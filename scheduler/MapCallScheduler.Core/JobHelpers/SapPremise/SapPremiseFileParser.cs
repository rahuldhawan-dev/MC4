using System;
using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapPremise
{
    public class SapPremiseFileParser : SapDelimitedFileParserBase<SapPremiseFileRecord>,
        ISapPremiseFileParser
    {
        #region Private Methods

        protected override Action<SapPremiseFileRecord, string>[] GetConversionTable()
        {
            return new Action<SapPremiseFileRecord, string>[] {
                (r, s) => r.OperatingCentre = s,
                (r, s) => r.PremiseNumber = s,
                (r, s) => r.ConnectionObject = s,
                (r, s) => r.DeviceLocation = s,
                (r, s) => r.DeviceCategory = s,
                (r, s) => r.Equipment = s,
                (r, s) => r.NextMeterReadingdate = s,
                (r, s) => r.DeviceSerialNumber = s,
                (r, s) => r.Latitude = s,
                (r, s) => r.Longitude = s,
                (r, s) => r.ServiceAddressHouseNumber = s,
                (r, s) => r.ServiceAddressFraction = s,
                (r, s) => r.ServiceAddressApartment = s,
                (r, s) => r.ServiceAddressStreet = s,
                (r, s) => r.ServiceCity = s,
                (r, s) => r.ServiceState = s,
                (r, s) => r.ServiceZip = s,
                (r, s) => r.ServiceDistrict = s,
                (r, s) => r.ServiceDistrictDescription = s,
                (r, s) => r.AreaCode = s,
                (r, s) => r.AreaCodeDescription = s,
                (r, s) => r.RegionCode = s,
                (r, s) => r.RegionCodeDescription = s,
                (r, s) => r.RouteNumber = s,
                (r, s) => r.RouteNumberDescription = s,
                (r, s) => r.StatusCode = s,
                (r, s) => r.ServiceUtilityType = s,
                (r, s) => r.MeterSize = s,
                (r, s) => r.MeterLocation = s,
                (r, s) => r.MeterLocationFreeText = s,
                (r, s) => r.Installation = s,
                (r, s) => r.MeterSerialNumber = s,
                (r, s) => r.CriticalCareType = s,
                ParseIsMajorAccount,
                (r, s) => r.MajorAccountManager = s,
                (r, s) => r.AccountManagerContactNumber = s,
                (r, s) => r.AccountManagerEmail = s,
                (r, s) => r.PremiseType = s,
                (r, s) => r.Pwsid = s
            };
        }

        private static void ParseIsMajorAccount(SapPremiseFileRecord record, string value)
        {
            // SAP sends us Major account flag  Y/N. MapCall sets as bit value 1/0.

            try
            {
                record.IsMajorAccount = value.ToLower() == "y";
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to parse ISMajorAccount: {value}", e);
            }
        }

        #endregion
    }
}