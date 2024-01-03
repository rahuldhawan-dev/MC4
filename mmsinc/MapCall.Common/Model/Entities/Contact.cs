using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    public interface IThingWithContact
    {
        Contact Contact { get; set; }
    }

    [Serializable]
    public class Contact : IEntityWithCreationTimeTracking, IValidatableObject
    {
        #region Consts

        public struct StringLengths
        {
            public const int FIRST_NAME = 255,
                             LAST_NAME = 255,
                             MIDDLE_INITIAL = 1,
                             BUSINESS_PHONE = 20,
                             FAX = 20,
                             MOBILE = 20,
                             HOME_PHONE = 20,
                             EMAIL = 255,
                             COMPANY = 255,
                             CREATED_BY = 50;
        }

        #endregion

        #region Properties

        [Obsolete("Use Id instead.")]
        public virtual int ContactID => Id;

        public virtual int Id { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string BusinessPhoneNumber { get; set; }

        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public virtual string Email { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string FaxNumber { get; set; }

        public virtual string FirstName { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string HomePhoneNumber { get; set; }

        public virtual string LastName { get; set; }
        public virtual string MiddleInitial { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string MobilePhoneNumber { get; set; }

        public virtual Address Address { get; set; }

        #region IThingWithContact references

        // NOTE: IF YOU ADD A PROPERTY MAKE SURE TO ADD IT TO THE THINGSWITHCONTACTS PROPERTY 
        public virtual IList<NotificationConfiguration> NotificationConfigurations { get; protected set; }
        public virtual IList<TownContact> TownContacts { get; protected set; }

        /// <summary>
        /// Returns all the known IThingWithContact references that refer to this
        /// Contact. Order is not guaranteed.
        /// </summary>
        public virtual IEnumerable<IThingWithContact> ThingsWithContacts =>
            Enumerable.Empty<IThingWithContact>()
                      .Union(NotificationConfigurations)
                      .Union(TownContacts);

        #endregion

        #region Logical Properties

        /// <summary>
        /// Returns the Contact's full name, last name first.
        /// </summary>
        public virtual string ContactName => new ContactDisplayItem {
            FirstName = FirstName,
            MiddleInitial = MiddleInitial,
            LastName = LastName
        }.Display;

        public virtual string FullName => string.IsNullOrWhiteSpace(MiddleInitial)
            ? $"{FirstName.Trim()} {LastName.Trim()}"
            : $"{FirstName.Trim()} {MiddleInitial.Trim()} {LastName.Trim()}";

        #endregion

        #endregion

        #region Constructors

        public Contact()
        {
            NotificationConfigurations = new List<NotificationConfiguration>();
            TownContacts = new List<TownContact>();
        }

        #endregion

        #region Public Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public class ContactDisplayItem : DisplayItem<Contact>
    {
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }

        public override string Display => !string.IsNullOrWhiteSpace(MiddleInitial)
            ? $"{LastName.Trim()}, {FirstName.Trim()} {MiddleInitial.Trim()}."
            : $"{LastName.Trim()}, {FirstName.Trim()}";
    }
}
