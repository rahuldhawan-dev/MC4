var ContactsService = function() { };
ContactsService.prototype = new Service();
ContactsService.initialize = function(servicePath) {
    ContactsService.prototype.servicePath = servicePath;
};

var proto = ContactsService.prototype;

proto.searchContacts = function(jsonArgs) {
    var reqArgs = {
        data: JSON.stringify({ term: jsonArgs.term }),
        success: jsonArgs.success
    };
    this.sendAjaxRequest("SearchContactsAutoComplete", reqArgs);
};


