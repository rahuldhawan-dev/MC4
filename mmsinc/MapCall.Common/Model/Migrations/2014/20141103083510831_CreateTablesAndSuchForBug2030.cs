using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141103083510831), Tags("Production")]
    public class CreateTablesAndSuchForBug2030 : Migration
    {
        public struct StringLengths
        {
            public const int LICENSE_NUMBER = 50;
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues("DriversLicenseClasses", "A", "B", "C", "D");
            this.CreateLookupTableWithValues("CommercialDriversLicenseProgramStatuses", "In Program", "Pursuing CDL",
                "Not In Program");

            Alter.Table("tblEmployee")
                 .AddForeignKeyColumn("CommercialDriversLicenseProgramStatusId",
                      "CommercialDriversLicenseProgramStatuses")
                 .Nullable();
            Execute.Sql(
                "UPDATE tblEmployee set CommercialDriversLicenseProgramStatusId = (Select Id from CommercialDriversLicenseProgramStatuses where Description = 'Not In Program')");
            Alter.Column("CommercialDriversLicenseProgramStatusId").OnTable("tblEmployee").AsInt32().NotNullable();

            Create.Table("DriversLicenseEndorsements")
                  .WithIdentityColumn()
                  .WithColumn("Letter").AsFixedLengthString(1).NotNullable()
                  .WithColumn("Title").AsString(55).NotNullable();

            Create.Table("DriversLicenseRestrictions")
                  .WithIdentityColumn()
                  .WithColumn("Letter").AsFixedLengthString(1).NotNullable()
                  .WithColumn("Title").AsString(55).NotNullable();

            Create.Table("DriversLicenses")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId").NotNullable()
                  .WithColumn("LicenseNumber").AsAnsiString(StringLengths.LICENSE_NUMBER).NotNullable()
                  .WithForeignKeyColumn("StateId", "States", "StateID").NotNullable()
                  .WithForeignKeyColumn("DriversLicenseClassId", "DriversLicenseClasses").NotNullable()
                  .WithColumn("IssuedDate").AsDateTime().Nullable()
                  .WithColumn("RenewalDate").AsDateTime().Nullable();

            Create.Table("DriversLicensesEndorsements")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("LicenseId", "DriversLicenses")
                  .WithForeignKeyColumn("EndorsementId", "DriversLicenseEndorsements");

            Create.Table("DriversLicensesRestrictions")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("LicenseId", "DriversLicenses")
                  .WithForeignKeyColumn("RestrictionId", "DriversLicenseRestrictions");

            Execute.Sql("INSERT INTO DriversLicenseEndorsements (Letter, Title) VALUES ('T', 'Doubles/Triples');");
            Execute.Sql(
                "INSERT INTO DriversLicenseEndorsements (Letter, Title) VALUES ('P', 'Passenger/Transportation');");
            Execute.Sql(
                "INSERT INTO DriversLicenseEndorsements (Letter, Title) VALUES ('N', 'Liquid Bulk/Tank Cargo');");
            Execute.Sql("INSERT INTO DriversLicenseEndorsements (Letter, Title) VALUES ('H', 'Hazardous Material');");
            Execute.Sql(
                "INSERT INTO DriversLicenseEndorsements (Letter, Title) VALUES ('X', 'Hazardous Material and Tank, Combined');");
            Execute.Sql("INSERT INTO DriversLicenseEndorsements (Letter, Title) VALUES ('S', 'School Bus');");
            Execute.Sql(
                "INSERT INTO DriversLicenseEndorsements (Letter, Title) VALUES ('V', 'Student Transportation Vehicle');");
            Execute.Sql("INSERT INTO DriversLicenseEndorsements (Letter, Title) VALUES ('A', 'Activity Vehicle');");
            Execute.Sql(
                "INSERT INTO DriversLicenseEndorsements (Letter, Title) VALUES ('F', 'Taxi, Livery, Service Bus, Motor Bus or Motor Coach');");

            Execute.Sql("INSERT INTO DriversLicenseRestrictions (Letter, Title) VALUES ('B', 'Corrective Lenses');");
            Execute.Sql("INSERT INTO DriversLicenseRestrictions (Letter, Title) VALUES ('C', 'Mechanical Aid');");
            Execute.Sql("INSERT INTO DriversLicenseRestrictions (Letter, Title) VALUES ('D', 'Prosthetic Aid');");
            Execute.Sql(
                "INSERT INTO DriversLicenseRestrictions (Letter, Title) VALUES ('E', 'Automatic Transmission');");
            Execute.Sql("INSERT INTO DriversLicenseRestrictions (Letter, Title) VALUES ('F', 'Outside Mirror');");
            Execute.Sql("INSERT INTO DriversLicenseRestrictions (Letter, Title) VALUES ('K', 'CDL Intrastate Only');");
            Execute.Sql(
                "INSERT INTO DriversLicenseRestrictions (Letter, Title) VALUES ('L', 'Vehicles Without Air Brakes');");
            Execute.Sql(
                "INSERT INTO DriversLicenseRestrictions (Letter, Title) VALUES ('U', 'Hearing Aide Required');");
            Execute.Sql(
                "INSERT INTO DriversLicenseRestrictions (Letter, Title) VALUES ('W', 'Medical Waiver Required');");

            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('DriversLicenses', 'DriversLicenses')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Document', @dataTypeId)");

            Create.Table("MedicalCertificates")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId").NotNullable()
                  .WithColumn("CertificationDate").AsDate().NotNullable()
                  .WithColumn("ExpirationDate").AsDate().NotNullable()
                  .WithColumn("Comments").AsCustom("text").Nullable();

            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('MedicalCertificates', 'MedicalCertificates')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Document', @dataTypeId)");

            Create.Table("ViolationCertificates")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId").NotNullable()
                  .WithColumn("CertificateDate").AsDate().NotNullable()
                  .WithColumn("Comments").AsCustom("text").Nullable();

            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('ViolationCertificates', 'ViolationCertificates')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Document', @dataTypeId)");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblEmployee", "CommercialDriversLicenseProgramStatusId",
                "CommercialDriversLicenseProgramStatuses");
            Delete.Table("CommercialDriversLicenseProgramStatuses");

            Delete.Table("ViolationCertificates");
            Delete.Table("MedicalCertificates");
            Delete.Table("DriversLicensesEndorsements");
            Delete.Table("DriversLicensesRestrictions");
            Delete.Table("DriversLicenseEndorsements");
            Delete.Table("DriversLicenseRestrictions");
            Delete.Table("DriversLicenses");
            Delete.Table("DriversLicenseClasses");

            this.DeleteDataType("MedicalCertificates");
            this.DeleteDataType("ViolationCertificates");
            this.DeleteDataType("DriversLicenses");
        }
    }
}
