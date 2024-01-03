using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231005151246962), Tags("Production")]
    public class MC6173_AddPlanningPlantWasteWaterSystemInitialRecords : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 169 And pp.Code = 'S404';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 200 And pp.Code = 'S404';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 201 And pp.Code = 'S404';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 203 And pp.Code = 'S404';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 204 And pp.Code = 'S404';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 199 And pp.Code = 'S407';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 137 And pp.Code = 'S451';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 138 And pp.Code = 'S452';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 156 And pp.Code = 'S453';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 247 And pp.Code = 'S504';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 132 And pp.Code = 'S301';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 249 And pp.Code = 'S301';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 34 And pp.Code = 'S314';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 133 And pp.Code = 'S308';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 139 And pp.Code = 'S318';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 173 And pp.Code = 'S321';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 246 And pp.Code = 'S321';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 210 And pp.Code = 'S322';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 154 And pp.Code = 'S319';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 136 And pp.Code = 'S306';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 191 And pp.Code = 'S306';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 208 And pp.Code = 'S306';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 155 And pp.Code = 'S316';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 241 And pp.Code = 'S327';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 29 And pp.Code = 'S313';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 35 And pp.Code = 'S313';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 36 And pp.Code = 'S313';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 40 And pp.Code = 'S313';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 41 And pp.Code = 'S313';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 43 And pp.Code = 'S313';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 44 And pp.Code = 'S313';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 45 And pp.Code = 'S313';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 170 And pp.Code = 'S311';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 167 And pp.Code = 'S551';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 27 And pp.Code = 'S554';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 152 And pp.Code = 'S564';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 215 And pp.Code = 'P566';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 22 And pp.Code = 'S603';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 24 And pp.Code = 'S602';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 185 And pp.Code = 'S605';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 23 And pp.Code = 'S601';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 25 And pp.Code = 'S604';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 209 And pp.Code = 'S386';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 255 And pp.Code = 'S389';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 64 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 67 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 69 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 70 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 71 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 73 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 76 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 77 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 78 And pp.Code = 'S379';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 79 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 80 And pp.Code = 'S379';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 81 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 82 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 83 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 84 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 85 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 86 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 87 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 88 And pp.Code = 'S376';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 89 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 90 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 92 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 96 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 97 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 98 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 99 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 100 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 101 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 103 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 107 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 108 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 112 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 113 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 115 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 116 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 117 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 119 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 120 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 121 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 122 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 123 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 124 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 125 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 126 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 127 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 128 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 129 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 130 And pp.Code = 'S379';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 150 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 214 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 216 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 258 And pp.Code = 'S352';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 74 And pp.Code = 'S367';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 75 And pp.Code = 'S367';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 91 And pp.Code = 'S380';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 111 And pp.Code = 'S380';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 114 And pp.Code = 'S380';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 146 And pp.Code = 'S367';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 147 And pp.Code = 'S375';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 148 And pp.Code = 'S367';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 243 And pp.Code = 'S359';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 168 And pp.Code = 'S358';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 174 And pp.Code = 'S382';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 175 And pp.Code = 'S382';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 186 And pp.Code = 'S382';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 190 And pp.Code = 'S382';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 245 And pp.Code = 'S388';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 260 And pp.Code = 'S390';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 271 And pp.Code = 'S391';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 118 And pp.Code = 'S384';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 242 And pp.Code = 'S387';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 102 And pp.Code = 'S385';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 254 And pp.Code = 'S385';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 262 And pp.Code = 'S385';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 65 And pp.Code = 'S373';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 93 And pp.Code = 'S378';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 142 And pp.Code = 'S368';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 144 And pp.Code = 'S381';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 7 And pp.Code = 'S250';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 14 And pp.Code = 'S250';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 19 And pp.Code = 'S250';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 20 And pp.Code = 'S250';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 5 And pp.Code = 'S251';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 6 And pp.Code = 'S251';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 10 And pp.Code = 'S251';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 13 And pp.Code = 'S251';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 16 And pp.Code = 'S251';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 21 And pp.Code = 'S251';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 2 And pp.Code = 'S252';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 3 And pp.Code = 'S253';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 171 And pp.Code = 'S215';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 149 And pp.Code = 'S129';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 239 And pp.Code = 'S129';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 240 And pp.Code = 'S129';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 52 And pp.Code = 'S139';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 172 And pp.Code = 'S154';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 51 And pp.Code = 'S119';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 60 And pp.Code = 'S157';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 157 And pp.Code = 'S121';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 55 And pp.Code = 'S155';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 59 And pp.Code = 'S162';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 187 And pp.Code = 'S160';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 188 And pp.Code = 'S159';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 192 And pp.Code = 'S117';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 53 And pp.Code = 'S136';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 57 And pp.Code = 'S104';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 62 And pp.Code = 'S104';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 183 And pp.Code = 'S104';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 58 And pp.Code = 'S158';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 63 And pp.Code = 'S161';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 250 And pp.Code = 'S190';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 162 And pp.Code = 'S755';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 163 And pp.Code = 'S756';
Insert Into PlanningPlantsWasteWaterSystems (WasteWaterSystemId,PlanningPlantId) SELECT wws.id,pp.id FROM PlanningPlants AS pp CROSS JOIN WasteWaterSystems AS wws Where wws.ID = 141 And pp.Code = 'S659';
");
        }

        public override void Down()
        {
            /*
             The PlanningPlantsWasteWaterSystems table will be deleted in the migration
             that creates the table if a rollback takes place.
            */
        }
    }
}

