
var ContactsAutoComplete = function(args) {
    this.instanceId = args.instanceId;

    // These need to be explicitly stored because of its use
    // in UpdatePanels and IE memory leaks and stuff.
    this.instance = jQuery("#" + this.instanceId);
    this.hidden = jQuery("#" + args.hiddenId);
    this.textBox = this.instance.find("input[type='text']");
    this.selectedDiv = this.instance.find(".selected");
    this.searchDiv = this.instance.find(".search");

    this.selectedDiv.bind("click", { instance: this }, this.selectedClick);

    this.setSearchMode(false);

    var that = this;

    // Connects the instance with UpdatePanel disposing
    this.instance[0].dispose = function() { that.dispose };

};
ContactsAutoComplete.initialize = function(args) {
    // Check for existing instance based on
    // some control key. Make sure to dispose of
    // that because otherwise there's gonna end
    // up being memory leaks due to update panels.

    ContactsAutoComplete.instances[args.instanceId] = new ContactsAutoComplete(args);
};
ContactsAutoComplete.instances = [];
var WHAT;
ContactsAutoComplete.prototype = {
    hidden: null,
    instanceId: null,
    instance: null,
    textBox: null,
    searchDiv: null,
    selectedDiv: null,
    searchService: null,
    selectedClick: function(e) { e.data.instance.setSearchMode(true); },
    tbKeyUp: function(e) { if (e.which === 27) { e.data.instance.setSearchMode(false); } },
    tbKeyPress: function(e) {
        // Prevents the autocomplete box from submitting 
        // forms if the user hits enter.
        if (e.which === 13) { e.preventDefault(); }
    },

    setSearchMode: function(isSearchMode) {

        var that = this;

        if (isSearchMode) {
            that.selectedDiv.hide();
            that.searchDiv.show();
            that.searchService = new ContactsService();

            that.textBox.autocomplete({
                height: '200px',
                source: function(request, response) {
                    that.searchService.searchContacts({
                        term: request.term,
                        success: function(data) {
                            response($.map(data, function(item) { return { value: item.Key, label: item.Value }; }));
                        }
                    });
                },
                focus: function(event, ui) {
                    that.textBox.val(ui.item.label);
                    return false;
                },
                select: function(event, ui) {
                    that.hidden.val(ui.item.value);
                    that.instance.find(".selected .label").html(ui.item.label);
                    that.setSearchMode(false);
                    return false;
                }

            });

            that.textBox.bind("keyup", { instance: this }, that.tbKeyUp);
            that.textBox.bind("keypress", that.tbKeyPress);
            that.textBox.focus();

        }
        else {

            that.selectedDiv.show();
            that.searchDiv.hide();
            that.textBox.unbind("keyup", that.tbKeyUp);
            that.textBox.unbind("keypress", that.tbKeyPress);

            that.textBox.autocomplete("destroy");
            if (that.searchService) { that.searchService.abort(); }
        }
    },

    dispose: function() {
        // destroys event handlers.
        this.setSearchMode(false);
        this.selectedDiv.unbind("click", this.selectedClick);
        this.instance[0].dispose = null;
        ContactsAutoComplete.instances[this.instanceId] = null;

        this.instance = null;
        this.textBox = null;
        this.hidden = null;
        this.selectedDiv = null;
    }

};

// ASP required stuff. Obsolete in 4.0.
if (typeof (Sys) !== 'undefined') { Sys.Application.notifyScriptLoaded(); }