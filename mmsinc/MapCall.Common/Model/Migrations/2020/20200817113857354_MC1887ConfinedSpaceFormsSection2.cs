using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200817113857354), Tags("Production")]
    public class MC1887ConfinedSpaceFormsSection2 : Migration
    {
        public override void Up()
        {
            Alter.Table("ConfinedSpaceForms")
                 .AddForeignKeyColumn("GasMonitorId", "GasMonitors").Nullable()
                 .AddForeignKeyColumn("BumpTestConfirmedByEmployeeId", "tblEmployee", "tblEmployeeId").Nullable()
                 .AddColumn("BumpTestConfirmedAt").AsDateTime().Nullable();

            Create.Table("ConfinedSpaceFormAtmosphericTests")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ConfinedSpaceFormId", "ConfinedSpaceForms").NotNullable().Indexed()
                  .WithForeignKeyColumn("TestedByEmployeeId", "tblEmployee", "tblEmployeeId").NotNullable()
                  .WithColumn("TestedAt").AsDateTime().NotNullable()
                  .WithColumn("OxygenPercentageTop").AsDecimal(5, 2).NotNullable() // Can only be 100.00% at the most.
                  .WithColumn("OxygenPercentageMiddle").AsDecimal(5, 2)
                  .NotNullable() // Can only be 100.00% at the most.
                  .WithColumn("OxygenPercentageBottom").AsDecimal(5, 2)
                  .NotNullable() // Can only be 100.00% at the most.
                  .WithColumn("LowerExplosiveLimitPercentageTop").AsDecimal(5, 2).NotNullable() // Same as above
                  .WithColumn("LowerExplosiveLimitPercentageMiddle").AsDecimal(5, 2).NotNullable() // Same as above
                  .WithColumn("LowerExplosiveLimitPercentageBottom").AsDecimal(5, 2).NotNullable() // Same as above
                  .WithColumn("CarbonMonoxidePartsPerMillionTop").AsInt32().NotNullable()
                  .WithColumn("CarbonMonoxidePartsPerMillionMiddle").AsInt32().NotNullable()
                  .WithColumn("CarbonMonoxidePartsPerMillionBottom").AsInt32().NotNullable()
                  .WithColumn("HydrogenSulfidePartsPerMillionTop").AsInt32().NotNullable()
                  .WithColumn("HydrogenSulfidePartsPerMillionMiddle").AsInt32().NotNullable()
                  .WithColumn("HydrogenSulfidePartsPerMillionBottom").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Index().OnTable("ConfinedSpaceFormAtmosphericTests").OnColumn("ConfinedSpaceFormId");
            Delete.ForeignKeyColumn("ConfinedSpaceFormAtmosphericTests", "TestedByEmployeeId", "tblEmployee",
                "tblEmployeeId");
            Delete.ForeignKeyColumn("ConfinedSpaceFormAtmosphericTests", "ConfinedSpaceFormId", "ConfinedSpaceForms");
            Delete.Table("ConfinedSpaceFormAtmosphericTests");

            Delete.ForeignKeyColumn("ConfinedSpaceForms", "BumpTestConfirmedByEmployeeId", "tblEmployee",
                "tblEmployeeId");
            Delete.Column("BumpTestConfirmedAt").FromTable("ConfinedSpaceForms");
            Delete.ForeignKeyColumn("ConfinedSpaceForms", "GasMonitorId", "GasMonitors");
        }
    }
}
