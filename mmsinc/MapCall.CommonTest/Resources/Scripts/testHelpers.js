var oldJQuery = jQuery || function (obj) { return obj };
var $ = oldJQuery;
var jQuery = function (obj) {
	if (obj === document) {
		return {
			ready: function (fn) {
				$.documentReadyFn = fn;
			}
		};
	}
	return oldJQuery(obj);
};
for (var x in oldJQuery) {
	jQuery[x] = oldJQuery[x];
}

jQuery.event = {
	remove: noop
};

jQuery.fn = {};