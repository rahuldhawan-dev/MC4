(($) => {
    const selectors = {
        CentralLabSampleReason: '#CentralLabSamplesReason',
        CentralLabSamplesCollected: '#CentralLabSamplesHaveBeenCollected',
        CentralLabSamplesReported: '#CentralLabSamplesHaveBeenReported',
        ContractedLabsSamplesCollected: '#ContractedLabsSamplesHaveBeenCollected',
        ContractedLabsSamplesReported: '#ContractedLabsSamplesHaveBeenReported',
        ContractedLabSampleReason: '#ContractedLabsSamplesReason',
        InternalLabSamplesCollected: '#InternalLabsSamplesHaveBeenCollected',
        InternalLabSamplesReported: '#InternalLabsSamplesHaveBeenReported',
        InternalLabSamplesReason: '#InternalLabSamplesReason',
        BactiSamplesCollected: '#BactiSamplesHaveBeenCollected',
        BactiSamplesReported: '#BactiSamplesHaveBeenReported',
        BactiReason: '#BactiSamplesReason',
        LeadAndCopperSamplesCollected: '#LeadAndCopperSamplesHaveBeenCollected',
        LeadAndCopperSamplesReported: '#LeadAndCopperSamplesHaveBeenReported',
        LeadAndCopperReason: '#LeadAndCopperSamplesReason',
        WQPSamplesCollected: '#WQPSamplesHaveBeenCollected',
        WQPSamplesReported: '#WQPSamplesHaveBeenReported',
        WQPSampleReason: '#WQPSamplesReason',
        SurfaceWaterPlantSamplesCollected: '#SurfaceWaterPlantSamplesHaveBeenCollected',
        SurfaceWaterPlantSamplesReported: '#SurfaceWaterPlantSamplesHaveBeenReported',
        SurfaceWaterPlantReason: '#SurfaceWaterPlantSamplesReason',
        ChlorineResidualsCollected: '#ChlorineResidualsHaveBeenCollected',
        ChlorineResidualsReported: '#ChlorineResidualsHaveBeenReported',
        ChlorineResidualReason: '#ChlorineResidualsReason'
    };

    const fieldChange = (x, y, t) => {
        const selectedCollected = $(`${x} option:selected`).text();
        const selectedReported = $(`${t} option:selected`).text();

        if (selectedCollected === 'No' || selectedReported === 'No') {
            Application.toggleField(y);
        }
        else {
            Application.toggleField(y, false);
            $(y).val('');
        }
    };

    $(() => {
        const initializeFieldChangeAndRunOnce = (answerDropDown, reasonField, refField) => {
            // Need to call function inside of change to be able to pass arguements and not the JQuery event
			const onChange = () => { fieldChange(answerDropDown, reasonField, refField); };
            $(answerDropDown).change(onChange);
            // Need the call directly after for when the page first loads and there is no event
            onChange();
        };

        initializeFieldChangeAndRunOnce(selectors.CentralLabSamplesCollected, selectors.CentralLabSampleReason, selectors.CentralLabSamplesReported);
        initializeFieldChangeAndRunOnce(selectors.ContractedLabsSamplesCollected, selectors.ContractedLabSampleReason, selectors.ContractedLabsSamplesReported);
        initializeFieldChangeAndRunOnce(selectors.InternalLabSamplesCollected, selectors.InternalLabSamplesReason, selectors.InternalLabSamplesReported);
        initializeFieldChangeAndRunOnce(selectors.BactiSamplesCollected, selectors.BactiReason, selectors.BactiSamplesReported);
        initializeFieldChangeAndRunOnce(selectors.LeadAndCopperSamplesCollected, selectors.LeadAndCopperReason, selectors.LeadAndCopperSamplesReported);
        initializeFieldChangeAndRunOnce(selectors.WQPSamplesCollected, selectors.WQPSampleReason, selectors.WQPSamplesReported);
        initializeFieldChangeAndRunOnce(selectors.SurfaceWaterPlantSamplesCollected, selectors.SurfaceWaterPlantReason, selectors.SurfaceWaterPlantSamplesReported);
        initializeFieldChangeAndRunOnce(selectors.ChlorineResidualsCollected, selectors.ChlorineResidualReason, selectors.ChlorineResidualsReported);

        initializeFieldChangeAndRunOnce(selectors.CentralLabSamplesReported, selectors.CentralLabSampleReason, selectors.CentralLabSamplesCollected);
        initializeFieldChangeAndRunOnce(selectors.ContractedLabsSamplesReported, selectors.ContractedLabSampleReason, selectors.ContractedLabsSamplesCollected);
        initializeFieldChangeAndRunOnce(selectors.InternalLabSamplesReported, selectors.InternalLabSamplesReason, selectors.InternalLabSamplesCollected);
        initializeFieldChangeAndRunOnce(selectors.BactiSamplesReported, selectors.BactiReason, selectors.BactiSamplesCollected);
        initializeFieldChangeAndRunOnce(selectors.LeadAndCopperSamplesReported, selectors.LeadAndCopperReason, selectors.LeadAndCopperSamplesCollected);
        initializeFieldChangeAndRunOnce(selectors.WQPSamplesReported, selectors.WQPSampleReason, selectors.WQPSamplesCollected);
        initializeFieldChangeAndRunOnce(selectors.SurfaceWaterPlantSamplesReported, selectors.SurfaceWaterPlantReason, selectors.SurfaceWaterPlantSamplesCollected);
        initializeFieldChangeAndRunOnce(selectors.ChlorineResidualsReported, selectors.ChlorineResidualReason, selectors.ChlorineResidualsCollected);
    });
})(jQuery);