// TODO: Kill this. It's only used in two places. Perhaps add this to Application.js instead.
// But first figure out why the two places that use it need to detach from the DOM rather than
// just display:none'ing it. -Ross 10/13/2023
/* 
 * Methods for toggling in a show/hide fashion but detaches nodes from the DOM instead of just display:none'ing them.
 */
(function($) {

    var SAVED_POSITION_DATA_KEY = 'saved position data key';

    $.fn.hideAndSavePosition = function() {
        var $this = $(this);
        var data = {
            parent : $this.parent(),
            index: $this.index()
        };
        $this.data(SAVED_POSITION_DATA_KEY, data);
        $this.detach();
      return $this;
    };

    $.fn.reattachAtSavedPosition = function() {
        var $this = $(this);
        var data = $this.data(SAVED_POSITION_DATA_KEY);
        if (data) {
            var sibs = data.parent.children();
            if (sibs.length === 0 || data.index === 0 || data.index === 1) {
                data.parent.append($this);
            } 
            else {
                sibs.eq(data.index - 1).append($this);
            }
      
            $this.data(SAVED_POSITION_DATA_KEY, null);
        }
      return $this;
    };
})(jQuery);
