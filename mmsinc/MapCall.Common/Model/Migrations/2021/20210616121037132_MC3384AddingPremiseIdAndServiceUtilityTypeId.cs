using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210616121037132), Tags("Production")]
    public class MC3384AddingPremiseIdAndServiceUtilityTypeId : Migration
    {
        public override void Up()
        {
            Execute.Sql("IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE  CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'Premises' AND TABLE_SCHEMA = 'dbo') ALTER TABLE Premises ADD PRIMARY KEY (Id);");
            Alter.Table("Services").AddForeignKeyColumn("PremiseId", "Premises", nullable: true);
            Alter.Table("ServiceCategories").AddForeignKeyColumn("ServiceUtilityTypeId", "ServiceUtilityTypes", foreignId: "ServiceUtilityTypeID", nullable: true);
            Execute.Sql(@"update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Public Fire Service') where Description = 'Fire Retire Service Only'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Public Fire Service') where Description = 'Fire Service Installation'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Public Fire Service') where Description = 'Fire Service Renewal'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Install Meter Set'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Irrigation') where Description = 'Irrigation New'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Irrigation') where Description = 'Irrigation Renewal'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Replace Meter Set'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Wastewater') where Description = 'Sewer Measurement Only'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Wastewater') where Description = 'Sewer Reconnect'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Wastewater') where Description = 'Sewer Retire Service Only'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Wastewater') where Description = 'Sewer Service Increase Size'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Wastewater') where Description = 'Sewer Service New'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Wastewater') where Description = 'Sewer Service Renewal'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Wastewater') where Description = 'Sewer Service Split'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Measurement Only'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Reconnect'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Relocate Meter Set'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Retire Meter Set Only'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Retire Service Only'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Service Increase Size'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Service New Commercial'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Service New Domestic'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Service Renewal'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Service Split'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Wastewater') where Description = 'Sewer Install Clean Out'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Wastewater') where Description = 'Sewer Replace Clean Out'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Service Renewal Cust Side'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Commercial Record Import'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Domestic Water') where Description = 'Water Domestic Record Import'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Public Fire Service') where Description = 'Fire Service Record Import'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Irrigation') where Description = 'Irrigation Service Record Import'
                          update ServiceCategories set ServiceUtilityTypeId = (select ServiceUtilityTypeID from ServiceUtilityTypes where Description = 'Public Fire Service') where Description = 'Fire Service Reconnect'");
            Execute.Sql(@"UPDATE Services SET PremiseId = P.Id FROM Services S 
                            JOIN Premises P
                                ON S.Installation = P.Installation and S.PremiseNumber = P.PremiseNumber
                            JOIN ServiceCategories SC 
	                            ON SC.ServiceUtilityTypeId = P.ServiceUtilityTypeId 
                            and SC.ServiceCategoryId =  S.ServiceCategoryId");
        }

        public override void Down()
        {
            Delete.Column("PremiseId").FromTable("Services");
            Delete.Column("ServiceUtilityTypeId").FromTable("ServiceCategories");
        }
    }
}

