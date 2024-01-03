var DocumentLinkNew = {
  selectors: {
    documentSearchForm: '#documentSearchForm',
    documentIdSelect: '#documentIdSelect'
  },

  findDocumentsSuccess: function (data) {
    DocumentLinkNew.populateDocumentList(data);
  },

  populateDocumentList: function (data) {
    var sb = [];
    for (var i = 0; i < data.length; ++i) {
      sb.push('<option value="' + data[i].value + '">' + data[i].text + '</option>');
    }
    $(DocumentLinkNew.selectors.documentIdSelect).html(sb.join(''));
  }
};