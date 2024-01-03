using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131017150317), Tags("Production")]
    public class AddPremiseTypes : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string PREMISE_TYPES = "PremiseTypes",
                                SERVICES = "tblNJAWService";
        }

        public struct Columns
        {
            public const string PREMISE_TYPE_ID = "PremiseTypeID",
                                DESCRIPTION = "Description",
                                ABBREVIATION = "Abbreviation",
                                CLEANED_COORDINATES = "CleanedCoordinates",
                                BUSINESS_PARTNER = "BusinessPartner",
                                NSI_NUMBER = "NSINumber";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             ABBREVIATION = 10,
                             BUSINESS_PARTNER = 10;
        }

        public struct ForeignKeys
        {
            public const string FK_SERVICES_PREMISE_TYPES = "FK_tblNJAWService_PremiseTypes_PremiseTypeID";
        }

        #endregion

        public override void Up()
        {
            Create.Table(Tables.PREMISE_TYPES)
                  .WithColumn(Columns.PREMISE_TYPE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.ABBREVIATION).AsAnsiString(StringLengths.ABBREVIATION)
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION);

            Alter.Table(Tables.SERVICES).AddColumn(Columns.PREMISE_TYPE_ID).AsInt32().Nullable();
            Alter.Table(Tables.SERVICES).AddColumn(Columns.CLEANED_COORDINATES).AsBoolean().Nullable();
            Alter.Table(Tables.SERVICES).AddColumn(Columns.BUSINESS_PARTNER)
                 .AsAnsiString(StringLengths.BUSINESS_PARTNER).Nullable();
            Alter.Table(Tables.SERVICES).AddColumn(Columns.NSI_NUMBER).AsInt32().Nullable();

            Create.ForeignKey(ForeignKeys.FK_SERVICES_PREMISE_TYPES)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.PREMISE_TYPE_ID)
                  .ToTable(Tables.PREMISE_TYPES).PrimaryColumn(Columns.PREMISE_TYPE_ID);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_PREMISE_TYPES).OnTable(Tables.SERVICES);

            Delete.Column(Columns.NSI_NUMBER).FromTable(Tables.SERVICES);
            Delete.Column(Columns.BUSINESS_PARTNER).FromTable(Tables.SERVICES);
            Delete.Column(Columns.CLEANED_COORDINATES).FromTable(Tables.SERVICES);
            Delete.Column(Columns.PREMISE_TYPE_ID).FromTable(Tables.SERVICES);

            Delete.Table(Tables.PREMISE_TYPES);
        }
    }
}
