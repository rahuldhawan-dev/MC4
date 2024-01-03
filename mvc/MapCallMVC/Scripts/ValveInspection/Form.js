var ValveInspections = (function () {
	var ELEMENTS = {};
	var vi = {
		init: function () {
			ELEMENTS = {
				minimumRequiredTurns: $('#hidMinimumRequiredTurns'),
				turns: $('#Turns'),
				turnsNotCompleted: $('#TurnsNotCompleted'),
				inspected: $('#Inspected'),
				positionFound: $('#PositionFound'),
				positionLeft: $('#PositionLeft')
			}
		},

		validateTurns: function() {
			var turns = parseInt(ELEMENTS.turns.val(), 10);
		  var minimumRequiredTurns = parseInt(ELEMENTS.minimumRequiredTurns.val(), 10);
		  if (turns < minimumRequiredTurns && !ELEMENTS.turnsNotCompleted.is(':checked')) {
			  return false;
		  }
			return true;
		},

		validatePositionFoundRequired: function () {
			if (ELEMENTS.inspected.val() === 'True' && !ELEMENTS.turnsNotCompleted.is(':checked') && !ELEMENTS.positionFound.find('option:selected').val()) {
				return false;
			}
			return true;
		},

		validatePositionLeftRequired: function () {
			if (ELEMENTS.inspected.val() === 'True' && !ELEMENTS.turnsNotCompleted.is(':checked') && !ELEMENTS.positionLeft.find('option:selected').val()) {
				return false;
			}
			return true;
		}
	};

	$(document).ready(vi.init);

	return vi;
})();