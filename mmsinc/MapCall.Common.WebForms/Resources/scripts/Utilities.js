////////////////////////////////////////GENERAL FUNCTIONALITY////////////////////////////////////////

// Attempts to find a control by walking the DOM, starting
// with the supplied /parent/.  /fn/ is a function object
// which should accept an HTML element as an argument, and
// return a boolean value indicating whether the element
// passed meets the search criteria.
function findControl(parent, fn) {
  var current, target;
  for (var i = parent.childNodes.length - 1; i >= 0; --i) {
    current = parent.childNodes[i];
    if (fn(current)) {
      target = current;
    } else if (current.tagName && current.tagName.toLowerCase() == 'select') {
      target = null;
    } else if (current.childNodes.length > 0) {
      target = findControl(current, fn);
    }
    if (target != null) {
      return target;
    }
  }
  return null;
}

// Attepts to find a control by waling the DOM, starting
// with the supplied /parent/.  A RegExp object is used,
// to test that elements id values end with the given
// string /id/.
function findControlByIdPart(parent, id) {
  return findControl(parent, function(elem) {
    if (!elem.id) return false;
    return new RegExp(id + '$').test((elem.id || ''));
  });
}

// Attempts to find the element with the given id part
// starting at document.body.  This is a drop-in
// replacement for the same function that comes with
// the ClientIDRepository, though it works quite differently.
function getServerElementById(id) {
  return $(findControlByIdPart(document.body, id));
}

// Returns the passed /date/ value as a string in the
// form MM/DD/YY.
function toCalendarControlDateString(date) {
  return padLeft((date.getMonth() + 1).toString(), '0', 2) + '/' +
    date.getDate() + '/' +
    date.getFullYear().toString().substr(2, 2);
}

// Pads the passed string /toPad/ with the character
// /patChar/ out to the numerical length /newLen/.
function padLeft(toPad, padChar, newLen) {
  var curLen = toPad.length;
  while (curLen < newLen) {
    toPad = padChar + toPad;
    curLen++;
  }
  return toPad;
}

// Checks the query string to determine if the current
// page is an RPC page.
function isRPCPage() {
  return window.top.location.toString().indexOf('?') > -1;
}

//////////////////////////////////////JQUERY EXTENSIONS/HELPERS//////////////////////////////////////

// Toggles the visibility of the elements in the passed
// jQuery object /elems/ based on the value of the boolean
// /visible/.
function toggleElementArray(visible, elems) {
  var fn = (visible) ? 'show' : 'hide';
  for (var i = elems.length - 1; i >= 0; --i)
    elems[i][fn]();
}

// Returns the string text value of the selected option
// of the specified select element (in a jQuery object).
function getSelectedText(jqSelect) {
  var select = jqSelect[0];
  return (select.selectedIndex <= 0 || select.selectedIndex >= select.options.length) ?
    null : select.options[select.selectedIndex].text;
}

function getParentRow(elem) {
  return (elem.tagName && elem.tagName.toLowerCase() == 'tr') ? elem : getParentRow(elem.parentNode);
}

function StringBuilder() {
  this.ar = [];

  return true;
}

StringBuilder.prototype.append = function(str) {
  this.ar[this.ar.length] = str;
}

StringBuilder.prototype.toString = function(str) {
  str = str || '';
  return this.ar.join(str);
}
