(function ($) {

	// UTILITIES

	var noop = function () {
		return false;
	};

	var getElementByText = function (tag, text) {
		return $(tag).filter(function (i) { return $(this).text() === text; });
	};

	var getLinkByText = function (text) {
		return getElementByText('a', text);
	};

	var getEditRow = function (href, $row, callback) {
		$.ajax({
			url: href,
			type: 'GET',
			dataType: 'html',
			success: function (result) {
				$row.replaceWith(result);
				var saveButton = getElementByText('button', 'Save');
				$('#inlineEditForm').attr('href', href.replace('Edit', 'Update')).submit(function () {
					saveButton.click();
					return false;
				});
				getLinkByText('Cancel').on('click', onCancelClick);
				saveButton.on('click', onSaveClick).parent().parent().find('input:first').focus();
				toggleAllowEdit(false);
				if (callback) {
					callback();
				}
			}
		});
	};

	// EVENT HANDLERS

	var onSaveClick = function (e) {
		var $this = $(this);
		var $row = $this.parent().parent();
		var $form = $('#inlineEditForm');

		$form.validate();
		$.validator.unobtrusive.parseDynamicContent($form);

		if ($form.valid()) {
			$.ajax({
				url: $form.attr('href'),
				type: 'POST',
				dataType: 'html',
				data: $form.serialize(),
				success: function (result) {
					$row.replaceWith(result);
					toggleAllowEdit(true);
				}
			});
		}

		return false;
	};

	var onCancelClick = function (e) {
		var $this = $(this);
		var $row = $this.parent().parent();
		var href = $('#inlineEditForm').attr('href');

		$.ajax({
			url: href.replace('Update', 'Show'),
			type: 'GET',
			dataType: 'html',
			success: function (result) {
				$row.replaceWith(result);
				$('#inlineEditForm').attr('href', '#');
				toggleAllowEdit(true);
			}
		});

		return false;
	};

	var onEditLinkClick = function (e) {
		var $this = $(this);
		var $row = $this.parent().parent();
		var href = $this.attr('href');

		getEditRow(href, $row);

		return false;
	};

	var onEditCellClick = function (e) {
		var $this = $(this);
		var $row = $this.parent();
		var href = $row.find('a').filter(function () { return $(this).text() === 'Edit' }).attr('href');
		var col = $row.children().index($this);

		// Something to try
		// Get mouse coordinates at click
		var scrollableContent = $('#siteContent');
		var scrollableOffset = scrollableContent.offset();
		var relativeMouseLeft = e.pageX - scrollableOffset.left;

		getEditRow(href, $row, function () {
			var expectedEditableCell = getElementByText('button', 'Save').parent().parent().find('td').eq(col);
			expectedEditableCell.find('input').focus();
		
			var newPosition = expectedEditableCell.position();

			// This calculation is annoying and not particularly friendly.
			// So to scroll the editable element into view where we initially clicked
			// (new element position - relative mouse position + (width of cell / 2))
			// width of cell is needed so the left border isn't the only thing scrolled into view.
			
			var halfCellWidth = expectedEditableCell.width() / 2;
			var scrollLeftAnswer = newPosition.left - relativeMouseLeft + halfCellWidth;
			scrollableContent.scrollLeft(scrollLeftAnswer);

			// There is a UX issue here in that the control is entirely in view, but it's shifted over to the left
			// a bit. This happens because the position is adjusted to the cell width. If the cell is all the way
			// at the edge of the scroll container, we need to scroll it entirely into view. 
		});
	};

	// INITIALIZATION

	var toggleAllowEdit = function (allow) {
		if (allow) {
			getLinkByText('Edit').on('click', onEditLinkClick).show();
			$('.edit-cell').on('click', onEditCellClick);
		} else {
			getLinkByText('Edit').hide();
			$('.edit-cell').off('click');
		}
	};

	$(document).ready(function () { toggleAllowEdit(true); });
})(jQuery);