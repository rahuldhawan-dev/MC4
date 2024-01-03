using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220714003905967), Tags("Production")]
    public class MC1216AddingLookupTablesForEasements : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("EasementCategories", "Wastewater", "Water");
            this.CreateLookupTableWithValues("EasementReasons", "Access", "Electric", "Pipe Water", "Pipe Wastewater");
            this.CreateLookupTableWithValues("EasementTypes", "Blanket", "Limited");
            this.CreateLookupTableWithValues("PayMonths", 
                "January", "Febraury", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December");
            this.CreateLookupTableWithValues("FeeFrequencies", "Annual", "Monthly", "One Time");
            this.CreateLookupTableWithValues("EasementStatuses", "Proposed", "Recorded");
            this.CreateLookupTableWithValues("GrantorTypes", "Government", "State", "County", "Municipal", "Railroad", "DOT", "Private");

            Alter.Table("Easements").AddForeignKeyColumn("CategoryId", "EasementCategories");
            Execute.Sql("Update Easements set CategoryId = 1 where Category = 673" +
                        "Update Easements set CategoryId = 2 where Category = 674");
            Delete.ForeignKeyColumn("Easements", "Category", "Lookup", "LookupID");

            Alter.Table("Easements").AddForeignKeyColumn("ReasonId", "EasementReasons");
            Execute.Sql("Update Easements set ReasonId = 3 where ReasonForEasement = 678" +
                        "Update Easements set ReasonId = 4 where ReasonForEasement = 679" +
                        "Update Easements set ReasonId = 1 where ReasonForEasement = 680" +
                        "Update Easements set ReasonId = 2 where ReasonForEasement = 681");
            Delete.ForeignKeyColumn("Easements", "ReasonForEasement", "Lookup", "LookupID");

            Alter.Table("Easements").AddForeignKeyColumn("TypeId", "EasementTypes");
            Execute.Sql("Update Easements set TypeId = 1 where TypeOfEasement = 682" +
                        "Update Easements set TypeId = 2 where TypeOfEasement = 683");
            Delete.ForeignKeyColumn("Easements", "TypeOfEasement", "Lookup", "LookupID");

            Alter.Table("Easements").AddForeignKeyColumn("FeeFrequencyId", "FeeFrequencies");
            Execute.Sql("Update Easements set FeeFrequencyId = 1 where FeeFrequency = 675" +
                        "Update Easements set FeeFrequencyId = 2 where FeeFrequency = 676" +
                        "Update Easements set FeeFrequencyId = 3 where FeeFrequency = 677");
            Delete.ForeignKeyColumn("Easements", "FeeFrequency", "Lookup", "LookupID");

            Alter.Table("Easements").AddForeignKeyColumn("PayMonthId", "PayMonths");
            Execute.Sql("Update Easements set PayMonthId = 1 where PayMonth = 684" +
                        "Update Easements set PayMonthId = 2 where PayMonth = 685" +
                        "Update Easements set PayMonthId = 3 where PayMonth = 686" +
                        "Update Easements set PayMonthId = 4 where PayMonth = 687" +
                        "Update Easements set PayMonthId = 5 where PayMonth = 688" +
                        "Update Easements set PayMonthId = 6 where PayMonth = 689" +
                        "Update Easements set PayMonthId = 7 where PayMonth = 690" +
                        "Update Easements set PayMonthId = 8 where PayMonth = 691" +
                        "Update Easements set PayMonthId = 9 where PayMonth = 692" +
                        "Update Easements set PayMonthId = 10 where PayMonth = 693" +
                        "Update Easements set PayMonthId = 11 where PayMonth = 694" +
                        "Update Easements set PayMonthId = 12 where PayMonth = 695");
            Delete.ForeignKeyColumn("Easements", "PayMonth", "Lookup", "LookupID");

            Alter.Table("Easements").AddForeignKeyColumn("GrantorTypeId", "GrantorTypes");
            Execute.Sql("Update Easements set GrantorTypeId = 1 where GovernmentEasement = 1" +
                        "Update Easements set GrantorTypeId = 2 where StateEasement = 1" +
                        "Update Easements set GrantorTypeId = 3 where CountyEasement = 1" +
                        "Update Easements set GrantorTypeId = 4 where MunicipalEasement = 1" +
                        "Update Easements set GrantorTypeId = 5 where NJTransit = 1" +
                        "Update Easements set GrantorTypeId = 6 where NJTurnpike = 1");
            Delete.Column("GovernmentEasement").Column("StateEasement")
                  .Column("CountyEasement").Column("MunicipalEasement")
                  .Column("NJTransit").Column("NJTurnpike").FromTable("Easements");

            Alter.Table("Easements").AddForeignKeyColumn("StatusId", "EasementStatuses")
                 .AddColumn("RecordNumber").AsAnsiString(50).Nullable()
                 .AddColumn("StreetNumber").AsAnsiString().Nullable()
                 .AddForeignKeyColumn("TownSectionId", "TownSections", "TownSectionID")
                 .AddForeignKeyColumn("StreetId", "Streets", "StreetID")
                 .AddForeignKeyColumn("CrossStreetId", "Streets", "StreetID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Easements", "CrossStreetId", "Streets", "StreetID");
            Delete.ForeignKeyColumn("Easements", "StreetId", "Streets", "StreetID");
            Delete.ForeignKeyColumn("Easements", "TownSectionId", "TownSections", "TownSectionID");
            Delete.Column("StreetNumber").FromTable("Easements");
            Delete.Column("RecordNumber").FromTable("Easements");
            Delete.ForeignKeyColumn("Easements", "StatusId", "EasementStatuses");

            Alter.Table("Easements")
                 .AddColumn("GovernmentEasement").AsBoolean().Nullable()
                 .AddColumn("StateEasement").AsBoolean().Nullable()
                 .AddColumn("CountyEasement").AsBoolean().Nullable()
                 .AddColumn("MunicipalEasement").AsBoolean().Nullable()
                 .AddColumn("NJTransit").AsBoolean().Nullable()
                 .AddColumn("NJTurnpike").AsBoolean().Nullable();
            Execute.Sql("Update Easements set  GovernmentEasement = 1 where GrantorTypeId = 1" +
                        "Update Easements set  StateEasement = 1 where GrantorTypeId = 2" +
                        "Update Easements set  CountyEasement = 1 where GrantorTypeId = 3" +
                        "Update Easements set  MunicipalEasement = 1 where GrantorTypeId = 4" +
                        "Update Easements set  NJTransit = 1 where GrantorTypeId = 5" +
                        "Update Easements set  NJTurnpike = 1 where GrantorTypeId = 6");
            Delete.ForeignKeyColumn("Easements", "GrantorTypeId", "GrantorTypes");

            Alter.Table("Easements").AddForeignKeyColumn("PayMonth", "Lookup", "LookupID");
            Execute.Sql("Update Easements set PayMonth = 684 where PayMonthId = 1 " +
                        "Update Easements set PayMonth = 685 where PayMonthId = 2" +
                        "Update Easements set PayMonth = 686 where PayMonthId = 3" +
                        "Update Easements set PayMonth = 687 where PayMonthId = 4" +
                        "Update Easements set PayMonth = 688 where PayMonthId = 5" +
                        "Update Easements set PayMonth = 689 where PayMonthId = 6" +
                        "Update Easements set PayMonth = 690 where PayMonthId = 7" +
                        "Update Easements set PayMonth = 691 where PayMonthId = 8" +
                        "Update Easements set PayMonth = 692 where PayMonthId = 9" +
                        "Update Easements set PayMonth = 693 where PayMonthId = 10" +
                        "Update Easements set PayMonth = 694 where PayMonthId = 11" +
                        "Update Easements set PayMonth = 695 where PayMonthId = 12");
            Delete.ForeignKeyColumn("Easements", "PayMonthId", "PayMonths");

            Alter.Table("Easements").AddForeignKeyColumn("FeeFrequency", "Lookup", "LookupID");
            Execute.Sql("Update Easements set FeeFrequency = 675 where FeeFrequencyId = 1" +
                        "Update Easements set FeeFrequency = 676 where FeeFrequencyId = 2" +
                        "Update Easements set FeeFrequency = 677 where FeeFrequencyId = 3");
            Delete.ForeignKeyColumn("Easements", "FeeFrequencyId", "FeeFrequencies");

            Alter.Table("Easements").AddForeignKeyColumn("TypeOfEasement", "Lookup", "LookupID");
            Execute.Sql("Update Easements set TypeOfEasement = 682 where TypeId = 1" +
                        "Update Easements set TypeOfEasement = 683 where TypeId = 2");
            Delete.ForeignKeyColumn("Easements", "TypeId", "EasementTypes");

            Alter.Table("Easements").AddForeignKeyColumn("ReasonForEasement", "Lookup", "LookupID");
            Execute.Sql("Update Easements set ReasonForEasement = 678 where ReasonId = 3" +
                        "Update Easements set ReasonForEasement = 679 where ReasonId = 4" +
                        "Update Easements set ReasonForEasement = 680 where ReasonId = 1" +
                        "Update Easements set ReasonForEasement = 681 where ReasonId = 2");
            Delete.ForeignKeyColumn("Easements", "ReasonId", "EasementReasons");

            Alter.Table("Easements").AddForeignKeyColumn("Category", "Lookup", "LookupID");
            Execute.Sql("Update Easements set Category = 673 where CategoryId = 1" +
                        "Update Easements set Category = 674 where CategoryId = 2");
            Delete.ForeignKeyColumn("Easements", "CategoryId", "EasementCategories");

            Delete.Table("GrantorTypes");
            Delete.Table("EasementStatuses");
            Delete.Table("FeeFrequencies");
            Delete.Table("PayMonths");
            Delete.Table("EasementTypes");
            Delete.Table("EasementReasons");
            Delete.Table("EasementCategories");
        }
    }
}

