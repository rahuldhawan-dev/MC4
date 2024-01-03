const NearMiss = (function ($, Application) {
    let ELEMENTS = null;
    let WORK_ORDER_TYPES = null;
    let EMPLOYEE_TYPE_ID = null;
    let ENVIRONMENTAL_NEARMISS_TYPE = null;
    let SEVERITY_TYPE_RED = null;

    const initReportAnonymously = () => {
        ELEMENTS.reportAnonymously.on('change', onReportAnonymouslyEmployeeTypeChanged);
        onReportAnonymouslyEmployeeTypeChanged();
    };

    const initEmployeeType = () => {
        ELEMENTS.employeeType.on('change', onReportAnonymouslyEmployeeTypeChanged);
        onReportAnonymouslyEmployeeTypeChanged();
    };

    const initType = () => {
        ELEMENTS.type.on('change', onTypeChanged);
        onTypeChanged();
    };

    const initNotCompanyFacility = () => {
        ELEMENTS.notCompanyFacility.on('change', onNotCompanyFacilityChanged);
        onNotCompanyFacilityChanged();
    };

    const initSystemType = () => {
        ELEMENTS.systemType.on('change', onSystemTypeChanged);
        onSystemTypeChanged();
    };

    const initWorkOrderType = () => {
        ELEMENTS.workOrderType.on('change', onWorkOrderTypeChanged);
        onWorkOrderTypeChanged();
    };

    const onReportAnonymouslyEmployeeTypeChanged = () => {
        if (ELEMENTS.reportAnonymously.prop("checked")) {
            ELEMENTS.reportedByRow.hide();
        } else {
            ELEMENTS.reportedByRow.show();
        }

        if (parseInt(ELEMENTS.employeeType.val()) === EMPLOYEE_TYPE_ID) {
            ELEMENTS.contractorCompany.val(null);
            ELEMENTS.contractorName.val(null);
            if (ELEMENTS.reportAnonymously.prop("checked")) {
                ELEMENTS.employee.val(null);
                ELEMENTS.employeeRow.hide();
            } else {
                ELEMENTS.employeeRow.show();
            }
        } else {
            ELEMENTS.employee.val(null);
            ELEMENTS.employeeRow.hide();
        }
    };

    const onTypeChanged = () => {
        const nearMissType = parseInt(ELEMENTS.type.val());
        if (nearMissType !== ENVIRONMENTAL_NEARMISS_TYPE) {
            ELEMENTS.systemType.val(null);
            ELEMENTS.publicWaterSupply.val(null);
            ELEMENTS.publicWaterSupplyRow.hide();
            ELEMENTS.wasteWaterSystem.val(null);
            ELEMENTS.wasteWaterSystemRow.hide();
            ELEMENTS.workOrderTypeRow.show();
            ELEMENTS.seriousInjuryOrFatalityRow.show();
            ELEMENTS.seriousInjuryOrFatalityTypeRow.hide();
            const workOrderType = parseInt(ELEMENTS.workOrderType.val());
            if (workOrderType === WORK_ORDER_TYPES.tdWorkorderType) {
                ELEMENTS.workOrderRow.show();
            } else if (workOrderType === WORK_ORDER_TYPES.productionWorkorderType) {
                ELEMENTS.productionWorkOrderRow.show();
            } else if (workOrderType === WORK_ORDER_TYPES.shortCycleWorkorderType) {
                ELEMENTS.shortCycleWorkOrderRow.show();
            } else if (workOrderType === WORK_ORDER_TYPES.unknownWorkorderType) {
                ELEMENTS.workOrderNumberRow.show();
            }
        } else {
            ELEMENTS.seriousInjuryOrFatality.prop("checked", false);
            ELEMENTS.seriousInjuryOrFatalityRow.hide();
            ELEMENTS.seriousInjuryOrFatalityType.val(null);
            ELEMENTS.seriousInjuryOrFatalityTypeRow.hide();
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
        }

        showOrHideSystemType();
    };

    const onNotCompanyFacilityChanged = () => {
        showOrHideSystemType();
    };

    const onSystemTypeChanged = () => {
        const systemType = parseInt(ELEMENTS.systemType.val());
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
    };

    const onWorkOrderTypeChanged = () => {
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
            ELEMENTS.productionWorkOrder.val(null);
            ELEMENTS.workOrderNumber.val(null);
            ELEMENTS.workOrder.val(null);
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
    };

    const showOrHideSystemType = () => {
        const nearMissType = parseInt(ELEMENTS.type.val());
        if (nearMissType === ENVIRONMENTAL_NEARMISS_TYPE && ELEMENTS.notCompanyFacility.val() === 'True') {
            ELEMENTS.systemTypeRow.show();
            Application.toggleFieldRequiredUiState(ELEMENTS.systemType, true);
        } else {
            ELEMENTS.systemTypeRow.hide();
            Application.toggleFieldRequiredUiState(ELEMENTS.systemType, false);
        }
    };

    return {
        init: (employeeTypeId, environmentalNearMissType, workOrderTypes, severityTypeRed) => {
            ELEMENTS = {
                contractorCompany: $("#ContractorCompany"),
                contractorName: $("#ContractorName"),
                employee: $('#Employee_AutoComplete'),
                employeeRow: $('#Employee_AutoComplete').parent().parent().parent(),
                employeeType: $('#EmployeeType'),
                notCompanyFacility: $("#NotCompanyFacility"),
                productionWorkOrder: $("#ProductionWorkOrder"),
                productionWorkOrderRow: $("#ProductionWorkOrder").parent().parent().parent(),
                publicWaterSupply: $("#PublicWaterSupply"),
                publicWaterSupplyRow: $("#PublicWaterSupply").parent().parent().parent(),
                reportAnonymously: $('#ReportAnonymously'),
                reportedBy: $("#ReportedBy"),
                reportedByRow: $("#ReportedBy").parent().parent().parent(),
                seriousInjuryOrFatality: $("#SeriousInjuryOrFatality"),
                seriousInjuryOrFatalityRow: $("#SeriousInjuryOrFatality").parent().parent().parent(),
                seriousInjuryOrFatalityType: $("#SeriousInjuryOrFatalityType"),
                seriousInjuryOrFatalityTypeRow: $("#SeriousInjuryOrFatalityType").parent().parent().parent(),
                severity: $("#Severity"),
                shortCycleWorkOrder: $("#ShortCycleWorkOrderNumber"),
                shortCycleWorkOrderRow: $("#ShortCycleWorkOrderNumber").parent().parent().parent(),
                stopWorkAuthorityPerformed: $("#StopWorkAuthorityPerformed"),
                systemType: $("#SystemType"),
                systemTypeRow: $("#SystemType").parent().parent().parent(),
                type: $("#Type"),
                wasteWaterSystem: $("#WasteWaterSystem"),
                wasteWaterSystemRow: $("#WasteWaterSystem").parent().parent().parent(),
                workOrder: $("#WorkOrder"),
                workOrderNumber: $("#WorkOrderNumber"),
                workOrderNumberRow: $("#WorkOrderNumber").parent().parent().parent(),
                workOrderRow: $("#WorkOrder").parent().parent().parent(),
                workOrderType: $("#WorkOrderType"),
                workOrderTypeRow: $("#WorkOrderType").parent().parent().parent()
            };
            EMPLOYEE_TYPE_ID = employeeTypeId;
            ENVIRONMENTAL_NEARMISS_TYPE = environmentalNearMissType;
            WORK_ORDER_TYPES = workOrderTypes;
            SEVERITY_TYPE_RED = severityTypeRed;
            ELEMENTS.reportedBy.attr("disabled", "disabled");
            initReportAnonymously();
            initEmployeeType();
            initType();
            initNotCompanyFacility();
            initSystemType();
            initWorkOrderType();
            showOrHideSystemType();
        },

        validateEmployee: () => {
            if (parseInt(ELEMENTS.employeeType.val()) === EMPLOYEE_TYPE_ID && !ELEMENTS.reportAnonymously.prop("checked")) {
                const empVal = ELEMENTS.employee.val();
                if (empVal === '' ||
                    empVal === null ||
                    empVal === undefined) {
                    return false;
                }
            }

            return true;
        },

        validateSystemType: () => {
            const nearMissType = parseInt(ELEMENTS.type.val());
            if (nearMissType === ENVIRONMENTAL_NEARMISS_TYPE && ELEMENTS.notCompanyFacility.val() === 'True') {
                const systemType = ELEMENTS.systemType.val();
                if (systemType === '' ||
                    systemType === null ||
                    systemType === undefined) {
                    return false;
                }
            }

            return true;
        },

        validateSeverity: () => {
            const severity = parseInt(ELEMENTS.severity.val());
            if (severity === SEVERITY_TYPE_RED && ELEMENTS.stopWorkAuthorityPerformed.prop('checked') === false) {
                return false;
            }
            return true;
        }
    };
})(jQuery, Application);
