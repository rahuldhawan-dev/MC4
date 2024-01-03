using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20171222144202721), Tags("Production")]
    public class WO0000000195516 : Migration
    {
        public override void Up()
        {
            Alter.Table("TapImages").AddForeignKeyColumn("ServiceSizeId", "ServiceSizes");
            Alter.Table("TapImages").AddForeignKeyColumn("ServiceMaterialId", "ServiceMaterials", "ServiceMaterialID");
            //
            Execute.Sql(
                "UPDATE TapImages SET ServiceSize = '3/4' WHERE ServiceSize IN('3/', '3/41', '3/4', '3/4''''', '3''4', '3/4\"', '3/11', '3/4`', '0.75', '0.75', '0.75\\', '0.75`', '0.75c', '0/75', '.75', '3/2', '3/6', '3/8');");
            Execute.Sql(
                "UPDATE TapImages SET ServiceSize = null WHERE ServiceSize IN('/', '\\', '`', '=', '0.', '1/8', '0.01', '0.04', '0.1', '0.125', '4/5', '2-1in', '3', '3.0', '3 1/2', '6/6/1972', '7', '7.0', '9', '9.0', '10/1/03', '11/2', '11', '11 FEET', '11/15/73', '11.0', '12/07/1967', '13', '17', '19', '27', '69', '75', '14 3/4', '31', '34', '34\"', '8/4', 'Copper', 'ductile iron', 'Galvanized', 'killed', 'Lead', 'MEASUREMENTS ONLY', 'NA', 'other', 'plastic', 'Unknow', 'Unknown', 'Unkown', 'X', 'XX', 'XXX', 'XXX`')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceSize = '1' WHERE ServiceSize IN(' 1', '1', '1', '1''', '1''''', '1 & 3/4', '1 3/4', '1\"', '1,0', '1.0', '1.0', '1.0''', '1.0''''', '1.00')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '1/4' WHERE ServiceSize IN('.25', '1/4', '0.25')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceSize = '5/8' WHERE ServiceSize IN('.625', '0.65', '0.625', '5/8', '5/8\"');");
            Execute.Sql(
                "UPDATE TapImages SET ServiceSize = '1/2' WHERE ServiceSize IN('1 /2', '.5', '.50', '0.5', '0.50', '0.6')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceSize = '1 1/2' WHERE ServiceSize IN('1 1.5', '1.5''''', '1 1/2', '1.5', '1.50', '1 1/2')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceSize = '4' WHERE ServiceSize IN('4', '4 1/2', '4\"', '4.', '4.0', '4.0', '4.0''''', '4.0 - FIRELINE', '4.0\"', '4.00', '4.5')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceSize = '2' WHERE ServiceSize IN('2', '2''', '2''''', '2\"', '2.0', '2.0''''', '2.00')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '5' WHERE ServiceSize IN('5', '5.0', '5.8', '5 1/2')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '8' WHERE ServiceSize IN('8', '8\"', '8.0', '8.00')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceSize = '6' WHERE ServiceSize IN('6', '6''''', '6\"', '6.0', '6.0\"', '6.00', '6.o')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '16' WHERE ServiceSize IN('16', '16.0')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '12' WHERE ServiceSize IN('12', '12.0')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '10' WHERE ServiceSize IN('10', '10.0')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '2 1/4' WHERE ServiceSize IN('2.25', '2 1/4')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '1 1/4' WHERE ServiceSize IN('1 1/4', '1.25', '1 114')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '2 1/2' WHERE ServiceSize IN('2 1/2', '2.5', '2.50')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '1 3/4' WHERE ServiceSize IN('1.75')");
            Execute.Sql("UPDATE TapImages SET ServiceSize = '2 3/4' WHERE ServiceSize IN('2 3/4', '2.75')");
            //
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = null WHERE ServiceMaterial IN('GALO', '`', 'CIQ', '0', '.50', '0.5', '\\', '0.75', '1', '2', '6', '5 / 8', '10/23/1968', '11/1/1963', '11/17/1954', '3/31/1999', '3/4', '4', '4/22/02', '5/1/1999', '6/7/1961', '7/13/1988', 'Birmingham Wire Gauge', 'Black Iron', 'Black Pipe', 'blow off valve', 'IRC-034', 'Black Pipe Cement', 'byers', 'Byers pipe', 'Carbon', 'CKI', 'CLP', 'CLP/TAP FOR HYDRANT', 'Copper or Plastic?', 'CT', 'curb', 'curb box', 'Domestic', 'FIRE LINE','FIRELINE', 'Hydrant Info', 'HYDRANT INSTALLATION', 'HYDRANT REPLACED', 'HYDRANT SERVICE', 'IRC - 034', 'KILLED', 'lid', 'Material Salvaged', 'McLane', 'NA', 'NO INFO', 'NO INFO FOUND', 'No Material', 'No Material Used', 'Other', 'Others', 'Red Brass', 'SDR', 'REPLACED HYDRANT', 'Sandspon  Pipe', 'SKETCH FOR GATE VALVE', 'Stainless Steel', 'Steel', 'TAP FOR PRIVATE HYDRANT', 'Unable to read', 'w/l', 'wet cut', 'XXX')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'AC' WHERE ServiceMaterial IN('AC', 'Asbestos Cement')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Carlon' WHERE ServiceMaterial IN('Carlon', 'CARLTON')");
            Execute.Sql("UPDATE TapImages Set ServiceMaterial = 'WICL' WHERE ServiceMaterial IN('WICL', 'WICL PIPE')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Unknown' WHERE ServiceMaterial IN('Uknown','Unknow', 'Unknown', 'Unkown')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Copper' WHERE ServiceMaterial IN('Brass', 'C', 'COPPET', 'cooper', 'COPPERQ', 'cop', 'Copepr', 'Coper', 'COPP','coppe', 'Copper', 'Copper', 'Copper', 'Copper', 'Copper''', 'copper bdg10', 'copper bg9', 'Copper Pipe', 'Copper.', 'Copperv', 'Coppper', 'copppr', 'Cpper','CU', 'Lead and Copper', 'OPPER', 'PLASTIC/ COPPER', 'T.C.', 'TC', 'vetrified copper', 'vitrified copper', 'CO')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Cast Iron' WHERE ServiceMaterial IN('cast', 'Cast Iron', 'Cast Irond', 'CI', 'CI TYTON', 'CI`','CICL', 'CL')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Ductile' WHERE ServiceMaterial IN('d', 'D.I', 'DI', 'DI PIPE', 'DICL', 'DIP', 'ductile','Ductile Iron', 'Ductile Iron Cement Lined', 'Ductile Iron Pipe', 'Ductile Lron', 'Tyron', 'Tyton')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Galvanized' WHERE ServiceMaterial IN('Iron', 'GAL', 'galv', 'Galvanised', 'galvanize', 'Galvanized', 'Galvanized Iorn', 'Galvanized Iron', 'Galvinized', 'Galvonize', 'GI')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Galvanized with Lead Gooseneck' WHERE ServiceMaterial IN('Galvanized, Lead Connections')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Plastic' WHERE ServiceMaterial IN('HDPE', 'HOPE', 'p', 'Pastic', 'PE', 'PL', 'plasitc','PLASTC', 'Plastic', 'plastic', 'Plastic - SERVICE VALVE', 'Plastiv', 'ploy', 'Polyethylene', 'PVC')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Lead' WHERE ServiceMaterial IN('L', 'Lead', 'Lead', 'Lead Line', 'Lead Line (renewed)', 'Leadline', 'Leadlined', 'Leadlined (Renewed)', 'Leadlined Galvanized', 'Leadlined Galvanized (Renewed)', 'Leas', 'LL')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Tubeloy' WHERE ServiceMaterial IN('TUBALOY', 'Tube-Alloy', 'Tubeloy', 'Lead Line', 'Tuboley')");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterial = 'Vitrified Clay' WHERE ServiceMaterial IN('V.C.', 'VC', 'Vitrified Clay', 'CLAY')");
            //
            Execute.Sql(
                "UPDATE TapImages SET ServiceSizeId = ServiceSizes.Id FROM ServiceSizes WHERE ServiceSize = ServiceSizes.ServiceSizeDescription");
            Execute.Sql(
                "UPDATE TapImages SET ServiceSizeId = ServiceSizes.Id FROM ServiceSizes WHERE ServiceSize = cast(ServiceSizes.Size as varchar)");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterialId = ServiceMaterials.ServiceMaterialID FROM ServiceMaterials WHERE ServiceMaterial = ServiceMaterials.Description");
            Execute.Sql(
                "UPDATE TapImages SET ServiceMaterialId = ServiceMaterials.ServiceMaterialID FROM ServiceMaterials WHERE ServiceMaterial = cast(ServiceMaterials.Description as varchar)");

            Delete.Column("ServiceSize").FromTable("TapImages");
            Delete.Column("ServiceMaterial").FromTable("TapImages");
        }

        public override void Down()
        {
            Create.Column("ServiceSize").OnTable("TapImages").AsString(50).Nullable();
            Create.Column("ServiceMaterial").OnTable("TapImages").AsString(255).Nullable();
            // sql to populated these columns from columns we're about to remove
            Execute.Sql(
                "Update TapImages SET ServiceSize = ServiceSizes.ServiceSizeDescription FROM ServiceSizes WHERE TapImages.ServiceSizeId = ServiceSizes.Id");
            Execute.Sql(
                "Update TapImages SET ServiceMaterial = ServiceMaterials.Description FROM ServiceMaterials WHERE TapImages.ServiceMaterialId = ServiceMaterials.ServiceMaterialID");

            Delete.ForeignKeyColumn("TapImages", "ServiceSizeId", "ServiceSizes");
            Delete.ForeignKeyColumn("TapImages", "ServiceMaterialId", "ServiceMaterials", "ServiceMaterialID");
        }
    }
}
