// dualSelectBox.js by Jason Duncan
//
// Contains the clientside classes and functions necessary
// to implement a dual ListBox control.

function PortableObject(text, value, selected) {
  this.text = text;
  this.value = value;
  this.selected = selected;
}

PortableObject.prototype.toOption = function() {
  return new Option(this.text, this.value, false, this.selected);
}

var DualSelectBox = {
  'stealOptions': function(from) {
    var opt, opts = [], i = from.options.length;
    while (i--) {
      opt = from.options[i];
      Array.add(opts, new PortableObject(opt.text, opt.value, opt.selected));
      from.options[i] = null;
    }
    return opts;
  },

  'moveAll': function(from, to) {
    var opts = DualSelectBox.stealOptions(from);
    var opt, i = opts.length;
    while (i--) {
      opt = opts[i];
      to.options[to.options.length] = opt.toOption();
    }
  },

  'moveSelected': function(from, to) {
    var opts = DualSelectBox.stealOptions(from);
    var opt, i = opts.length;
    while (i--) {
      opt = opts[i];
      if (opt.selected)
        to.options[to.options.length] = opt.toOption();
      else
        from.options[from.options.length] = opt.toOption();
    }
  }
}
