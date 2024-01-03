using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200601135832744), Tags("Production")]
    public class AddPersonnelAreaToCovidIssueMC2260 : Migration
    {
        public override void Up()
        {
            // NOTE: Deleting ReportingLocation and making the PersonnelAreaId column not nullable
            // will need to be done in a separate ticket. There's no guarantee that this migration
            // will run before any other problem records get added. Those will need to be cleaned up
            // after we can ensure no more null/incorrect values can be added via the site.
            Alter.Table("CovidIssues").AddForeignKeyColumn("PersonnelAreaId", "PersonnelAreas").Nullable();

            // 1. Set PersonnelAreaId based on ReportingLocation having an exact match for Description.
            Execute.Sql(
                @"update CovidIssues set PersonnelAreaId = (select Id from PersonnelAreas where Description = ReportingLocation)");

            // 2. If PersonnelAreaId is still missing, set this based on the employee's current PersonnelArea.
            Execute.Sql(
                @"update CovidIssues set PersonnelAreaId = (select PersonnelAreaId from tblEmployee where tblEmployeeID = CovidIssues.EmployeeID) where PersonnelAreaId is null");

            // 3. Correct or fill in the blanks for any missing personnel areas. 
            // This is all data that's been analyzed for correctness with Nicole.
            void correctCovidIssue(int covidIssueId, int personnelAreaId)
            {
                Update.Table("CovidIssues").Set(new {PersonnelAreaId = personnelAreaId}).Where(new {Id = covidIssueId});
            }

            correctCovidIssue(347, 28);
            correctCovidIssue(307, 616);
            correctCovidIssue(250, 648);
            correctCovidIssue(782, 648);
            correctCovidIssue(748, 30);
            correctCovidIssue(288, 200);
            correctCovidIssue(173, 611);
            correctCovidIssue(346, 87);
            correctCovidIssue(600, 1);
            correctCovidIssue(578, 1);
            correctCovidIssue(540, 1);
            correctCovidIssue(166, 1);
            correctCovidIssue(324, 647);
            correctCovidIssue(455, 647);
            correctCovidIssue(456, 647);
            correctCovidIssue(457, 647);
            correctCovidIssue(458, 647);
            correctCovidIssue(459, 647);
            correctCovidIssue(460, 647);
            correctCovidIssue(461, 647);
            correctCovidIssue(462, 647);
            correctCovidIssue(463, 647);
            correctCovidIssue(465, 647);
            correctCovidIssue(466, 647);
            correctCovidIssue(290, 185);
            correctCovidIssue(391, 647);
            correctCovidIssue(768, 609);
            correctCovidIssue(296, 639);
            correctCovidIssue(168, 613);
            correctCovidIssue(749, 626);
            correctCovidIssue(493, 616);
            correctCovidIssue(349, 613);
            correctCovidIssue(292, 620);
            correctCovidIssue(816, 620);
            correctCovidIssue(375, 620);
            correctCovidIssue(149, 609);
            correctCovidIssue(305, 610);
            correctCovidIssue(306, 610);
            correctCovidIssue(315, 224);
            correctCovidIssue(320, 224);
            correctCovidIssue(341, 495);
            correctCovidIssue(793, 628);
            correctCovidIssue(293, 144);
            correctCovidIssue(637, 31);
            correctCovidIssue(452, 214);
            correctCovidIssue(377, 214);
            correctCovidIssue(403, 214);
            correctCovidIssue(68, 535);
            correctCovidIssue(900, 78);
            correctCovidIssue(901, 78);
            correctCovidIssue(902, 78);
            correctCovidIssue(709, 651);
            correctCovidIssue(145, 610);
            correctCovidIssue(230, 610);
            correctCovidIssue(746, 610);
            correctCovidIssue(754, 610);
            correctCovidIssue(771, 610);
            correctCovidIssue(769, 610);
            correctCovidIssue(308, 617);
            correctCovidIssue(317, 609);
            correctCovidIssue(318, 609);
            correctCovidIssue(694, 240);
            correctCovidIssue(622, 649);
            correctCovidIssue(588, 647);
            correctCovidIssue(151, 647);
            correctCovidIssue(160, 609);
            correctCovidIssue(840, 647);
            correctCovidIssue(713, 87);
            correctCovidIssue(413, 87);
            correctCovidIssue(613, 117);
            correctCovidIssue(603, 117);
            correctCovidIssue(110, 117);
            correctCovidIssue(869, 117);
            correctCovidIssue(892, 117);
            correctCovidIssue(838, 117);
            correctCovidIssue(555, 117);
            correctCovidIssue(319, 652);
            correctCovidIssue(104, 535);
            correctCovidIssue(593, 535);
            correctCovidIssue(759, 89);
            correctCovidIssue(660, 610);
            correctCovidIssue(687, 610);
            correctCovidIssue(371, 610);
            correctCovidIssue(689, 650);
            correctCovidIssue(448, 1);
            correctCovidIssue(796, 76);
            correctCovidIssue(792, 76);
            correctCovidIssue(742, 76);
            correctCovidIssue(505, 72);
            correctCovidIssue(736, 499);
            correctCovidIssue(889, 86);
            correctCovidIssue(566, 86);
            correctCovidIssue(385, 146);
            correctCovidIssue(69, 94);
            correctCovidIssue(167, 585);
            correctCovidIssue(314, 620);
            correctCovidIssue(733, 183);
            correctCovidIssue(241, 219);
            correctCovidIssue(27, 191);
            correctCovidIssue(653, 630);

            // valid due to a data import that hasn't reached production
            correctCovidIssue(18, 117);
            correctCovidIssue(36, 117);
            correctCovidIssue(66, 117);
            correctCovidIssue(122, 608);
            correctCovidIssue(136, 610);
            correctCovidIssue(137, 615);
            correctCovidIssue(157, 610);
            correctCovidIssue(171, 535);
            correctCovidIssue(178, 117);
            correctCovidIssue(202, 622);
            correctCovidIssue(203, 611);
            correctCovidIssue(219, 149);
            correctCovidIssue(238, 610);
            correctCovidIssue(239, 610);
            correctCovidIssue(309, 618);
            correctCovidIssue(312, 619);
            correctCovidIssue(313, 619);
            correctCovidIssue(337, 640);
            correctCovidIssue(348, 627);
            correctCovidIssue(368, 628);
            correctCovidIssue(424, 608);
            correctCovidIssue(425, 608);
            correctCovidIssue(436, 618);
            correctCovidIssue(467, 618);
            correctCovidIssue(485, 615);
            correctCovidIssue(486, 535);
            correctCovidIssue(571, 628);
            correctCovidIssue(629, 535);
            correctCovidIssue(636, 117);
            correctCovidIssue(677, 615);
            correctCovidIssue(738, 618);
            correctCovidIssue(765, 627);
            correctCovidIssue(767, 117);
            correctCovidIssue(781, 117);
            correctCovidIssue(863, 639);
            correctCovidIssue(864, 618);
            correctCovidIssue(867, 618);
            correctCovidIssue(870, 646);
            correctCovidIssue(895, 608);
            correctCovidIssue(907, 618);
            correctCovidIssue(912, 625);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("CovidIssues", "PersonnelAreaId", "PersonnelAreas");
        }
    }
}
