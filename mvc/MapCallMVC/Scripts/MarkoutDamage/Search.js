var Search = {
  selectors: {
    werePicturesTakenDropDown: 'select#WerePicturesTaken',
    missingAttachedPicturesRow: 'div.MissingAttachedPictures'
  },
  
  initialize: function () {
    Search.werePicturesTaken_change({
      target: $(Search.selectors.werePicturesTakenDropDown).change(Search.werePicturesTaken_change)[0]
    });
  },

  werePicturesTaken_change: function(e) {
    $(Search.selectors.missingAttachedPicturesRow).toggle($(e.target).val() == 'True');
  }
};

$(document).ready(Search.initialize);