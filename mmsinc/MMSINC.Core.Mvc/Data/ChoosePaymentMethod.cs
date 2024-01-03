using System.Collections.Generic;
using AuthorizeNet;

namespace MMSINC.Data
{
    public class ChoosePaymentMethod
    {
        #region Properties

        public int Id { get; set; }
        public IList<PaymentProfile> PaymentProfiles { get; set; }
        public string SelectedProfileId { get; set; }

        #endregion
    }
}
