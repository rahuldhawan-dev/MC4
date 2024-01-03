using System;
using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170101095526427), Tags("Production")]
    public class PlantMaintenanceActivityTypesBug3458 : Migration
    {
        public override void Up()
        {
            #region PlantMaintenanceActivityTypes

            Create.Table("PlantMaintenanceActivityTypes")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("Description").AsString(50).NotNullable().Unique()
                  .WithColumn("Code").AsString(3).NotNullable().Unique();

            Action<string, string> add = (code, desc) => {
                Insert.IntoTable("PlantMaintenanceActivityTypes").Row(new {Code = code, Description = desc});
            };

            add("BRB", "Blanket: Mains - Replace");
            add("BRC", "Blanket: Mains - Unsch/Replace");
            add("BRE", "Blanket: New Hyd, Valve, MnHol");
            add("BRF", "Blanket: HydrValvManholeRepl");
            add("BRG", "Blanket: Services&LateralsNew");
            add("BRH", "Blanket: Services&LateralsRepl");
            add("BRI", "Blankets meter new");
            add("BRJ", "Blankets meter replace");
            add("DVA", "Developer Services");
            add("MLH", "Maintenance Hydrant Leak");
            add("MLP", "Maintenance Service Lateral");
            add("MLS", "Maintenance Street Leak");
            add("MLY", "Maintenance Yard Leak");
            add("OLH", "Operations Hydrant Leak");
            add("OLP", "Operations Service Lateral");
            add("OLS", "Operations Street Leak");
            add("OLY", "Operations Yard Leak");
            add("PBC", "Possible Billable Claim");
            add("RBS", "Child WO to PS Project");
            add("RPA", "Line Item A: Mains -– New");
            add("RPB", "Line Item B: mains -Replaced");
            add("RPC", "Line C: Mains-UnschedReplaced");
            add("RPD", "Line D: Mains-Relocated");
            add("RPE", "Line E: HydsValvsManholesNew");
            add("RPF", "Line F: HydsValvsManholesRepl");
            add("RPG", "Line G: Servs&LateralsNew");
            add("RPH", "Line H: Servs&LateralsRepl");
            add("RPI", "Line I: Meters New");
            add("RPJ", "Line J: Meters Replaced");
            add("RPQ", "Line Q: ProcessPlnt Fac&Equip");
            add("RPR", "Line R: CapitalTankRehab/Paint");
            add("RPS", "Line S: Engineering Studies");
            add("RPT", "Capital RP Training");

            #endregion

            #region WorkDescriptions

            Alter.Table("WorkDescriptions")
                 .AddColumn("PlantMaintenanceActivityTypeId").AsInt32().Nullable()
                 .ForeignKey("FK_WorkDescriptions_PlantMaintenanceActivityTypes_PlantMaintenanceActivityTypeId",
                      "PlantMaintenanceActivityTypes", "Id");

            Action<int, string> addToWorkDesc = (workDescId, code) => {
                Execute.Sql(
                    $@"update [WorkDescriptions] set PlantMaintenanceActivityTypeId = (select top 1 Id from PlantMaintenanceActivityTypes where Code = '{code}') where WorkDescriptionId = {workDescId}");
            };

            addToWorkDesc(2, "MLS");
            addToWorkDesc(3, "BRH");
            addToWorkDesc(4, "MLP");
            addToWorkDesc(5, "MLP");
            addToWorkDesc(9, "MLP");
            addToWorkDesc(14, "MLP");
            addToWorkDesc(18, "MLP");
            addToWorkDesc(19, "MLH");
            addToWorkDesc(20, "MLP");
            addToWorkDesc(21, "MLP");
            addToWorkDesc(22, "MLP");
            addToWorkDesc(23, "MLP");
            addToWorkDesc(24, "MLH");
            addToWorkDesc(25, "MLH");
            addToWorkDesc(26, "BRE");
            addToWorkDesc(27, "MLH");
            addToWorkDesc(28, "MLH");
            addToWorkDesc(29, "MLH");
            addToWorkDesc(30, "BRF");
            addToWorkDesc(31, "BRF");
            addToWorkDesc(32, "MLP");
            addToWorkDesc(34, "BRE");
            addToWorkDesc(35, "BRG");
            addToWorkDesc(36, "BRE");
            addToWorkDesc(37, "BRG");
            addToWorkDesc(38, "MLP");
            addToWorkDesc(40, "MLP");
            addToWorkDesc(41, "MLS");
            addToWorkDesc(42, "MLP");
            addToWorkDesc(43, "MLP");
            addToWorkDesc(44, "MLP");
            addToWorkDesc(47, "BRG");
            addToWorkDesc(49, "BRH");
            addToWorkDesc(50, "MLP");
            addToWorkDesc(54, "BRB");
            addToWorkDesc(56, "BRG");
            addToWorkDesc(58, "MLP");
            addToWorkDesc(59, "BRH");
            addToWorkDesc(60, "BRH");
            addToWorkDesc(61, "MLP");
            addToWorkDesc(62, "BRB");
            addToWorkDesc(64, "MLS");
            addToWorkDesc(65, "MLS");
            addToWorkDesc(66, "MLP");
            addToWorkDesc(67, "MLS");
            addToWorkDesc(68, "MLS");
            addToWorkDesc(69, "MLS");
            addToWorkDesc(70, "MLS");
            addToWorkDesc(71, "BRF");
            addToWorkDesc(72, "BRF");
            addToWorkDesc(73, "MLP");
            addToWorkDesc(74, "MLS");
            addToWorkDesc(75, "BRB");
            addToWorkDesc(76, "BRB");
            addToWorkDesc(78, "MLP");
            addToWorkDesc(80, "BRC");
            addToWorkDesc(81, "BRH");
            addToWorkDesc(82, "MLS");
            addToWorkDesc(83, "BRC");
            addToWorkDesc(84, "BRC");
            addToWorkDesc(85, "BRB");
            addToWorkDesc(86, "BRC");
            addToWorkDesc(87, "BRH");
            addToWorkDesc(88, "MLP");
            addToWorkDesc(89, "BRH");
            addToWorkDesc(90, "BRH");
            addToWorkDesc(91, "MLP");
            addToWorkDesc(92, "MLS");
            addToWorkDesc(93, "BRF");
            addToWorkDesc(94, "BRE");
            addToWorkDesc(95, "MLS");
            addToWorkDesc(96, "MLP");
            addToWorkDesc(97, "MLS");
            addToWorkDesc(98, "MLH");
            addToWorkDesc(99, "MLS");
            addToWorkDesc(100, "BRF");
            addToWorkDesc(101, "BRG");
            addToWorkDesc(102, "BRH");
            addToWorkDesc(103, "MLP");
            addToWorkDesc(104, "BRH");
            addToWorkDesc(105, "MLP");
            addToWorkDesc(106, "MLP");
            addToWorkDesc(107, "MLS");
            addToWorkDesc(108, "MLP");
            addToWorkDesc(109, "MLS");
            addToWorkDesc(110, "MLS");
            addToWorkDesc(111, "MLS");
            addToWorkDesc(112, "MLS");
            addToWorkDesc(113, "MLP");
            addToWorkDesc(114, "MLP");
            addToWorkDesc(115, "MLP");
            addToWorkDesc(116, "MLP");
            addToWorkDesc(117, "MLP");
            addToWorkDesc(118, "BRE");
            addToWorkDesc(119, "BRF");
            addToWorkDesc(120, "MLH");
            addToWorkDesc(121, "BRH");
            addToWorkDesc(122, "BRF");
            addToWorkDesc(123, "BRF");
            addToWorkDesc(124, "BRB");
            addToWorkDesc(125, "BRF");
            addToWorkDesc(126, "MLP");
            addToWorkDesc(127, "MLS");
            addToWorkDesc(128, "MLP");
            addToWorkDesc(129, "MLP");
            addToWorkDesc(130, "MLS");
            addToWorkDesc(131, "MLS");
            addToWorkDesc(132, "BRH");
            addToWorkDesc(133, "BRH");
            addToWorkDesc(134, "MLS");
            addToWorkDesc(135, "BRF");
            addToWorkDesc(136, "BRF");
            addToWorkDesc(137, "BRF");
            addToWorkDesc(138, "MLH");
            addToWorkDesc(139, "MLH");
            addToWorkDesc(140, "MLH");
            addToWorkDesc(141, "MLS");
            addToWorkDesc(142, "MLS");
            addToWorkDesc(143, "MLS");
            addToWorkDesc(144, "MLP");
            addToWorkDesc(145, "MLP");
            addToWorkDesc(146, "MLP");
            addToWorkDesc(147, "MLP");
            addToWorkDesc(148, "MLP");
            addToWorkDesc(149, "MLP");
            addToWorkDesc(150, "MLS");
            addToWorkDesc(151, "MLS");
            addToWorkDesc(152, "MLS");
            addToWorkDesc(153, "MLS");
            addToWorkDesc(154, "MLS");
            addToWorkDesc(155, "MLS");
            addToWorkDesc(159, "MLS");
            addToWorkDesc(160, "MLS");
            addToWorkDesc(161, "MLS");
            addToWorkDesc(162, "MLS");
            addToWorkDesc(163, "MLS");
            addToWorkDesc(164, "MLS");
            addToWorkDesc(165, "MLS");
            addToWorkDesc(166, "MLP");
            addToWorkDesc(167, "MLP");
            addToWorkDesc(168, "MLS");
            addToWorkDesc(169, "MLP");
            addToWorkDesc(170, "MLP");
            addToWorkDesc(171, "MLP");
            addToWorkDesc(172, "MLS");
            addToWorkDesc(175, "MLH");
            addToWorkDesc(203, "MLS");
            addToWorkDesc(204, "MLS");
            addToWorkDesc(205, "MLS");
            addToWorkDesc(206, "MLS");
            addToWorkDesc(207, "MLS");
            addToWorkDesc(208, "MLP");
            addToWorkDesc(209, "MLP");
            addToWorkDesc(210, "MLP");
            addToWorkDesc(211, "MLP");
            addToWorkDesc(212, "MLP");
            addToWorkDesc(213, "MLP");
            addToWorkDesc(214, "MLP");
            addToWorkDesc(215, "MLP");
            addToWorkDesc(216, "MLP");
            addToWorkDesc(217, "MLP");
            addToWorkDesc(220, "MLH");
            addToWorkDesc(221, "MLH");
            addToWorkDesc(222, "BRG");
            addToWorkDesc(223, "MLS");
            addToWorkDesc(224, "MLS");
            addToWorkDesc(225, "BRB");
            addToWorkDesc(226, "BRB");
            addToWorkDesc(227, "MLS");

            Alter.Column("PlantMaintenanceActivityTypeId").OnTable("WorkDescriptions").AsInt32().NotNullable();

            #endregion
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_WorkDescriptions_PlantMaintenanceActivityTypes_PlantMaintenanceActivityTypeId")
                  .OnTable("WorkDescriptions");
            Delete.Column("PlantMaintenanceActivityTypeId").FromTable("WorkDescriptions");

            Delete.Table("PlantMaintenanceActivityTypes");
        }
    }
}
