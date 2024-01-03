////////////////////////////////JQUERY MOCK////////////////////////////////
// TODO: clean this up.  These methods belong solely in this object, not
//       the mock object classes.
function jQueryMock(obj) {
  var mock = [obj];

  mock.toggled = false;

  mock.ready = function() { /* noop */ };

  mock.focus = function() {
    this[0].focus();
  };

  mock.html = function(html) {
    var ret = this[0].innerHTML;
    if (html != null) {
      this[0].innerHTML = html;
    }
    return ret;
  };

  mock.toggle = function() {
    this.toggled = true;
    this[0].toggle();
  };

  mock.val = function(newVal) {
    if (newVal) {
      this[0].val(newVal);
      return this;
    } else {
      return this[0].val();
    }
  };

  mock.text = function(newText) {
    if (obj.text && typeof(obj.text) == 'function') {
      return obj.text(newText);
    }
    else if (obj instanceof MockOption) {
      if (newText) {
        obj.text = newText;
      }
      return obj.text;
    } else {
      if (newText) {
        obj.innerText = newText;
      }
      return obj.innerText;
    }
  };

  mock.show = function() {
    if (!this[0].show) {
      this[0].show = function() {
        this.style.display = '';
      };
    }
    return this[0].show();
  };

  mock.hide = function() {
    if (!this[0].hide) {
      this[0].hide = function() {
        this.style.display = 'none';
      };
    }
    return this[0].hide();
  };

  mock.find = function(str) {
    if (this[0].find) {
      return this[0].find(str);
    }
    throw 'Mocked object does not implement "find".';
  };

  return mock;
}

var _$ = {};

function setup$(str, obj, ctxt) {
  _$[str] = (ctxt == null) ? obj : [obj, ctxt];
}

function $(obj, ctxt) {
  var ret;

  switch (true) {
    case (typeof(obj) == 'string'):
      ret = _$[obj];
      break;
    default:
      ret = obj;
      break;
  }

  // array, should be [str, obj],
  // identifier string and context object
  if (typeof(ret.length) != 'undefined') {
    ret = (ctxt === ret[1]) ? ret[0] : null;
  }

  return jQueryMock(ret);
}

$.ajax = function() {/* noop */};
