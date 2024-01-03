var WorkOrderDocumentsForm = {
  initialize: function() {
  },

  lbView_click: function(lb) {
    var loc = window.top.location.toString();
    window.top.location = loc.replace(/Views.+$/, 'Views/Documents/DocumentResourceRPCPage.aspx?cmd=view&arg=' + lb.getAttribute('docid'));
    return false;
  },

  fileUploadComplete: function(e) {
    getServerElementById('btnInsert').removeAttr('disabled');
    getServerElementById('txtFileName').val(
        WorkOrderDocumentsForm._getFileFromPath(e.get_inputFile().value.toString()));
  },

  fileUploadStarted: function() {
    getServerElementById('btnInsert').attr('disabled', 'disabled');
  },

  _getFileFromPath: function(path) {
    return path.match(/\\?([^\\]*)$/)[1];
  }
}