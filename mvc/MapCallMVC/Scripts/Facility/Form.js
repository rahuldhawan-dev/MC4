var FacilityForm = (function ($) {
    var ELEMENTS = {};
    var likelihoods = {
        '1': 'Low',
        '2': 'Medium',
        '3': 'High'
    };

    var calculateLikelihood = function(condition, performance) {
        switch (condition) {
            case '1':
                return performance === '3' ? '2' : '1';
            case '2':
                return performance;
            case '3':
                return performance === '1' ? '2' : '3';
        }
        return null;
    };

    var calculateRiskScore = function(consequence, likelihood) {
        return parseInt(consequence) * parseInt(likelihood);
    };

    var getStrategyTier = function(riskScore) {
        switch (true) {
            case riskScore > 5:
                return 'Tier 1';
            case riskScore > 2:
                return 'Tier 2';
            default:
                return 'Tier 3';
        }
    };

    var calculateRiskCharacteristics = function() {
        var condition = $('#Condition option:selected').val();
        var performance = $('#Performance option:selected').val();
        var consequence = $('#ConsequenceOfFailure option:selected').val();
        var likelihood, riskScore, strategyTier;

        if (condition === '' || performance === '') {
            likelihood = '';
            riskScore = '';
            strategyTier = '';
        } else {
            likelihood = calculateLikelihood(condition, performance);
            if (consequence === '') {
                riskScore = '';
                strategyTier = '';
            } else {
                riskScore = calculateRiskScore(consequence, likelihood);
                strategyTier = getStrategyTier(riskScore);
            }
        }

        $('div.field div',
            $('label[for="LikelihoodOfFailure"]').parent().parent()).text(likelihoods[likelihood]);
        $('div.field div',
            $('label[for="MaintenanceRiskOfFailure"]').parent().parent()).text(riskScore.toString());
        $('div.field div',
            $('label[for="StrategyTier"]').parent().parent()).text(strategyTier);
    };

    var initRiskCharacteristics = function() {
        $('#Condition, #Performance, #ConsequenceOfFailure').change(calculateRiskCharacteristics);
    };

    var ff = {
        initialize: function() {
            ELEMENTS = {
                functionalLocation: '#FunctionalLocation',
                operatingCenter: '#OperatingCenter',
                planningPlant: '#PlanningPlant',
                rmp: $('#RMP'),
                rmpNumber: $('#RMPNumber'),
                riskBasedCompletedDate: $('#RiskBasedCompletedDate'),
                condition: $('#Condition'),
                performance: $('#Performance'),
                consequenceOfFailure: $('#ConsequenceOfFailure'),
                WaterStress: $('#WaterStress'),
                PointOfEntry: $('#PointOfEntry')
        };
            $(ELEMENTS.operatingCenter).on('change', ff.clearFunctionalLocation);
            $(ELEMENTS.planningPlant).on('change', ff.clearFunctionalLocation);
            $(ELEMENTS.rmp).on('change', ff.toggleRmpNumberField);
            ELEMENTS.PointOfEntry.change(ff.toggleWaterStress);
            initRiskCharacteristics();
            ff.toggleRmpNumberField();
            ff.toggleWaterStress();
        },
        clearFunctionalLocation: function() {
            $(ELEMENTS.functionalLocation).val('');
        },

        toggleWaterStress: () => {
            var isPointOfEntry = ELEMENTS.PointOfEntry.is(':checked');

            Application.toggleField(ELEMENTS.WaterStress, isPointOfEntry);
            ELEMENTS.WaterStress.val(isPointOfEntry);
        },

        toggleRmpNumberField:  () => {
            var isRmp = ELEMENTS.rmp.is(':checked');

            Application.toggleField(ELEMENTS.rmpNumber, isRmp);
            if (!isRmp) {
                ELEMENTS.rmpNumber.val('');
            }
        }
    };
    $(document).ready(ff.initialize);
    return ff;
})(jQuery);