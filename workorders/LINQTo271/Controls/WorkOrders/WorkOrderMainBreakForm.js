var WorkOrderMainBreakForm = {
  lbMainBreakInsert_Click: function (e, lb) {
    if (Page_ClientValidate('MainBreakFooter')) {
      var footageReplaced = getServerElementById('txtFootageReplaced');
      if (!footageReplaced.prop('disabled') && footageReplaced.val().length == 0) {
          alert('Please enter Footage Replaced.');
          return false;
      }
      var replacedWith = getServerElementById('ddlReplacedWith');
      if (!replacedWith.prop('disabled') && replacedWith.val().length == 0) {
          alert('Please select Replaced With.');
          return false;
      }
      WorkOrderMainBreakForm.killEvent(e);
      var href = lb.href.replace(/%20/g, ' ');
      lb.href = '#';
      eval(href);
      return false;
    }
  },

  killEvent: function (e) {
    e.cancelBubble = true;
    if (e.stopPropagation) {
      e.stopPropagation();
    }
  },
};

function GlobalFootage(oSrc, args) {
	args.IsValid = false;
}