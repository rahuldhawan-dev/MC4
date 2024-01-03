using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160713101748691), Tags("Production")]
    public class MigrationForBug3022 : Migration
    {
        public override void Up()
        {
            Create.Table("ServiceDwellingTypes")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(50).NotNullable().Unique()
                  .WithColumn("WaterGPD").AsInt32().NotNullable()
                  .WithColumn("SewerGPD").AsInt32().NotNullable();

            Action<string, int, int> addType = (desc, water, sewer) => {
                Insert.IntoTable("ServiceDwellingTypes")
                      .Row(new {Description = desc, WaterGPD = water, SewerGPD = sewer});
            };

            addType("SF 1-bedroom", 150, 150);
            addType("SF 2-bedroom", 215, 225);
            addType("SF 3-bedroom", 320, 300);
            addType("SF 4-bedroom", 395, 300);
            addType("SF 5-bedroom", 475, 300);
            addType("Apt 1-bedroom", 120, 110);
            addType("Apt 2-bedroom", 175, 170);
            addType("Apt 3-bedroom", 270, 225);
            addType("Townhouse 1-bedroom", 125, 125);
            addType("Townhouse 2-bedroom", 150, 150);
            addType("Townhouse 3-bedroom", 210, 210);
            addType("Townhouse 4-bedroom", 275, 275);
            addType("High-Rise studio", 80, 80);
            addType("High-Rise 1-bedroom", 100, 100);
            addType("High-Rise 2-bedroom", 160, 160);
            addType("Mobile Home 1-bedroom", 130, 130);
            addType("Mobile Home 2-bedroom", 150, 150);
            addType("Mobile Home 3-bedroom", 260, 260);

            Create.Column("ServiceDwellingTypeId").OnTable("Services")
                  .AsInt32().Nullable()
                  .ForeignKey("FK_Services_ServiceDwellingTypes_ServiceDwellingTypeId", "ServiceDwellingTypes", "Id");

            Create.Column("ServiceDwellingTypeQuantity").OnTable("Services")
                  .AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Services_ServiceDwellingTypes_ServiceDwellingTypeId").OnTable("Services");
            Delete.Column("ServiceDwellingTypeId").FromTable("Services");
            Delete.Column("ServiceDwellingTypeQuantity").FromTable("Services");
            Delete.Table("ServiceDwellingTypes");
        }
    }
}
