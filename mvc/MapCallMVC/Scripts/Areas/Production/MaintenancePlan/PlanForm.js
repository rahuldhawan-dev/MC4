
(($) => {
    var selectors = {
        HasPsmRequirement: '#HasPsmRequirement',
        HasCompanyRequirement: '#HasCompanyRequirement',
        HasRegulatoryRequirement: '#HasRegulatoryRequirement',
        HasOshaRequirement: '#HasOshaRequirement',
        HasOtherCompliance: '#HasOtherCompliance',
        HasACompletionRequirement: '#HasACompletionRequirement'
    };

    var onRequirementChange = () => {

        if ($(selectors.HasPsmRequirement).is(':checked') ||
            $(selectors.HasCompanyRequirement).is(':checked') ||
            $(selectors.HasRegulatoryRequirement).is(':checked') ||
            $(selectors.HasOshaRequirement).is(':checked') ||
            $(selectors.HasOtherCompliance).is(':checked'))
        {
            Application.toggleField(selectors.HasACompletionRequirement, false);
            $(selectors.HasACompletionRequirement).val('False');
        }
        else
        {
            Application.toggleField(selectors.HasACompletionRequirement, true);
        }
    };

    $(document).ready(onRequirementChange);

    $(() => {
        $(selectors.HasPsmRequirement).change(onRequirementChange);
        $(selectors.HasCompanyRequirement).change(onRequirementChange);
        $(selectors.HasRegulatoryRequirement).change(onRequirementChange);
        $(selectors.HasOshaRequirement).change(onRequirementChange);
        $(selectors.HasOtherCompliance).change(onRequirementChange);
    });

})(jQuery);