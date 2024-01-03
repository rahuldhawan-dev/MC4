var WorkOrderStreetOpeningPermitCreateForm = {
  AUTOPOPULATE_FIELDS: [
    'LocationStreetNumber', 'LocationStreetName', 'LocationCity', 'LocationState', 'LocationZip', 'ArbitraryIdentifier','PurposeOfOpening'
  ],

  initialize: function () {
    WorkOrderStreetOpeningPermitCreateForm.populateFields();
    WorkOrderStreetOpeningPermitCreateForm.initializeUploader();
  },

  initializeUploader: function () {
    if (window['UploaderInit'] === undefined) return;
    if ($('#file-uploader').length > 0) {
      var uploader = new qq.FineUploader({
        element: $('#file-uploader')[0],
        request: {
           endpoint: '../../Permits/Drawings.asmx/Create',
           params: { permitId: UploaderInit.permitId, workOrderId: UploaderInit.workOrderId }
        },
        validation: {
          allowedExtensions: ['png', 'jpg', 'tif', 'tiff', 'pdf']
        },
        callbacks: {
          onComplete: function () {
            WorkOrderStreetOpeningPermitCreateForm.toggleContinue();
          }
        },
        debug: true
      });
    }
  },

  populateFields: function () {
    if (window['WorkOrderData'] === undefined) return;
    $(WorkOrderStreetOpeningPermitCreateForm.AUTOPOPULATE_FIELDS).each(function (i, str) {
      $('#' + str).val(WorkOrderData[str]);
    });
  },

  toggleContinue: function () {
    $('#continue').toggle(true);
  }
};
