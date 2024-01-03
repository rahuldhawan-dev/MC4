using System;
using MMSINC.Exceptions;
using WorkOrders.Model;

namespace WorkOrders.Library
{
    /// <summary>
    /// Object suitable for verifying the address properties of classes which
    /// implement IVerifiesAddress.
    /// </summary>
    public class AddressVerifier
    {
        #region Private Members

        private readonly IVerifiesAddress _toValidate;

        #endregion

        #region Properties

        protected IVerifiesAddress ToValidate
        {
            get { return _toValidate; }
        }

        #endregion

        #region Constructors

        private AddressVerifier(IVerifiesAddress entity)
        {
            _toValidate = entity;
        }

        #endregion

        #region Exposed Methods

        private bool TestAddress()
        {
            if (ToValidate.Town == null ||
                (ToValidate.Street == null && ToValidate.NearestCrossStreet == null && ToValidate.TownSection == null))
                return true;

            var townID = ToValidate.Town.TownID;
            Predicate<Street> streetValid = street =>
                (street == null || street.TownID == townID);
            Predicate<TownSection> townSectionValid = section =>
                (section == null || section.TownID == townID);

            #if DEBUG

            var streetIsValid = streetValid(ToValidate.Street);
            var crossStreetIsValid = streetValid(ToValidate.NearestCrossStreet);
            var townSectionIsValid = townSectionValid(ToValidate.TownSection);

            return (streetIsValid && crossStreetIsValid && townSectionIsValid);

            #else

            return (streetValid(ToValidate.Street) &&
                    streetValid(ToValidate.NearestCrossStreet) &&
                    townSectionValid(ToValidate.TownSection));

            #endif
        }

        private void VerifyAddress()
        {
            if (!TestAddress())
                throw new DomainLogicException("Address where street listed is not part of town listed is invalid.");
        }

        #endregion

        #region Exposed Static Methods

        /// <summary>
        /// Tests the address properties of the supplied object.
        /// </summary>
        /// <param name="entity">
        /// IVerifiesAddress object to be tested.
        /// </param>
        /// <returns>
        /// True if object's address is valid, false is not.
        /// </returns>
        public static bool Test(IVerifiesAddress entity)
        {
            return new AddressVerifier(entity).TestAddress();
        }

        /// <summary>
        /// Tests the address properties of the supplied object, throwing an
        /// exception if found to be invalid.
        /// </summary>
        /// <param name="entity">
        /// IVerifiesAddress object to be tested.
        /// </param>
        public static void Verify(IVerifiesAddress entity)
        {
            new AddressVerifier(entity).VerifyAddress();
        }

        #endregion
    }

    /// <summary>
    /// Interface ensuring that an object has the correct properties in order
    /// to have it's address verified by the AddressVerifier object.
    /// </summary>
    public interface IVerifiesAddress
    {
        #region Properties

        /// <summary>
        /// String representing the Street Number portion of an address.
        /// </summary>
        string StreetNumber { get; }

        /// <summary>
        /// Street object representing the Street on which an address
        /// resides.
        /// </summary>
        Street Street { get; }

        /// <summary>
        /// Street object representing the nearest cross street to a given
        /// address.
        /// </summary>
        Street NearestCrossStreet { get; }

        /// <summary>
        /// Town object representing the town in which an address resides.
        /// </summary>
        Town Town { get; }

        /// <summary>
        /// TownSection object representing the town section within the town
        /// in which an address resides.
        /// </summary>
        TownSection TownSection { get; }

        #endregion
    }
}
