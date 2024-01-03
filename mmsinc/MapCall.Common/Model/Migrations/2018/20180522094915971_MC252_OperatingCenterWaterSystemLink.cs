using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180522094915971), Tags("Production")]
    public class MC252_OperatingCenterWaterSystemLink : Migration
    {
        public override void Up()
        {
            //Creates new table to deal with link between operating center & water systems
            Create.Table("OperatingCenterWaterSystems")
                  .WithIdentityColumn("ID")
                  .WithColumn("OperatingCenterID").AsInt32().NotNullable()
                  .ForeignKey("FK_OperatingCenterWaterSystems_OperatingCenters_OperatingCenterID", "OperatingCenters",
                       "OperatingCenterID")
                  .WithColumn("WaterSystemID").AsInt32().NotNullable()
                  .ForeignKey("FK_OperatingCenterWaterSystems_WaterSystems_Id", "WaterSystems", "Id");

            //Insert values into new table 
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(45, 112)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(39, 96)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(43, 99)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(42, 95)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 63)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(21, 27)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 64)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 65)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(144, 94)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 66)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 67)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(43, 26)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 68)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 69)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 70)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(47, 101)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 71)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 72)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(24, 93)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(48, 57)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 73)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 74)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 75)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 76)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 77)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 78)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 79)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 80)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(50, 56)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(49, 34)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(129, 28)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 81)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 82)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 83)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(25, 89)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 84)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(51, 58)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(128, 29)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(128, 85)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 86)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 87)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(84, 88)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(131, 33)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(53, 36)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(54, 37)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(55, 38)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(135, 39)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(136, 40)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(138, 41)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(52, 42)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(132, 43)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(100, 25)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(57, 44)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(137, 97)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(133, 45)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(133, 46)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(56, 47)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(57, 48)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(133, 49)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(132, 50)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(130, 32)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(134, 51)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(131, 31)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(58, 55)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(59, 103)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(149, 132)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 118)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(150, 148)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(60, 102)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 119)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 120)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(151, 136)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(61, 24)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 121)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(62, 116)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 122)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 123)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 124)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(63, 139)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 125)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(64, 135)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 126)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 127)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(66, 117)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(63, 140)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 128)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(148, 129)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(11, 1)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(13, 2)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(13, 3)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(34, 4)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(11, 5)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(34, 6)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(12, 7)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(14, 8)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(16, 22)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(12, 9)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(13, 10)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(12, 11)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(14, 12)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(13, 13)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(34, 14)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(10, 15)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(14, 16)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(12, 17)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(13, 18)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(16, 19)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(10, 20)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(34, 21)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(20, 53)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(35, 52)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(113, 133)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(122, 59)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(124, 104)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(113, 134)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(72, 143)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(67, 106)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(107, 144)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(103, 114)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(68, 138)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(123, 60)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(116, 130)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(116, 131)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(83, 100)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(115, 145)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(110, 108)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(108, 107)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(69, 23)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(127, 61)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(71, 142)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(125, 105)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(121, 137)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(117, 110)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(115, 146)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(106, 111)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(118, 141)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(115, 147)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(103, 115)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(109, 109)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(70, 62)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(74, 113)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(75, 90)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(139, 91)");
            Execute.Sql("INSERT INTO OperatingCenterWaterSystems(OperatingCenterID, WaterSystemID) VALUES(140, 92)");
        }

        public override void Down()
        {
            //deletes foreign keys and then table
            Delete.ForeignKeyColumn("OperatingCenterWaterSystems", "OperatingCenterID", "OperatingCenters",
                "OperatingCenterID");
            Delete.ForeignKey("FK_OperatingCenterWaterSystems_WaterSystems_Id").OnTable("OperatingCenterWaterSystems");
            Delete.Table("OperatingCenterWaterSystems");
        }
    }
}
