(($) => {
   const ELEMENTS = {
      byInstallationNumberAndOperatingCenter: '#ByInstallationNumberAndOperatingCenter',
      operatingCenter: '#OperatingCenter',
      installation: '#Installation',
      serviceMaterial: '#ServiceMaterial',
      serviceSize: '#ServiceSize',
      customerSideMaterial: '#CustomerSideMaterial',
      customerSideSize: '#CustomerSideSize',
   };
   
   const maybeDefaultMaterialOrSize = (data, dataKey, elementKey) => {
      let dataValue;
      let $element;
      
      if ((dataValue = data[dataKey]) && !($element = $(ELEMENTS[elementKey])).val()) {
         $element.val(dataValue);
      }
   };
   
   const maybeDefaultMaterialsAndSizes = (operatingCenter, installation) => {
      $.ajax({
         type: 'GET',
         async: false,
         url: $(ELEMENTS.byInstallationNumberAndOperatingCenter).val(),
         data: {
            'installation': installation,
            'operatingCenterId': operatingCenter,
         },
         success: function (d) {
            if (!d.Data) {
               return;
            }
            
            [
                ['ServiceMaterialId', 'serviceMaterial'],
                ['ServiceSizeId', 'serviceSize'],
                ['CustomerSideMaterialId', 'customerSideMaterial'],
                ['CustomerSideSizeId', 'customerSideSize']
            ].forEach(arr => maybeDefaultMaterialOrSize(d.Data, arr[0], arr[1]));
         }
      });
   };

   const onInstallationChange = () => {
      const operatingCenter = $(ELEMENTS.operatingCenter).val();
      const installation = $(ELEMENTS.installation).val();

      if (operatingCenter === '' || installation === '') {
         return;
      }

      maybeDefaultMaterialsAndSizes(operatingCenter, installation);
   };


   $(document).ready(() => {
       $(ELEMENTS.installation).change(onInstallationChange);
       // fire the change handler now in case we're coming from a work order
       onInstallationChange();
   });
})(jQuery);