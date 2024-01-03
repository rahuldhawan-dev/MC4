using System;
using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201204113538942), Tags("Production")]
    public class UpdateNearMissFieldsForMC2685 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("NearMissTypes", "Environmental Near Miss", "Safety Near Miss");
            this.CreateLookupTableWithValues("NearMissCategories", 100,
                "Chemical",
                "Confined Space",
                "Electrical",
                "Ergonomics",
                "Fall Or Slip",
                "Fall Protection",
                "Lock Out Tag Out",
                "Misc Cases",
                "Other",
                "Security",
                "Tools and Equipment",
                "Trenching And Excavation",
                "Vehicles (Construction, Other)",
                "Work Area - Company Facility",
                "Work Area - Public / Customer",
                "Work Zone - Roadway");
            Create.LookupTable("NearMissSubCategories", 100);

            Alter.Table("NearMissCategories")
                 .AddForeignKeyColumn("TypeId", "NearMissTypes");
            Alter.Table("NearMissSubCategories")
                 .AddForeignKeyColumn("CategoryId", "NearMissCategories");
            Alter.Table("NearMisses")
                 .AddForeignKeyColumn("TypeId", "NearMissTypes")
                 .AddForeignKeyColumn("CategoryId", "NearMissCategories")
                 .AddForeignKeyColumn("SubCategoryId", "NearMissSubCategories");
            Alter.Column("Severity").OnTable("NearMisses")
                 .AsAnsiString(100).Nullable();

            // Set the Types for Categories
            Execute.Sql("UPDATE NearMissCategories SET TypeId = 2;" +
                        "UPDATE NearMisses SET TypeId = 2;");
            // Set the Category for NearMisses
            Execute.Sql(
                "UPDATE NearMisses set CategoryId = nmc.id FROM NearMisses, NearMissCategories nmc WHERE SafetyNearMiss like nmc.Description + '%'");
            // Create Subcategories from existing data
            Execute.Sql(@"
                INSERT INTO
                    NearMissSubCategories(CategoryId, Description)
                SELECT
                    Distinct CategoryId, Replace(Replace(LTRIM(RTRIM(SUBSTRING(SafetyNearMiss, len(nmc.Description)+1, 100))), char(10), ''), char(13), '')
                FROM
                    NearMisses nm
                JOIN 
                    NearMissCategories nmc on nm.CategoryId = nmc.Id
                WHERE   
                     Replace(Replace(LTRIM(RTRIM(SUBSTRING(SafetyNearMiss, len(nmc.Description)+1, 100))), char(10), ''), char(13), '') <> ''
                ORDER BY 
                    1, 2");
            // Update records for subcategories
            Execute.Sql(@"
                UPDATE
                    NearMisses
                SET
                    SubCategoryId = nmsc.Id
                FROM
                    NearMisses nm
                JOIN 
                    NearMissCategories nmc on nm.CategoryId = nmc.Id
                JOIN
                    NearMissSubCategories nmsc on nmsc.Description = Replace(Replace(LTRIM(RTRIM(SUBSTRING(SafetyNearMiss, len(nmc.Description)+1, 100))), char(10), ''), char(13), '') and nmsc.CategoryId = nmc.Id
                WHERE   
                     Replace(Replace(LTRIM(RTRIM(SUBSTRING(SafetyNearMiss, len(nmc.Description)+1, 100))), char(10), ''), char(13), '') <> ''
                ");
            Delete.Column("SafetyNearMiss").FromTable("NearMisses");

            //These are all new categories and sub categories we don't have yet.
            AddEnvironmentalCategoriesAndSubCategories();
        }

        private void AddEnvironmentalCategoriesAndSubCategories()
        {
            Action<string, int> addCategory = (desc, typeId) => {
                Execute.Sql($"INSERT INTO NearMissCategories Values('{desc}', {typeId})");
            };
            Action<string, string> addSubCategory = (desc, category) => {
                Execute.Sql(
                    $"INSERT INTO NearMissSubCategories SELECT '{desc}', Id FROM NearMissCategories where Description = '{category}';");
            };
            addCategory("Air", 1);
            addCategory("Chemical Delivery and Storage", 1);
            addCategory("Drinking Water - Distribution System", 1);
            addCategory("Drinking Water - Sample collection, analysis, and reporting", 1);
            addCategory("Drinking Water - Source & Treatment", 1);
            addCategory("Environmental", 1);
            addCategory("Stormwater", 1);
            addCategory("Wastewater - Collection", 1);
            addCategory("Wastewater - Sample Collection, Analysis, &Reporting", 1);
            addCategory("Wastewater - Treatment & Discharge", 1);

            //addSubCategory("bah", "foo");
            addSubCategory("Generator Log", "Air");
            addSubCategory("Generator Testing", "Air");
            addSubCategory("Other (please describe in comments)", "Air");
            addSubCategory(
                "Chemical Delivery Issues (improper paperwork, incorrect chemical, delivery equipment, or location)",
                "Chemical Delivery and Storage");
            addSubCategory("Other (please describe in comments)", "Chemical Delivery and Storage");
            addSubCategory("Spill \\ leak", "Chemical Delivery and Storage");
            addSubCategory(
                "Drinking Water Emergencies (e.g. issues related to isolation\\disinfection\\service restoration)",
                "Drinking Water - Distribution System");
            addSubCategory("Other (please describe in comments)", "Drinking Water - Distribution System");
            addSubCategory("Potential backflow incident", "Drinking Water - Distribution System");
            addSubCategory("Potential microbial concern", "Drinking Water - Distribution System");
            addSubCategory("Tank management", "Drinking Water - Distribution System");
            addSubCategory("Other (please describe in comments)",
                "Drinking Water - Sample collection, analysis, and reporting");
            addSubCategory(
                "Sample analysis (lost\\broken sample, sample hold time\\temp, etc.) that does not lead to an NOV",
                "Drinking Water - Sample collection, analysis, and reporting");
            addSubCategory("Sample collection", "Drinking Water - Sample collection, analysis, and reporting");
            addSubCategory("Sample reporting that does not lead to NOV",
                "Drinking Water - Sample collection, analysis, and reporting");
            addSubCategory("Sample result (proximity to MCL\\AL, recognition of need to take additional action)",
                "Drinking Water - Sample collection, analysis, and reporting");
            addSubCategory("Equipment \\ instrumentation malfunction", "Drinking Water - Source & Treatment");
            addSubCategory("Other (please describe in comments)", "Drinking Water - Source & Treatment");
            addSubCategory("Overfeed or underfeed of treatment chemical", "Drinking Water - Source & Treatment");
            addSubCategory("Treatmnet irregularities due to source water conditions",
                "Drinking Water - Source & Treatment");
            addSubCategory("Administrative (other communication, fee, or permitting issues)", "Environmental");
            addSubCategory("Air Quality or Water Allocation Monitoring failures", "Environmental");
            addSubCategory("Capital Project Permits and Approvals", "Environmental");
            addSubCategory("Community Right to Know\\Hazard Communication issues", "Environmental");
            addSubCategory("Expired\\Missing Operating Permits", "Environmental");
            addSubCategory("Licensed Operator", "Environmental");
            addSubCategory("Municipal, Industrial, or Hazardous Waste", "Environmental");
            addSubCategory("Other (please describe in comments)", "Environmental");
            addSubCategory("Erosion Controls", "Stormwater");
            addSubCategory("Other (please describe in comments)", "Stormwater");
            addSubCategory("Collection system issue ( grease\\rags\\roots, sewer line collapse, flow-related issues)",
                "Wastewater - Collection");
            addSubCategory("Equipment \\ instrumentation malfunction", "Wastewater - Collection");
            addSubCategory("Mechanical issue at lift station", "Wastewater - Collection");
            addSubCategory("Operational issue at lift station", "Wastewater - Collection");
            addSubCategory("Other (please describe in comments)", "Wastewater - Collection");
            addSubCategory("Unauthorized discharge", "Wastewater - Collection");
            addSubCategory("Other (please describe in comments)",
                "Wastewater - Sample Collection, Analysis, & Reporting");
            addSubCategory("Sample collection", "Wastewater - Sample Collection, Analysis, & Reporting");
            addSubCategory("Sample reporting", "Wastewater - Sample Collection, Analysis, & Reporting");
            addSubCategory("Equipment \\ instrumentation malfunction", "Wastewater - Treatment & Discharge");
            addSubCategory("Mechanical issue at treatment plant", "Wastewater - Treatment & Discharge");
            addSubCategory("Operational issue at treatment plant", "Wastewater - Treatment & Discharge");
            addSubCategory("Other (please describe in comments)", "Wastewater - Treatment & Discharge");
            addSubCategory("Wastewater influent quality \\ in-plant quality issue",
                "Wastewater - Treatment & Discharge");
            addSubCategory("General Housekeeping", "Work Area - Company Facility");
        }

        public override void Down()
        {
            Alter.Table("NearMisses").AddColumn("SafetyNearMiss").AsAnsiString(100).Nullable();
            Execute.Sql(@"
                UPDATE
                    NearMisses
                SET
                    SafetyNearMiss = rtrim(ltrim(isNull(nmc.Description,'') + ' ' + isNull(nmsc.Description,'')))
                FROM
                    NearMisses nm
                LEFT JOIN 
                    NearMissCategories nmc on nm.CategoryId = nmc.Id
                LEFT JOIN
                    NearMissSubCategories nmsc ON nm.SubCategoryId = nmsc.Id");
            Delete.ForeignKeyColumn("NearMisses", "SubCategoryId", "NearMissSubCategories");
            Delete.ForeignKeyColumn("NearMisses", "CategoryId", "NearMissCategories");
            Delete.ForeignKeyColumn("NearMisses", "TypeId", "NearMissTypes");
            Delete.Table("NearMissSubcategories");
            Delete.Table("NearMissCategories");
            Delete.Table("NearMissTypes");
        }
    }
}
