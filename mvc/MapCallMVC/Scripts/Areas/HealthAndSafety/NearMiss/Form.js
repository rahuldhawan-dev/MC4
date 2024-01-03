var NearMiss = (function ($) {
    var ELEMENTS = {};
    let WORK_ORDER_TYPES = null;
    let ENVIRONMENTAL_NEARMISS_TYPE = null;
    let SEVERITY_TYPE_RED = null;

    var nearmiss = {
        init: function(environmentalNearMissType, workOrderTypes, severityTypeRed) {
            ELEMENTS = {
                type: $("select#Type"),
                systemType: $("select#SystemType"),
                publicWaterSupply: $("select#PublicWaterSupply"),
                publicWaterSupplyRow: $("select#PublicWaterSupply").parent().parent().parent(),
                wasteWaterSystem: $("select#WasteWaterSystem"),
                wasteWaterSystemRow: $("select#WasteWaterSystem").parent().parent().parent(),
                workOrderType: $("select#WorkOrderType"),
                workOrderTypeRow: $("select#WorkOrderType").parent().parent().parent(),
                workOrder: $("input#WorkOrder"),
                workOrderRow: $("input#WorkOrder").parent().parent().parent(),
                productionWorkOrder: $("input#ProductionWorkOrder"),
                productionWorkOrderRow: $("input#ProductionWorkOrder").parent().parent().parent(),
                shortCycleWorkOrder: $("input#ShortCycleWorkOrderNumber"),
                shortCycleWorkOrderRow: $("input#ShortCycleWorkOrderNumber").parent().parent().parent(),
                workOrderNumber: $("input#WorkOrderNumber"),
                workOrderNumberRow: $("input#WorkOrderNumber").parent().parent().parent(),
                seriousInjuryOrFatality: $("input#SeriousInjuryOrFatality"),
                seriousInjuryOrFatalityRow: $("input#SeriousInjuryOrFatality").parent().parent().parent(),
                reportedToRegulator: $("input#ReportedToRegulator"),
                reportedToRegulatorRow: $("input#ReportedToRegulator").parent().parent().parent(),
                lifeSavingRuleTypeRow: $("select#LifeSavingRuleType").parent().parent().parent(),
                severity: $("select#Severity"),
                stopWorkAuthorityPerformed: $("input#StopWorkAuthorityPerformed")
            };
            WORK_ORDER_TYPES = workOrderTypes;
            SEVERITY_TYPE_RED = severityTypeRed;
            ENVIRONMENTAL_NEARMISS_TYPE = environmentalNearMissType;
            nearmiss.initType();
            nearmiss.initSystemType();
            nearmiss.initWorkOrderType();
        },

        initType: function() {
            ELEMENTS.type.on('change', nearmiss.onTypeChange);
            nearmiss.onTypeChange();
        },

        initSystemType: function() {
            ELEMENTS.systemType.on('change', nearmiss.onSystemTypeChanged);
            nearMiss.onSystemTypeChanged();
        },

        initWorkOrderType: function() {
            ELEMENTS.workOrderType.on('change', nearmiss.onWorkOrderTypeChanged);
            nearmiss.onWorkOrderTypeChanged();
        },

        onTypeChange: function() {
            var type = parseInt(ELEMENTS.type.val());
            if (type !== ENVIRONMENTAL_NEARMISS_TYPE) {
                ELEMENTS.systemType.val(null);
                ELEMENTS.publicWaterSupply.val(null);
                ELEMENTS.publicWaterSupplyRow.hide();
                ELEMENTS.wasteWaterSystem.val(null);
                ELEMENTS.wasteWaterSystemRow.hide();
                ELEMENTS.reportedToRegulator.prop("checked", false);
                ELEMENTS.reportedToRegulatorRow.hide();
                ELEMENTS.workOrderTypeRow.show();
                ELEMENTS.seriousInjuryOrFatalityRow.show();
                ELEMENTS.lifeSavingRuleTypeRow.show();

                var workOrderType = parseInt(ELEMENTS.workOrderType.val());
                if (workOrderType === WORK_ORDER_TYPES.tdWorkOrderType) {
                    ELEMENTS.workOrderRow.show();
                } else if (workOrderType === WORK_ORDER_TYPES.productionWorkOrderType) {
                    ELEMENTS.productionWorkOrderRow.show();
                } else if (workOrderType === WORK_ORDER_TYPES.shortCycleWorkOrderType) {
                    ELEMENTS.shortCycleWorkOrderRow.show();
                } else if (workOrderType === WORK_ORDER_TYPES.unknownWorkOrderType) {
                    ELEMENTS.workOrderNumberRow.show();
                }
            } else {
                ELEMENTS.seriousInjuryOrFatality.prop("checked", false);
                ELEMENTS.seriousInjuryOrFatalityRow.hide();
                ELEMENTS.workOrder.val(null);
                ELEMENTS.workOrderTypeRow.hide();
                ELEMENTS.workOrderType.val(null);
                ELEMENTS.workOrderRow.hide();
                ELEMENTS.productionWorkOrder.val(null);
                ELEMENTS.productionWorkOrderRow.hide();
                ELEMENTS.shortCycleWorkOrder.val(null);
                ELEMENTS.shortCycleWorkOrderRow.hide();
                ELEMENTS.workOrderNumber.val(null);
                ELEMENTS.workOrderNumberRow.hide();
                ELEMENTS.reportedToRegulatorRow.show();
                ELEMENTS.lifeSavingRuleTypeRow.hide();
            }
        },

        onSystemTypeChanged: function() {
            var systemType = parseInt(ELEMENTS.systemType.val());
            if (systemType === WORK_ORDER_TYPES.drinkingWaterSystemType) {
                ELEMENTS.wasteWaterSystemRow.hide();
                ELEMENTS.publicWaterSupplyRow.show();
            } else if (systemType === WORK_ORDER_TYPES.wasteWaterSystemType) {
                ELEMENTS.publicWaterSupplyRow.hide();
                ELEMENTS.wasteWaterSystemRow.show();
            } else {
                ELEMENTS.publicWaterSupplyRow.hide();
                ELEMENTS.wasteWaterSystemRow.hide();
            }
        },

        onWorkOrderTypeChanged: function() {
            ELEMENTS.workOrderRow.hide();
            ELEMENTS.productionWorkOrderRow.hide();
            ELEMENTS.shortCycleWorkOrderRow.hide();
            ELEMENTS.workOrderNumberRow.hide();
            const workOrderType = parseInt(ELEMENTS.workOrderType.val(), 10);
            switch (workOrderType) {
                case WORK_ORDER_TYPES.tdWorkorderType:
                    ELEMENTS.workOrderRow.show();
                    ELEMENTS.productionWorkOrder.val(null);
                    ELEMENTS.shortCycleWorkOrder.val(null);
                    ELEMENTS.workOrderNumber.val(null);
                    break;
                case WORK_ORDER_TYPES.productionWorkorderType:
                    ELEMENTS.productionWorkOrderRow.show();
                    ELEMENTS.workOrder.val(null);
                    ELEMENTS.shortCycleWorkOrder.val(null);
                    ELEMENTS.workOrderNumber.val(null);
                    break;
                case WORK_ORDER_TYPES.shortCycleWorkorderType:
                    ELEMENTS.shortCycleWorkOrderRow.show();
                    ELEMENTS.workOrder.val(null);
                    ELEMENTS.productionWorkOrder.val(null);
                    ELEMENTS.workOrderNumber.val(null);
                    break;
                case WORK_ORDER_TYPES.unknownWorkorderType:
                    ELEMENTS.workOrderNumberRow.show();
                    ELEMENTS.workOrder.val(null);
                    ELEMENTS.productionWorkOrder.val(null);
                    ELEMENTS.shortCycleWorkOrder.val(null);
                    break;
                default:
                    ELEMENTS.workOrder.val(null);
                    ELEMENTS.productionWorkOrder.val(null);
                    ELEMENTS.shortCycleWorkOrder.val(null);
                    ELEMENTS.workOrderNumber.val(null);
            }
        },

        validateSeverity: function() {
            var severity = parseInt(ELEMENTS.severity.val());
            if (severity === SEVERITY_TYPE_RED && ELEMENTS.stopWorkAuthorityPerformed.prop('checked') === false) {
                return false;
            }
            return true;
        }
    }

    return nearmiss;
})(jQuery);