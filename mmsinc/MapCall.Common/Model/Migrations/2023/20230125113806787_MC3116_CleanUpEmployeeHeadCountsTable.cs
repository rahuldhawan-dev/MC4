using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230125113806787), Tags("Production")]
    public class MC3116_CleanUpEmployeeHeadCountsTable : Migration
    {
        public override void Up()
        {
            // "Area" is not table specific, hard to determine if anything else uses it so we don't wanna delete that data.
            this.ExtractLookupTableLookup("BusinessUnits", "Area", "BusinessUnitAreas", 50, "Area", 
                lookupIsTableSpecific: false,
                deleteOldForeignKey: true,
                deleteLookupValues: false,
                createForeignKey: true);

            // Renaming this column to follow conventions.
            Rename.Column("Area").OnTable("BusinessUnits").To("BusinessUnitAreaId");

            this.ExtractLookupTableLookup("EmployeeHeadCounts", "Category", "EmployeeHeadCountCategories", 50, "Category",
                lookupIsTableSpecific: true,
                deleteOldForeignKey: false, // There's no existing foreign key here
                deleteLookupValues: false,
                createForeignKey: true);
            
            // Renaming this column to follow conventions.
            Rename.Column("Category").OnTable("EmployeeHeadCounts").To("EmployeeHeadCountCategoryId");

            // Nicole specifically asked to round these values up since you can't have a fraction of a person.
            Execute.Sql(@"
                update EmployeeHeadCounts set 
                Total = CEILING(Total),
                NonUnion = CEILING(NonUnion),
                [Union] = CEILING([Union]),
                Other = CEILING(Other)");

            Alter.Column("Total").OnTable("EmployeeHeadCounts").AsInt32().NotNullable();
            Alter.Column("NonUnion").OnTable("EmployeeHeadCounts").AsInt32().NotNullable();
            Alter.Column("Union").OnTable("EmployeeHeadCounts").AsInt32().NotNullable();
            Alter.Column("Other").OnTable("EmployeeHeadCounts").AsInt32().NotNullable();
            Alter.Column("CreatedBy").OnTable("EmployeeHeadCounts").AsString(50).NotNullable();
            Rename.Column("Total").OnTable("EmployeeHeadCounts").To("TotalCount");
            Rename.Column("NonUnion").OnTable("EmployeeHeadCounts").To("NonUnionCount");
            Rename.Column("Union").OnTable("EmployeeHeadCounts").To("UnionCount");
            Rename.Column("Other").OnTable("EmployeeHeadCounts").To("OtherCount");
            Rename.Column("EmployeeHeadCountId").OnTable("EmployeeHeadCounts").To("Id");
            Rename.Column("CreatedOn").OnTable("EmployeeHeadCounts").To("CreatedAt");
        }

        public override void Down()
        {           
            Rename.Column("CreatedAt").OnTable("EmployeeHeadCounts").To("CreatedOn");
            Rename.Column("Id").OnTable("EmployeeHeadCounts").To("EmployeeHeadCountId");
            Rename.Column("TotalCount").OnTable("EmployeeHeadCounts").To("Total");
            Rename.Column("NonUnionCount").OnTable("EmployeeHeadCounts").To("NonUnion");
            Rename.Column("UnionCount").OnTable("EmployeeHeadCounts").To("Union");
            Rename.Column("OtherCount").OnTable("EmployeeHeadCounts").To("Other");
            Alter.Column("CreatedBy").OnTable("EmployeeHeadCounts").AsString(50).Nullable();
            Alter.Column("Total").OnTable("EmployeeHeadCounts").AsFloat().Nullable();
            Alter.Column("NonUnion").OnTable("EmployeeHeadCounts").AsFloat().Nullable();
            Alter.Column("Union").OnTable("EmployeeHeadCounts").AsFloat().Nullable();
            Alter.Column("Other").OnTable("EmployeeHeadCounts").AsFloat().Nullable();

            Rename.Column("EmployeeHeadCountCategoryId").OnTable("EmployeeHeadCounts").To("Category");
            this.ReplaceLookupTableLookup("EmployeeHeadCounts", "Category", "EmployeeHeadCountCategories", 50, "Category",
                createdForeignKey: true,
                deletedLookupValues: false,
                lookupIsTableSpecific: false,
                recreateOldForeignKey: false); // This column never had a foreign key.

            Rename.Column("BusinessUnitAreaId").OnTable("BusinessUnits").To("Area");
            this.ReplaceLookupTableLookup("BusinessUnits", "Area", "BusinessUnitAreas", 50, "Area",
                createdForeignKey: true,
                deletedLookupValues: false,
                lookupIsTableSpecific: false);
        }
    }
}

