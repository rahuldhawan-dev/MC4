(($) => {
    AjaxTable.initialize('#streetOpeningPermitsTable');

    const submitPermitButton = $('#submit-new-permit-button');
    const permitUrl = submitPermitButton.attr('data-url');

    submitPermitButton.click(() => {
        const dialogHtml = `
            <dialog style="width:90vw; height:90vh; display: flex; flex-direction: column;">
                <div style="display:flex;">
                    <div style="flex-grow: 1; font-size:18px; font-weight: 700;">Create New Permit</div>
                    <div><button id="create-permit-close-button">Close</button></div>
                </div>
                <iframe id="submit-permit-frame" style="flex-grow: 1; border: none;" src="${permitUrl}"></iframe>
            </dialog>
        `;

        var dialogEl = $(dialogHtml)[0];
        $(document.body).append(dialogEl);

        const closeButton = dialogEl.querySelector('#create-permit-close-button');
        $(closeButton).click(() => {
            $(dialogEl).remove();

            // Force the page to reload so the SOP tab has updated data.
            window.location.reload();
        });

        dialogEl.showModal();
    });
})(jQuery);