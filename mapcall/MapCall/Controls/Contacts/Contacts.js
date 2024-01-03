var ContactsView = {
    confirmDelete: function() {
        return confirm("Are you sure you want to remove this contact?");
    },
    initModalDialog: function(id) {
      jQuery("#" + id).dialog({ title: 'Contact Information', modal: true });
    }
};


