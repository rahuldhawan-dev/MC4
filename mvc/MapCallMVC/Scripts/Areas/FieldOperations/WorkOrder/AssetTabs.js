const AssetTabs = (function ($) {
    const at = {
        initialize: function () {
            ELEMENTS = {
                editLink: '#edit-asset-child'
            },
            at.initButtons();
        },
        initButtons: function() {
            $(ELEMENTS.editLink).on('click', at.editLink_Click);
        },
        editLink_Click: function () {
            alert("The asset edit screen will open in a new window. Please close that when you're done editing to return here.");
            let child = window.open(this.href, '', '');
            child.onunload = at.child_OnUnload;
            return false;
        },
        child_OnUnload: function () {
            if (this.location.href !== 'about:blank') {
                location.reload();
            }
        }
    }
    $(document).ready(at.initialize);
    return at;
})(jQuery);