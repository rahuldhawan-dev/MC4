(($) => {
  const ELEMENTS = {
    requiresInspection: '#RequiresInspection',
    requiresPainting: '#RequiresPainting'
  };

  const oneOrTheOther = (thisElem, otherElem) => {
    $(thisElem).change(e =>
      $(otherElem).prop('disabled', $('option:selected', e.target).val() !== ''));
  };

  $(() => {
    oneOrTheOther(ELEMENTS.requiresInspection, ELEMENTS.requiresPainting);
    oneOrTheOther(ELEMENTS.requiresPainting, ELEMENTS.requiresInspection);
  });
})(jQuery);