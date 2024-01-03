using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170424162148083), Tags("Production")]
    public class CreatePAOperatingCentersForBug3764 : Migration
    {
        public override void Up()
        {
            CreateOperatingCenter("PA21", "McMurray D131", "300 Galley Road", "15317", "1", "5", "5");
            CreateOperatingCenter("PA22", "Mon-Valley D129", "101 Long St.", "15037", "1", "5", "5");
            CreateOperatingCenter("PA23", "Uniontown D130", "72 Coolspring St.", "15401", "1", "5", "5");
            CreateOperatingCenter("PA31", "New Castle D122", "2736 Ellwood Road", "16101", "1", "5", "5");
            CreateOperatingCenter("PA41", "Inidana D120", "1909 Oakland Ave.", "15701", "1", "5", "5");
            CreateOperatingCenter("PA42", "Punxsutawney D124", " PO Box 1096", "15767", "1", "5", "5");
            CreateOperatingCenter("PA43", "CLARION D119", "425 Waterworks Road", "16214", "1", "5", "5");
            CreateOperatingCenter("PA44", "Kittanning D127", "215 N. Jefferson St.", "16201", "1", "5", "5");
            CreateOperatingCenter("PA45", "Warren D125", "PO Box 906", "16365", "1", "5", "5");
            CreateOperatingCenter("PA46", "Kane D121", "PO Box 358", "16735", "1", "5", "5");
            CreateOperatingCenter("PA53", "Abington D101", "1 Zimmerman St.", "18411", "1", "5", "5");
            CreateOperatingCenter("PA54", "Susquehanna D107 ", "1 Zimmerman St.", "18411", "1", "5", "5");
            CreateOperatingCenter("PA55", "Bangor D102", "PO Box 203", "18013", "1", "5", "5");
            CreateOperatingCenter("PA56", "Nazareth D103", "PO Box 207", "18064", "1", "5", "5");
            CreateOperatingCenter("PA57", "Pocono D106", "446 Sterling Road", "18466", "1", "5", "5");
            CreateOperatingCenter("PA59", "Glen Alsace D114", "660 Lincoln Road", "19508", "1", "5", "5");
            CreateOperatingCenter("PA63", "Wyomissing D116", "920 Mountian Home Road", "19608", "1", "5", "5");
            CreateOperatingCenter("PA64", "Royersford D117", "PO Box 585", "19468", "1", "5", "5");
            CreateOperatingCenter("PA66", "Lake Heritage D137", "852 Wesley Drive", "17055", "1", "5", "5");
            CreateOperatingCenter("PA68", "Lehman Pike D104", "Winona Falls Road", "18324", "1", "5", "5");
            CreateOperatingCenter("PA72", "Philipsburg D113", "PO Box 707", "16866", "1", "5", "5");
            CreateOperatingCenter("PA73", "Berwick D108", "PO Box 249", "18603", "1", "5", "5");
            CreateOperatingCenter("PA74", "Frackville D109", " PO Box 23", "17931", "1", "5", "5");
            CreateOperatingCenter("PA77", "Boggs D113", "820 Old 220 Road", "16853", "1", "5", "5");
            CreateOperatingCenter("PA78", "Nittany D113", "454 Nittany Ridge Road", "16841", "1", "5", "5");
            CreateOperatingCenter("PA79", "Wildcat D109", " PO Box 23", "17931", "1", "5", "5");
            CreateOperatingCenter("PA84", "McEwensville D112", "702 S. Front St.", "17847", "1", "5", "5");
        }

        private void CreateOperatingCenter(string operatingCenterCode, string operatingCenterName,
            string mailingAddress, string zipCode, string hydrantInspectionFrequency,
            string largeValveInspectionFrequency, string smallValveInspectionFrequency)
        {
            Execute.Sql(
                $@"DECLARE @newId int;
INSERT INTO OperatingCenters (OperatingCenterCode, OperatingCenterName, MailAdd, MailCSZ, HydrantInspectionFrequency, LargeValveInspectionFrequency, SmallValveInspectionFrequency, HydrantInspectionFrequencyUnitId, LargeValveInspectionFrequencyUnitId, SmallValveInspectionFrequencyUnitId, StateId, MailCo, WorkOrdersEnabled) 
VALUES ('{operatingCenterCode}', '{operatingCenterName}', '{mailingAddress}', '{zipCode}', {hydrantInspectionFrequency}, {largeValveInspectionFrequency}, {smallValveInspectionFrequency}, 4, 4, 4, 3, 'Pennsylvania American Water', 1);

SELECT @newId = @@IDENTITY;

insert into OperatingCenterAssetTypes (OperatingCenterId, AssetTypeId) values(@newId, 1)
insert into OperatingCenterAssetTypes (OperatingCenterId, AssetTypeId) values(@newId, 2)
insert into OperatingCenterAssetTypes (OperatingCenterId, AssetTypeId) values(@newId, 3)
insert into OperatingCenterAssetTypes (OperatingCenterId, AssetTypeId) values(@newId, 4)
insert into OperatingCenterAssetTypes (OperatingCenterId, AssetTypeId) values(@newId, 12)
");
        }

        public override void Down()
        {
            DeleteOperatingCenter("PA21");
            DeleteOperatingCenter("PA22");
            DeleteOperatingCenter("PA23");
            DeleteOperatingCenter("PA31");
            DeleteOperatingCenter("PA41");
            DeleteOperatingCenter("PA42");
            DeleteOperatingCenter("PA43");
            DeleteOperatingCenter("PA44");
            DeleteOperatingCenter("PA45");
            DeleteOperatingCenter("PA46");
            DeleteOperatingCenter("PA53");
            DeleteOperatingCenter("PA54");
            DeleteOperatingCenter("PA55");
            DeleteOperatingCenter("PA56");
            DeleteOperatingCenter("PA57");
            DeleteOperatingCenter("PA59");
            DeleteOperatingCenter("PA63");
            DeleteOperatingCenter("PA64");
            DeleteOperatingCenter("PA66");
            DeleteOperatingCenter("PA68");
            DeleteOperatingCenter("PA72");
            DeleteOperatingCenter("PA73");
            DeleteOperatingCenter("PA74");
            DeleteOperatingCenter("PA77");
            DeleteOperatingCenter("PA78");
            DeleteOperatingCenter("PA79");
            DeleteOperatingCenter("PA84");
        }

        private void DeleteOperatingCenter(string opCode)
        {
            Execute.Sql($@"DECLARE @deleteId int;
SELECT @deleteId = OperatingCenterId FROM OperatingCenters where OperatingCenterCode = '{opCode}';
DELETE FROM OperatingCenterAssetTypes where OperatingCenterId = @deleteId;
DELETE FROM OperatingCenters where OperatingCenterId = @deleteId;");
        }
    }
}
