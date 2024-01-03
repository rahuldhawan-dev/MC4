using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131218163530), Tags("Production")]
    public class AddConstructionAndCrossingTypeToMainCrossings : Migration
    {
        #region Constants

        public struct Columns
        {
            public const string CROSSING_TYPE_ID = "CrossingTypeID",
                                CONSTRUCTION_TYPE_ID = "ConstructionTypeID",
                                DESCRIPTION = "Description";
        }

        public struct Tables
        {
            public const string CROSSING_TYPES = "CrossingTypes",
                                CONSTRUCTION_TYPES = "ConstructionTypes",
                                MAIN_CROSSINGS = "MainCrossings";
        }

        public struct ForeignKeys
        {
            public const string
                MAIN_CROSSINGS_CROSSING_TYPES = "FK_MainCrossings_CrossingTypes_CrossingTypeID",
                MAIN_CROSSINGS_CONSTRUCTION_TYPES = "FK_MainCrossings_ConstructionTypes_ConstructionTypeID";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }

        #endregion

        public override void Up()
        {
            Alter.Table(Tables.MAIN_CROSSINGS)
                 .AddColumn(Columns.CONSTRUCTION_TYPE_ID).AsInt32().Nullable();
            Alter.Table(Tables.MAIN_CROSSINGS)
                 .AddColumn(Columns.CROSSING_TYPE_ID).AsInt32().Nullable();

            Create.Table(Tables.CONSTRUCTION_TYPES)
                  .WithColumn(Columns.CONSTRUCTION_TYPE_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.CROSSING_TYPES)
                  .WithColumn(Columns.CROSSING_TYPE_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();

            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_CONSTRUCTION_TYPES)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.CONSTRUCTION_TYPE_ID)
                  .ToTable(Tables.CONSTRUCTION_TYPES).PrimaryColumn(Columns.CONSTRUCTION_TYPE_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_CROSSING_TYPES)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.CROSSING_TYPE_ID)
                  .ToTable(Tables.CROSSING_TYPES).PrimaryColumn(Columns.CROSSING_TYPE_ID);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_CONSTRUCTION_TYPES)
                  .OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_CROSSING_TYPES)
                  .OnTable(Tables.MAIN_CROSSINGS);

            Delete.Column(Columns.CONSTRUCTION_TYPE_ID)
                  .FromTable(Tables.MAIN_CROSSINGS);
            Delete.Column(Columns.CROSSING_TYPE_ID)
                  .FromTable(Tables.MAIN_CROSSINGS);

            Delete.Table(Tables.CONSTRUCTION_TYPES);
            Delete.Table(Tables.CROSSING_TYPES);
        }
    }
}
