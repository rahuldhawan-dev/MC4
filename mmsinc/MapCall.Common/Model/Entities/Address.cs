using System;
using System.Text;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Address : IEntity

    {
        #region Consts

        public struct StringLengths
        {
            public const int ADDRESS_1 = CreateAddressesTable.MAX_ADDRESS1_LENGTH,
                             ADDRESS_2 = CreateAddressesTable.MAX_ADDRESS2_LENGTH,
                             ZIP_CODE = CreateAddressesTable.MAX_ZIP_CODE;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string ZipCode { get; set; }

        public virtual Town Town { get; set; }

        public virtual County County => Town != null ? Town.County : null;

        public virtual State State
        {
            get
            {
                var c = County;
                return c != null ? c.State : null;
            }
        }

        #endregion

        #region Private Methods

        private string GetZipCode()
        {
            if (!string.IsNullOrWhiteSpace(ZipCode))
            {
                return ZipCode;
            }

            if (Town != null)
            {
                return Town.Zip;
            }

            return null;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Address1);
            if (!string.IsNullOrWhiteSpace(Address2))
            {
                sb.AppendLine();
                sb.Append(Address2);
            }

            var townStreetZip = string.Format("{0}, {1} {2}", Town, State, GetZipCode()).Trim();

            if (townStreetZip != ",")
            {
                sb.AppendLine();
                sb.Append(townStreetZip);
            }

            return sb.ToString();
        }

        #endregion
    }
}
