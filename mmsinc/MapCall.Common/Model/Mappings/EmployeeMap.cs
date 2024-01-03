using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        #region Constants

        public const string TABLE_NAME = FixTableAndColumnNamesForBug1623.OldTableNames.EMPLOYEES,
                            SQLITE_CURRENT_POSITION_FORMULA =
                                "(SELECT p.PositionId FROM tblPosition_History ph INNER JOIN tblPositions_Classifications p ON ph.Position_Id = p.PositionId WHERE ph.tblEmployeeId = tblEmployeeId ORDER BY ph.Position_Start_Date desc LIMIT 1)",
                            SQL_SERVER_CURRENT_POSITION_FORMULA =
                                "(SELECT TOP 1 p.PositionId FROM tblPosition_History ph INNER JOIN tblPositions_Classifications p ON ph.Position_Id = p.PositionId WHERE ph.tblEmployeeId = tblEmployeeId ORDER BY ph.Position_Start_Date desc)",
                            SQL_HAS_VALID_CDL =
                                "(CASE WHEN ((select 1 from DriversLicenses dl where dl.EmployeeId = tblEmployeeID AND dl.RenewalDate >= GETDATE() AND dl.DriversLicenseClassId in (1,2,3)) = 1) THEN 1 ELSE 0 END)",
                            SQL_HAS_EXPIRED_CDL =
                                "(CASE WHEN (((select count(1) from DriversLicenses dl where dl.EmployeeId = tblEmployeeID AND dl.RenewalDate >= GETDATE() AND dl.DriversLicenseClassId in (1,2,3)) = 0) AND (select 1 from DriversLicenses dl where dl.EmployeeId = tblEmployeeID AND dl.RenewalDate < GETDATE() AND dl.DriversLicenseClassId in (1,2,3)) = 1) THEN 1 ELSE 0 END)";

        public const string SQL_IS_CDL_COMPLIANT = @"
            (
	            CASE WHEN
	            (
		            (   -- has a drivers license that has not expired
			            (select count(1) from DriversLicenses dl where dl.EmployeeId = tblEmployeeId AND dl.RenewalDate >= GETDATE() AND dl.DriversLicenseClassId in (1,2,3)) > 0
		            )
		            AND
		            (   -- has a medical certificate that has not expired
			            (select count(1) from MedicalCertificates mc where mc.EmployeeId = tblEmployeeId AND mc.ExpirationDate >= GETDATE()) > 0
		            )
                    AND
                    (
                        (select count(1) from ViolationCertificates vc where vc.EmployeeId = tblEmployeeId AND DATEADD(year, 1, vc.CertificateDate) >= GETDATE()) > 0
                    )
	            ) 
	            THEN 1 ELSE 0 END
            )",
                            SQL_DRIVERS_LICENSE_REWEWAL_DATE =
                                "(SELECT Max(dl.RenewalDate) from DriversLicenses dl where dl.EmployeeId = tblEmployeeID)",
                            SQL_DRIVERS_LICENSE_ISSUED_DATE =
                                "(SELECT Max(dl.IssuedDate) from DriversLicenses dl where dl.EmployeeId = tblEmployeeID)",
                            SQL_MEDICAL_CERTIFICATE_EXPIRATION_DATE =
                                "(SELECT Max(mc.ExpirationDate) from MedicalCertificates mc where mc.EmployeeId = tblEmployeeID)",
                            SQL_VIOLATION_CERTIFICATE_EXPIRATION_DATE =
                                "(SELECT DATEADD(year, 1, Max(vc.CertificateDate)) from ViolationCertificates vc where vc.EmployeeId = tblEmployeeID)",
                            // If it's Jan 29th and you add a month, you get Feb 29th in a leap year. 1 year from Feb 29th is Mar 1st
                            // the SQLITE DateAdd function rounds up where as our dateaddplus rounds down like c#/mssql
                            SQL_LITE_VIOLATION_CERTIFICATE_EXPIRATION_DATE =
                                "(SELECT dateaddplus('y', 1, Max(vc.CertificateDate)) from ViolationCertificates vc where vc.EmployeeId = tblEmployeeID)";

        #endregion

        #region Constructors

        public EmployeeMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id, "tblEmployeeID");

            Map(x => x.FirstName).Column("First_Name").Length(Employee.StringLengths.FIRST_NAME);
            Map(x => x.LastName).Column("Last_Name").Length(Employee.StringLengths.LAST_NAME);
            Map(x => x.MiddleName).Column("Middle_Name").Length(Employee.StringLengths.MIDDLE_NAME);
            Map(x => x.EmployeeId).Column("EmployeeID").Unique();
            Map(x => x.DateHired).Column("Date_Hired");
            Map(x => x.SeniorityDate).Column("Seniority_Date");
            Map(x => x.SeniorityRanking).Column("Seniority_Ranking");
            Map(x => x.InactiveDate).Column("Inactive_Date");
            Map(x => x.LicenseWaterTreatment).Column("License_Water_Treatment")
                                             .Length(Employee.StringLengths.LICENSE_WATER_TREATMENT);
            Map(x => x.TLicense).Column("T_License").Length(Employee.StringLengths.T_LICENSE);
            Map(x => x.DateOfTLicense).Column("Date_Of_T_License");
            Map(x => x.LicenseWaterDistribution)
               .Column("License_Water_Distribution").Length(Employee.StringLengths.LICENSE_WATER_DISTRIBUTION);
            Map(x => x.WLicense).Column("W_License").Length(Employee.StringLengths.W_LICENSE);
            Map(x => x.DateOfWLicense).Column("Date_Of_W_License");
            Map(x => x.LicenseSewerCollection).Column("License_Sewer_Collection")
                                              .Length(Employee.StringLengths.LICENSE_SEWER_COLLECTION);
            Map(x => x.CLicense).Column("C_License").Length(Employee.StringLengths.C_LICENSE);
            Map(x => x.DateOfCLicense).Column("Date_Of_C_License");
            Map(x => x.LicenseSewerTreatment).Column("License_Sewer_Treatment")
                                             .Length(Employee.StringLengths.LICENSE_SEWER_TREATMENT);
            Map(x => x.SLicense).Column("S_License").Length(Employee.StringLengths.S_LICENSE);
            Map(x => x.DateOfSLicense).Column("Date_Of_S_License");
            Map(x => x.LicenseIndustrialDischarge)
               .Column("License_Industrial_Discharge").Length(Employee.StringLengths.LICENSE_INDUSTRIAL_DISCHARGE);
            Map(x => x.NLicense).Column("N_License").Length(Employee.StringLengths.N_LICENSE);
            Map(x => x.DateOfNLicense).Column("Date_Of_N_License");
            Map(x => x.CDL).Length(Employee.StringLengths.CDL);
            Map(x => x.DriversLicense).Column("Drivers_License").Length(Employee.StringLengths.DRIVERS_LICENSE);
            Map(x => x.EmergencyContactName).Length(Employee.StringLengths.EMERGENCY_CONTACT_NAME);
            Map(x => x.EmergencyContactPhone).Length(Employee.StringLengths.EMERGENCY_CONTACT_PHONE);
            Map(x => x.EmailAddress).Length(Employee.StringLengths.EMAIL_ADDRESS);
            Map(x => x.Address).Length(Employee.StringLengths.ADDRESS);
            Map(x => x.Address2).Nullable().Length(Employee.StringLengths.ADDRESS2);
            Map(x => x.City).Length(Employee.StringLengths.CITY);
            Map(x => x.State).Length(Employee.StringLengths.STATE);
            Map(x => x.ZipCode).Column("Zip_Code").Length(Employee.StringLengths.ZIP_CODE);
            Map(x => x.PhonePager).Column("Phone_Pager").Length(Employee.StringLengths.PHONE_PAGER);
            Map(x => x.PhoneCellular).Column("Phone_Cellular").Length(Employee.StringLengths.PHONE_CELLULAR);
            Map(x => x.PhoneHome).Column("Phone_Home").Length(Employee.StringLengths.PHONE_HOME);
            Map(x => x.PhoneWork).Column("Phone_Work").Length(Employee.StringLengths.PHONE_WORK);
            Map(x => x.PhonePersonalCellular).Column("Phone_Personal_Cellular")
                                             .Length(Employee.StringLengths.PHONE_PERSONAL_CELLULAR);
            Map(x => x.PurchaseCardNumber).Column("Purchase_Card_Number")
                                          .Length(Employee.StringLengths.PURCHASE_CARD_NUMBER);
            Map(x => x.MonthlyDollarLimit).Nullable();
            Map(x => x.SingleDollarLimit).Nullable();
            Map(x => x.InstitutionalKnowledgeDescription).Nullable().AsTextField();
            Map(x => x.ValidEssentialEmployeeCard);
            Map(x => x.GETSWPSCard);

            Map(x => x.OneDayDoctorsNoteRestrictionEndDate)
               .Nullable();
            Map(x => x.HasOneDayDoctorsNoteRestriction)
               .Not.Nullable()
                // NOTE: date('now') returns midnight in sqlite, but getdate() returns the current time as well.
                // Need to remove the time for this check.
               .DbSpecificFormula(
                    "(CASE WHEN (OneDayDoctorsNoteRestrictionEndDate >= cast(getdate() as date)) THEN 1 ELSE 0 END)",
                    "(CASE WHEN (OneDayDoctorsNoteRestrictionEndDate >= date('now', 'localtime')) THEN 1 ELSE 0 END)");
            Map(x => x.Dummy).Formula("NULL");

            Map(x => x.IsCDLCompliant)
               .DbSpecificFormula(
                    SQL_IS_CDL_COMPLIANT,
                    SQL_IS_CDL_COMPLIANT
                       .Replace("GETDATE()", "date('now')")
                       .Replace("DATEADD(year, 1, vc.CertificateDate)", "date(vc.certificateDate, '1 year')"))
               .LazyLoad();

            Map(x => x.MedicalCertificateExpirationDate).Formula(SQL_MEDICAL_CERTIFICATE_EXPIRATION_DATE);
            Map(x => x.DriversLicenseRenewalDate).Formula(SQL_DRIVERS_LICENSE_REWEWAL_DATE);
            Map(x => x.DriversLicenseIssuedDate).Formula(SQL_DRIVERS_LICENSE_ISSUED_DATE);
            Map(x => x.ViolationCertificateExpirationDate).DbSpecificFormula(SQL_VIOLATION_CERTIFICATE_EXPIRATION_DATE,
                SQL_LITE_VIOLATION_CERTIFICATE_EXPIRATION_DATE);

            References(x => x.ScheduleType, "ScheduleType").Nullable();
            References(x => x.ReportsTo, "Reports_To").Nullable();
            References(x => x.HumanResourcesManager).Nullable();
            References(x => x.PersonnelArea).Nullable();
            References(x => x.PurchaseCardReviewer, "Purchase_Card_Reviewer").Nullable();
            References(x => x.PurchaseCardApprover, "Purchase_Card_Approver").Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.OperatingCenter,
                    OptimizeReferencesForBug1801.NewColumnNames.Employees.OPERATING_CENTER_ID)
               .Nullable();
            References(x => x.ReportingFacility,
                    OptimizeReferencesForBug1801.NewColumnNames.Employees.REPORTING_FACILITY_ID)
               .Nullable();
            References(x => x.Status,
                FixTableAndColumnNamesForBug1623.NewColumnNames.Employees.STATUS_ID).Nullable();
            References(x => x.Department,
                FixTableAndColumnNamesForBug1623.NewColumnNames.Employees.DEPARTMENT_ID).Nullable();
            References(x => x.Gender,
                FixTableAndColumnNamesForBug1623.NewColumnNames.Employees.GENDER_ID).Nullable();
            References(x => x.ReasonForDeparture,
                FixTableAndColumnNamesForBug1623.NewColumnNames.Employees.REASON_FOR_DEPARTURE_ID).Nullable();
            References(x => x.TCPAStatus,
                FixTableAndColumnNamesForBug1623.NewColumnNames.Employees.TCPA_STATUS_ID).Nullable();
            References(x => x.UnionAffiliation,
                FixTableAndColumnNamesForBug1623.NewColumnNames.Employees.UNION_AFFILIATION_ID).Nullable();
            References(x => x.DPCCStatus,
                FixTableAndColumnNamesForBug1623.NewColumnNames.Employees.DPCC_STATUS_ID).Nullable();
            References(x => x.InstitutionalKnowledge,
                FixTableAndColumnNamesForBug1623.NewColumnNames.Employees.INSTITUTIONAL_KNOWLEDGE_ID).Nullable();

            HasOne(x => x.User).PropertyRef(x => x.Employee); // Not all employees are users.
            References(x => x.CurrentPosition)
               .DbSpecificFormula(SQL_SERVER_CURRENT_POSITION_FORMULA, SQLITE_CURRENT_POSITION_FORMULA)
               .ReadOnly();

            // This is nullable, but it is required on all new employee records.
            References(x => x.PositionGroup).Nullable();
            References(x => x.CommercialDriversLicenseProgramStatus)
               .Not.Nullable();

            HasMany(x => x.EmployeeDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            HasMany(x => x.EmployeeNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None(); //.Cascade.DeleteOrphan();
            // these are probably ok but might not be? sorry for being vague, good luck.  - ARR
            // I think those are all linking to EmployeeID on EmployeeLinkView - RD
            // i think the .Inverse means the key is maintained on the other side of the relationship - JD
            // pfft, if I sign my code comments I sign with - Ross - RD
            HasMany(x => x.EmployeeGrievances)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.PositionHistory).KeyColumn("tblEmployeeId");
            HasMany(x => x.EmployeeAccountabilityActions)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.EmployeeTailgateTalks)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.AttendedTrainingRecords)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.ScheduledTrainingRecords)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.EmployeeJobObservations)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.DriversLicenses)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.OperatorLicenses)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.Incidents)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.GeneralLiabilityClaims)
               .KeyColumn("CompanyContactId").Inverse().Cascade.None();
            HasMany(x => x.MedicalCertificates)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.ViolationCertificates)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();
            HasMany(x => x.HepatitisBVaccinations)
               .KeyColumn("EmployeeId").Inverse().Cascade.None();

            HasMany(x => x.AbsenceNotifications)
               .KeyColumn("Employee").Inverse().Cascade.None();
            HasMany(x => x.FamilyMedicalLeaveActCases)
               .KeyColumn("Employee").Inverse().Cascade.None();
            HasMany(x => x.ProductionSkillSets)
               .KeyColumn("EmployeeId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.ProductionAssignments)
               .KeyColumn("AssignedToId").Inverse();
            HasMany(x => x.ScheduledAssignments)
               .KeyColumn("AssignedToId").Cascade.AllDeleteOrphan().Inverse();

            HasManyToMany(x => x.RelatedProductionAssignments)
               .Table("EmployeeAssignmentsEmployees")
               .ParentKeyColumn("EmployeeId")
               .ChildKeyColumn("EmployeeAssignmentId");
        }

        #endregion
    }
}
