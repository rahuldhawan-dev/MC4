using System;

namespace MapCallScheduler.JobHelpers.SapEmployee
{
    public class SapEmployeeFileRecord
    {
        // These properties are in the exact same order as the csv file.
        public string PositionGroupCompanyCode { get; set; }
        public string PositionGroupBusinessUnit { get; set; }
        public string PositionGroupBusinessUnitDescription { get; set; }
        public string EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PositionGroupPositionDescription { get; set; }
        public string PositionGroupGroup { get; set; }
        public string PositionGroupState { get; set; } // I don't know what this is. It's a number.
        public DateTime? DateHired { get; set; } // Needs to be parsed from mm/dd/yyyy. This is DateOfHire in the csv.
        public string Status { get; set; }
        public int PersonnelAreaId { get; set; }
        public string PersonnelAreaDescription { get; set; }
        public string ReportsTo { get; set; } // This is another EmployeeID
       // public DateTime? Birthdate { get; set; } // Needs to be parsed from mm/dd/yyyy. This is DateOfBirth in the csv.
        public string Address { get; set; } // This is AddressLine1 in the csv
        public string Address2 { get; set; } // This is AddressLine2 in the csv
        public string City { get; set; } // This is CityCountry in the csv.
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneHome { get; set; } // This is Telephone in the csv.
        public string EmailAddress { get; set; } // This is Email Id in the csv.
        public string PositionGroupKey { get; set; }
        public string ReportingLocation { get; set; }
        public string LocalEmployeeRelationsBusinessPartner { get; set; }
        public string LocalEmployeeRelationsBusinessPartnerCell { get; set; }
        public string PhoneCellular { get; set; } // EmployeeCell in the csv
    }
}