(($) => {
  const WaterType = {
    initialize: () => {
          $('#WaterType').change(WaterType.onChange);
    },

    onChange: () => {
        const WaterTypeValue = $('#WaterType').val();
        
        if (WaterTypeValue === '1') {
            Application.toggleField('#PublicWaterSupply', true);
            Application.toggleField('#WasteWaterSystem', false);
        }
        else if (WaterTypeValue === '2') {
            Application.toggleField('#PublicWaterSupply', false);
            Application.toggleField('#WasteWaterSystem', true);

        }
        else if (WaterTypeValue === '') {
          Application.toggleField('#PublicWaterSupply', false);
          Application.toggleField('#WasteWaterSystem', false);
        }
        else {
            Application.toggleField('#PublicWaterSupply', true);
            Application.toggleField('#WasteWaterSystem', true);
        }
    }
  };

    $(WaterType.initialize);

})(jQuery);