var noop = function() {};
// legacy test support
if (typeof(mock) != 'undefined' && typeof(jack) == 'undefined') {
  var jack = mock;
}