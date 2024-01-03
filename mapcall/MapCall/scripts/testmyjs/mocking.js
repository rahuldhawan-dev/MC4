// mocking.js
// This script contains all of my DOM mocking code, including a jQuery-esque
// object (because in jQuery $ returns an array).  Also included is a rough
// $ function that should eventually become more of a full-blown IoC container.

// TODO:
// - need more elements
// - elements need tag names
// - the elements can probably share a common base through some inheritance
//   scheme.
// - cleanup and documentation

//////////////////////////////////ELEMENT//////////////////////////////////
function MockElement(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);

  return true;
}

///////////////ELEMENT METHOD REFS///////////////
MockElement._focus = function() {
  this.focused = true;
};


///////////ELEMENT CONSTRUCTOR HELPERS///////////
MockElement.mergeBaseOptions = function(opts, elem) {
  opts = opts || {};
  elem.id = opts.id || '';
  elem.innerHTML = opts.innerHTML || '';
  elem.style = opts.style || {
    display: ''
  };
};

MockElement.hasSelection = function(opts, elem) {
  opts = opts || {};
  elem.options = opts.options || [];
  elem.selectedIndex = (opts.selectedIndex !== undefined) ?
    opts.selectedIndex : -1;
};

MockElement.hasFocus = function(opts, elem) {
  opts = opts || {};
  elem.focused = (opts.focused !== undefined) ?
    opts.focused : false;
  elem.focus = MockElement._focus;
};

MockElement.hasChangeEvent = function(opts, elem) {
  opts = opts || {};
  elem.onchange = opts.onchange || function() { /* noop */ };
};

MockElement.hasClickEvent = function(opts, elem) {
  opts = opts || {};
  elem.onclick = opts.onclick || function() { /* noop */ };
};

MockElement.hasInnerText = function(opts, elem) {
  opts = opts || {};
  elem.innerText = opts.innerText || '';
};

MockElement.hasValue = function(opts, elem) {
  opts = opts || {};
  elem.value = opts.value || '';
};

//////////////////////////////////SELECT///////////////////////////////////
function MockSelect(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);
  MockElement.hasSelection(obj, this);
  MockElement.hasFocus(obj, this);
  MockElement.hasChangeEvent(obj, this);

  return true;
}

MockSelect.prototype.toggle = function() {
  return (this.style.display == 'none') ? this.show() : this.hide();
};

MockSelect.prototype.hide = function() {
  this.style.display = 'none';
  this.visible = false;
};

MockSelect.prototype.show = function() {
  this.style.display = '';
  this.visible = true;
};

MockSelect.prototype.val = function(newVal) {
  return this.options[this.selectedIndex].val(newVal);
};

MockSelect.prototype.tagName = 'select';

////////////////////////////////////DIV////////////////////////////////////
function MockDiv(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);
  MockElement.hasInnerText(obj, this);

  return true;
}

MockDiv.prototype.tagName = 'div';

////////////////////////////////TEXT INPUT/////////////////////////////////
function MockTextInput(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);
  MockElement.hasValue(obj, this);
  MockElement.hasFocus(obj, this);
  MockElement.hasChangeEvent(obj, this);

  return true;
}

MockTextInput.prototype.focus = function() {
  this.focused = true;
};

MockTextInput.prototype.val = function(newValue) {
  if (newValue) {
    this.value = newValue;
  }
  return this.value;
};

MockTextInput.prototype.toString = function() {
  return 'Input value="' + this.val() + '"';
};

MockTextInput.prototype.tagName = 'input';

MockTextInput.prototype.type = 'text';

///////////////////////////////BUTTON INPUT////////////////////////////////
function MockButtonInput(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);
  MockElement.hasValue(obj, this);
  MockElement.hasFocus(obj, this);
  MockElement.hasClickEvent(obj, this);

  return true;
}

MockButtonInput.prototype.focus = function() {
  this.focused = true;
};

MockButtonInput.prototype.tagName = 'input';

MockButtonInput.prototype.type = 'button';

////////////////////////////////////IMG////////////////////////////////////
function MockImage(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);
  MockElement.hasClickEvent(obj, this);
  this.src = obj.src || '';

  return true;
}

MockImage.prototype.tagName = 'img';

///////////////////////////////////TABLE///////////////////////////////////
function MockTable(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);

  return true;
}

MockTable.prototype.tagName = 'table';

/////////////////////////////////TABLE ROW/////////////////////////////////
function MockTableRow(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);

  return true;
}

MockTableRow.prototype.tagName = 'tr';

////////////////////////////////TABLE CELL/////////////////////////////////
function MockTableCell(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);

  return true;
}

MockTableCell.prototype.tagName = 'td';

//////////////////////////////////IFRAME///////////////////////////////////
function MockIFrame(obj) {
  obj = obj || {};
  MockElement.mergeBaseOptions(obj, this);
  this.src = obj.src || '';

  return true;
}

MockIFrame.prototype.tagName = 'iframe';

//////////////////////////////////OPTION///////////////////////////////////
function MockOption(text, value) {
  this.text = text || '';
  this.value = (value !== undefined) ?
    value.toString() : '';

  return true;
}

MockOption.prototype.val = function(newVal) {
  if (newVal) {
    this.value = newVal;
  }
  return this.value;
};

MockOption.prototype.tagName = 'option';

///////////////////////////////////EVENT///////////////////////////////////
function MockEvent(keyCode) {
  this.which = keyCode;
  this.cancelBubble = false;
  this.stopPropagationCalled = false;

  return true;
}

MockEvent.prototype.stopPropagation = function() {
  this.stopPropagationCalled = true;
};
