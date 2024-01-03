using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(2022100408372147), Tags("Production")]
    public class MC5043AddPackagingTypesTableAndColumnToChemicals : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string PACKAGING_TYPES = "PackagingTypes";
            public const string CHEMICALS = "Chemicals";
        }

        public struct Columns
        {
            public const string PACKAGING_TYPE = "PackagingTypeId";
            public const string OLD_PACKAGING_NAME = "Packaging";
        }

        public struct SQL
        {
            public const string SQL_CHANGE_TEXT_TO_LOOKUP = @"UPDATE c
                Set c.PackagingTypeId = pt.Id
                From Chemicals c
                Inner Join PackagingTypes pt
                On Case 
	                When c.Packaging In ('Bag', 'Bags', 'dry', 'Pallet') And pt.Description = 'Bag' Then 1
                    When c.Packaging In ('Bulk', 'See note') And pt.Description = 'Bulk' Then 1
	                When c.Packaging In ('Cylinder','Cylinders') And pt.Description = 'Cylinder' Then 1
	                When c.Packaging In ('Drums','Drum')  And pt.Description = 'Drum' Then 1
	                When c.Packaging In ('Mini bulk','Mini-Bulk') And pt.Description = 'Mini-Bulk' Then 1
	                When c.Packaging In ('14 Gal carboy','5 Gal carboys','5 Gal Pails','Pail','Pails','Pail/carboy') And pt.Description = 'Pail/carboy' Then 1
	                When c.Packaging In ('sack','sacks','Super Sack') And pt.Description = 'Sack' Then 1
	                When c.Packaging In ('Tote') And pt.Description = 'Tote' Then 1
	                Else 0
                    End = 1";

            public const string SQL_ROLLBACK_PACKAGING_COLUMN = @"Update c
                Set c.Packaging = pt.Description
                From Chemicals c
                Inner Join PackagingTypes pt
                On c.PackagingTypeId = pt.Id";
        }

        #endregion
        public override void Up()
        {
            this.CreateLookupTableWithValues(Tables.PACKAGING_TYPES, "Bag", "Bulk", "Cylinder", "Drum", "Mini-Bulk", "Pail/carboy", "Sack", "Tote");
            Alter.Table(Tables.CHEMICALS).AddForeignKeyColumn(Columns.PACKAGING_TYPE, Tables.PACKAGING_TYPES);
            Execute.Sql(SQL.SQL_CHANGE_TEXT_TO_LOOKUP);
            Delete.Column(Columns.OLD_PACKAGING_NAME).FromTable(Tables.CHEMICALS);
        }
        
        public override void Down()
        {
            Alter.Table(Tables.CHEMICALS).AddColumn(Columns.OLD_PACKAGING_NAME).AsAnsiString(50).Nullable();
            Execute.Sql(SQL.SQL_ROLLBACK_PACKAGING_COLUMN);
            this.DeleteForeignKeyColumn(Tables.CHEMICALS, Columns.PACKAGING_TYPE, Tables.PACKAGING_TYPES);
            Delete.Table(Tables.PACKAGING_TYPES);
        }
    }
}