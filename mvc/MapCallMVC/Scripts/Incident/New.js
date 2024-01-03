﻿var Incident = {

    init: function () {
        $('#IncidentNurseRecommendationType').change(Incident.onIncidentNurseRecommendationTypeChanged);
        $('#EmployeeType').change(Incident.onEmployeeTypeChanged);
        $('#OperatingCenter').change(Incident.onOperatingCenterChanged);
        Incident.onIncidentNurseRecommendationTypeChanged();
        $('fieldset.drugalcohol-fieldset').hide();
    },

    onIncidentNurseRecommendationTypeChanged: function () {
        var recommendedMedicalProvider = $('.recommended-medical-provider');
        var nonMedicalProvider = $('.non-medical-treatment-recommendation');

        recommendedMedicalProvider.hide();
        nonMedicalProvider.hide();

        var selectedRecommendationType = $('#IncidentNurseRecommendationType option:selected').text();

        if (selectedRecommendationType === 'Medical Evaluation Recommended') {
            recommendedMedicalProvider.show();
        }
        if (selectedRecommendationType === 'Non Medical Treatment Recommendation, No Action Required') {
            nonMedicalProvider.show();
        }
    },

    onEmployeeTypeChanged: function () {
        var isEmployee = $('#EmployeeType option:selected').val() === '1';
        Application.toggleField('#Employee', isEmployee);
        Application.toggleField('#ContractorName', !isEmployee);
        Application.toggleField('#ContractorCompany', !isEmployee);
    },

    onOperatingCenterChanged: function () {
        var selectedOperatingCenter = $('#OperatingCenter option:selected').val();
        var roles = $("#drugTestingOperatingCenters").val().split(",");
        if (roles.length > 0 && roles.includes(selectedOperatingCenter)) {
            $('fieldset.drugalcohol-fieldset').show();
        } else {
            $('fieldset.drugalcohol-fieldset').hide();
        }
    },

    getAddress: function () {
        var state = $('#AccidentState option:selected').text();
        var town = $('#AccidentTown option:selected').text();
        if (state == "" || state == "-- Select --") {
            state = "";
            town = "";
        }
        var streetNumber = $('#AccidentStreetNumber').val();
        var streetName = $('#AccidentStreetName').val();
        var ret = streetNumber + ' ' + streetName + ' ' + town + ', ' + state

        return (ret != "  , ") ? ret : "";
    }
};

$(document).ready(Incident.init);