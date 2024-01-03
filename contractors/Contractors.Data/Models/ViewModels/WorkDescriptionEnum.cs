using MapCall.Common.Model.Entities;

namespace Contractors.Data.Models.ViewModels {
    public enum WorkDescriptionEnum
    {
        CurbBoxRepair = 5,
        FrozenHydrant = 19,
        ValveBoxRepair = 64,
        ServiceLineRenewal = WorkDescription.Indices.SERVICE_LINE_RENEWAL,
        ServiceLineRetire = WorkDescription.Indices.SERVICE_LINE_RETIRE,
        WaterMainBreakRepair = WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR,
        WaterMainBreakReplace = WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE
    }
}