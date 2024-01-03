using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130327090253), Tags("Production")]
    public class AddTownColumns : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string PERMISSIONS = "GRANT ALL ON ServiceCategorizations TO MCUser";
        }

        public struct Tables
        {
            public const string TOWNS = "Towns",
                                SERVICE_CATEGORIZATIONS = "ServiceCategorizations";
        }

        public struct Columns
        {
            public const string ServicePopulation = "ServicePopulation",
                                ResidentialCustomers = "ResidentialCustomers",
                                HousingUnits = "HousingUnits",
                                DataSource = "DataSource",
                                FeetOfMains = "FeetOfMains",
                                MilesOfMains = "MilesOfMains",
                                ProgramInterestNumber = "ProgramInterestNumber",
                                Census2009 = "Census2009",
                                Census2000 = "Census2000",
                                PlanningBoardProjections2005 = "PlanningBoardProjections2005",
                                PlanningBoardProjections2010 = "PlanningBoardProjections2010",
                                PlanningBoardProjections2020 = "PlanningBoardProjections2020",
                                ServiceCategorizationId = "ServiceCategorizationID",
                                Description = "Description";
        }

        public struct ForeignKeys
        {
            public const string FK_TOWNS_SERVICE_CATEGORIZATION =
                "FK_Towns_ServiceCategorizations_ServiceCategorizationID";
        }

        public struct StringLengths
        {
            public const int MAX_DEFAULT_VALUE = 255,
                             PROGRAM_INTEREST_NUMBER = 25,
                             DESCRIPTION_LENGTH = 50;
        }

        #endregion

        public override void Up()
        {
            Alter.Table(Tables.TOWNS).AddColumn(Columns.ServicePopulation).AsDecimal(18, 2).Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.ResidentialCustomers).AsInt32().Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.HousingUnits).AsInt32().Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.DataSource).AsAnsiString(StringLengths.MAX_DEFAULT_VALUE)
                 .Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.FeetOfMains).AsDecimal(18, 2).Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.MilesOfMains).AsDecimal(18, 2).Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.ProgramInterestNumber)
                 .AsAnsiString(StringLengths.PROGRAM_INTEREST_NUMBER).Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.ServiceCategorizationId).AsInt32().Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.Census2009).AsInt32().Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.Census2000).AsInt32().Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.PlanningBoardProjections2005).AsInt32().Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.PlanningBoardProjections2010).AsInt32().Nullable();
            Alter.Table(Tables.TOWNS).AddColumn(Columns.PlanningBoardProjections2020).AsInt32().Nullable();

            Create.Table(Tables.SERVICE_CATEGORIZATIONS)
                  .WithColumn(Columns.ServiceCategorizationId).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.Description).AsAnsiString(StringLengths.DESCRIPTION_LENGTH).NotNullable();
            Create.ForeignKey(ForeignKeys.FK_TOWNS_SERVICE_CATEGORIZATION)
                  .FromTable(Tables.TOWNS).ForeignColumn(Columns.ServiceCategorizationId)
                  .ToTable(Tables.SERVICE_CATEGORIZATIONS).PrimaryColumn(Columns.ServiceCategorizationId);
            Execute.Sql(Sql.PERMISSIONS);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_TOWNS_SERVICE_CATEGORIZATION).OnTable(Tables.TOWNS);
            Delete.Table(Tables.SERVICE_CATEGORIZATIONS);

            Delete.Column(Columns.ServicePopulation).FromTable(Tables.TOWNS);
            Delete.Column(Columns.ResidentialCustomers).FromTable(Tables.TOWNS);
            Delete.Column(Columns.HousingUnits).FromTable(Tables.TOWNS);
            Delete.Column(Columns.DataSource).FromTable(Tables.TOWNS);
            Delete.Column(Columns.FeetOfMains).FromTable(Tables.TOWNS);
            Delete.Column(Columns.MilesOfMains).FromTable(Tables.TOWNS);
            Delete.Column(Columns.ProgramInterestNumber).FromTable(Tables.TOWNS);
            Delete.Column(Columns.ServiceCategorizationId).FromTable(Tables.TOWNS);
            Delete.Column(Columns.Census2009).FromTable(Tables.TOWNS);
            Delete.Column(Columns.Census2000).FromTable(Tables.TOWNS);
            Delete.Column(Columns.PlanningBoardProjections2005).FromTable(Tables.TOWNS);
            Delete.Column(Columns.PlanningBoardProjections2010).FromTable(Tables.TOWNS);
            Delete.Column(Columns.PlanningBoardProjections2020).FromTable(Tables.TOWNS);
        }
    }
}
