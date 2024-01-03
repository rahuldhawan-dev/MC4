using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190906094315264), Tags("Production")]
    public class MC1433AddUtilityCompanies : Migration
    {
        #region Constants

        public const string INSERT_UtilityCompanies = @"
INSERT INTO UtilityCompanies
SELECT 'City of Pasadena, CA', StateID FROM States WHERE Abbreviation = 'CA' UNION ALL
SELECT 'Los Angeles Dept of Water & Power/30808', StateID FROM States WHERE Abbreviation = 'CA' UNION ALL
SELECT 'Merced Irrigation District', StateID FROM States WHERE Abbreviation = 'CA' UNION ALL
SELECT 'Pacific Gas & Electric', StateID FROM States WHERE Abbreviation = 'CA' UNION ALL
SELECT 'Pacific Gas & Electric-Departing Load', StateID FROM States WHERE Abbreviation = 'CA' UNION ALL
SELECT 'San Diego Gas & Electric', StateID FROM States WHERE Abbreviation = 'CA' UNION ALL
SELECT 'Shell Energy North America L.P. (Elec)', StateID FROM States WHERE Abbreviation = 'CA' UNION ALL
SELECT 'SMUD', StateID FROM States WHERE Abbreviation = 'CA' UNION ALL
SELECT 'Southern California Edison', StateID FROM States WHERE Abbreviation = 'CA' UNION ALL
SELECT 'Gulf Power', StateID FROM States WHERE Abbreviation = 'FL' UNION ALL
SELECT 'North Georgia EMC', StateID FROM States WHERE Abbreviation = 'GA' UNION ALL
SELECT 'Hawaii Electric Light Co., Inc. (HELCO)', StateID FROM States WHERE Abbreviation = 'HI' UNION ALL
SELECT 'Hawaiian Electric Company (HECO)', StateID FROM States WHERE Abbreviation = 'HI' UNION ALL
SELECT 'Alliant Energy/IPL', StateID FROM States WHERE Abbreviation = 'IA' UNION ALL
SELECT 'MidAmerican Energy Company', StateID FROM States WHERE Abbreviation = 'IA' UNION ALL
SELECT 'Ameren Illinois', StateID FROM States WHERE Abbreviation = 'IL' UNION ALL
SELECT 'Cairo Public Utility Company, IL', StateID FROM States WHERE Abbreviation = 'IL' UNION ALL
SELECT 'Com Ed', StateID FROM States WHERE Abbreviation = 'IL' UNION ALL
SELECT 'Constellation NewEnergy/4640', StateID FROM States WHERE Abbreviation = 'IL' UNION ALL
SELECT 'Illinois Power Marketing', StateID FROM States WHERE Abbreviation = 'IL' UNION ALL
SELECT 'Monroe County Electric Co-Op', StateID FROM States WHERE Abbreviation = 'IL' UNION ALL
SELECT 'Rock Energy Cooperative', StateID FROM States WHERE Abbreviation = 'IL' UNION ALL
SELECT 'Southeastern IL Electric Cooperative', StateID FROM States WHERE Abbreviation = 'IL' UNION ALL
SELECT 'Southwestern Electric Cooperative, Inc.', StateID FROM States WHERE Abbreviation = 'IL' UNION ALL
SELECT 'Clark County REMC', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Crawfordsville Electric Light & Power', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Darlington Municipal UtilityCompanies', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Duke Energy/1326', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Harrison REMC', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Heartland REMC', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Indiana Michigan Power', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Indianapolis Power & Light (IPL)', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Jackson County R E M C', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Johnson County R E M C', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Kankakee Valley R E M C', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Kosciusko R E M C', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'NIPSCO - Northern Indiana Public Serv Co', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Parke County R E M C', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Richmond Power & Light', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Rush Shelby Energy', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'South Central Indiana R E M C', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Tipmont R E M C', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Town of Bargersville, IN', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Vectren Energy Delivery/6250', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'WIN Energy REMC', StateID FROM States WHERE Abbreviation = 'IN' UNION ALL
SELECT 'Blue Grass Energy', StateID FROM States WHERE Abbreviation = 'KY' UNION ALL
SELECT 'Clark Energy Cooperative', StateID FROM States WHERE Abbreviation = 'KY' UNION ALL
SELECT 'Jackson Energy Cooperative', StateID FROM States WHERE Abbreviation = 'KY' UNION ALL
SELECT 'KU-Kentucky UtilityCompanies Company', StateID FROM States WHERE Abbreviation = 'KY' UNION ALL
SELECT 'Owen Electric Cooperative, Inc.', StateID FROM States WHERE Abbreviation = 'KY' UNION ALL
SELECT 'BGE', StateID FROM States WHERE Abbreviation = 'MD' UNION ALL
SELECT 'Constellation NewEnergy/4640', StateID FROM States WHERE Abbreviation = 'MD' UNION ALL
SELECT 'UPPCO-Upper Peninsula Power Co', StateID FROM States WHERE Abbreviation = 'MI' UNION ALL
SELECT 'Ameren Missouri', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Callaway Electric Cooperative', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Carroll Electric Cooperative Corp', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Central Missouri Electric Cooperative', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Co-Mo Electric Cooperative Inc - 219465', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Consolidated Electric Coop, MO', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Cuivre River Electric Cooperative Inc', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Kansas City Power & Light/219330/219703', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Liberty UtilityCompanies - Empire District', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'New-Mac Electric Coop, Inc.', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Ozark Electric Cooperative/MO', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Platte-Clay Electric Cooperative, Inc', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Southwest Electric Cooperative', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Three Rivers Electric Cooperative', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'United Electric Cooperative, Inc-MO', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'White River Valley Electric Cooperative', StateID FROM States WHERE Abbreviation = 'MO' UNION ALL
SELECT 'Atlantic City Electric/13610', StateID FROM States WHERE Abbreviation = 'NJ' UNION ALL
SELECT 'Borough of Seaside Heights Electric Dept', StateID FROM States WHERE Abbreviation = 'NJ' UNION ALL
SELECT 'Direct Energy/70220', StateID FROM States WHERE Abbreviation = 'NJ' UNION ALL
SELECT 'ENGIE Resources', StateID FROM States WHERE Abbreviation = 'NJ' UNION ALL
SELECT 'Jersey Central Power & Light', StateID FROM States WHERE Abbreviation = 'NJ' UNION ALL
SELECT 'PSE&G-Public Service Elec & Gas Co', StateID FROM States WHERE Abbreviation = 'NJ' UNION ALL
SELECT 'Rockland Electric Company (O&R)', StateID FROM States WHERE Abbreviation = 'NJ' UNION ALL
SELECT 'Central Hudson Gas & Electric Co', StateID FROM States WHERE Abbreviation = 'NY' UNION ALL
SELECT 'National Grid - New York/11742', StateID FROM States WHERE Abbreviation = 'NY' UNION ALL
SELECT 'NYSEG-New York State Electric & Gas', StateID FROM States WHERE Abbreviation = 'NY' UNION ALL
SELECT 'Orange and Rockland UtilityCompanies (O&R)', StateID FROM States WHERE Abbreviation = 'NY' UNION ALL
SELECT 'PSEGLI', StateID FROM States WHERE Abbreviation = 'NY' UNION ALL
SELECT 'Adams Electric Cooperative, PA', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Blakely Electric Light Company', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Borough of Ellwood City', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Central Electric Cooperative, Inc./PA', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Citizens Electric Company', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Claverack Rural Electric Coop. Inc.', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Constellation NewEnergy/4640', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Duquesne Light Company', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'ENGIE Resources', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Met-Ed/3687', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'PECO/37629', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Penelec/3687', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Penn Power', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Pike County Light & Power Company', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'PPL Electric UtilityCompanies/Allentown', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Talen Energy Marketing LLC', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'UGI UtilityCompanies Inc', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'United Electric Cooperative, Inc.-PA', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'Watsontown Borough', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'West Penn Power', StateID FROM States WHERE Abbreviation = 'PA' UNION ALL
SELECT 'EPB - Electric Power Board-Chattanooga', StateID FROM States WHERE Abbreviation = 'TN' UNION ALL
SELECT 'Sequachee Valley Electric Cooperative', StateID FROM States WHERE Abbreviation = 'TN' UNION ALL
SELECT 'Dominion VA/NC Power/26543/26666', StateID FROM States WHERE Abbreviation = 'VA' UNION ALL
SELECT 'Northern Neck Electric Cooperative, Inc.', StateID FROM States WHERE Abbreviation = 'VA' UNION ALL
SELECT 'Northern Virginia Electric Cooperative', StateID FROM States WHERE Abbreviation = 'VA' UNION ALL
SELECT 'Rappahannock Electric Coop', StateID FROM States WHERE Abbreviation = 'VA' UNION ALL
SELECT 'Appalachian Power', StateID FROM States WHERE Abbreviation = 'WV' UNION ALL
SELECT 'MonPower/Monongahela Power', StateID FROM States WHERE Abbreviation = 'WV'";

        #endregion

        public override void Up()
        {
            Create.Table("UtilityCompanies")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithForeignKeyColumn("StateId", "States", "StateID");
            Alter.Table("tblFacilities")
                 .AddForeignKeyColumn("UtilityCompanyId", "UtilityCompanies")
                 .AddColumn("UtilityCompanyOther").AsAnsiString(50).Nullable();
            Execute.Sql(INSERT_UtilityCompanies);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblFacilities", "UtilityCompanyId", "UtilityCompanies");
            Delete.Column("UtilityCompanyOther").FromTable("tblFacilities");
            Delete.Table("UtilityCompanies");
        }
    }
}
