using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        #region Fields

        private readonly IAuthenticationService<User> _authenticationService;

        #endregion

        #region Constructor

        public ContactRepository(
            ISession session,
            IContainer container,
            IAuthenticationService<User> authenticationService)
            : base(session, container)
        {
            _authenticationService = authenticationService;
        }

        #endregion

        #region Public Methods

        public override Contact Save(Contact entity)
        {
            if (entity.Id == 0)
            {
                entity.CreatedBy = _authenticationService.CurrentUser.UniqueName;
            }

            return base.Save(entity);
        }

        public override IQueryable<Contact> GetAllSorted()
        {
            return base.GetAllSorted().OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.MiddleInitial);
        }

        /// <summary>
        /// Returns true if the Contact can be deleted and is not associated with anything.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public bool CanDelete(Contact contact)
        {
            return !contact.ThingsWithContacts.Any();
        }

        public IEnumerable<Contact> FindByPartialNameMatch(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
            {
                return Enumerable.Empty<Contact>();
            }

            // Trim and then replace the comma in case someone tries searching
            // in the Lastname, Firstname format.
            partialName = partialName.Trim().ToLowerInvariant().Replace(",", "");

            var nameSplit = partialName.SplitOnWhiteSpace().ToArray();
            var matches = FindByPartialFirstNameOrLastNameMatch(nameSplit[0], Linq).ToList();

            // Doing a cap at 3 so someone doesn't try searching 100 letters with a space in between.
            const int SPLIT_CAP = 3;
            for (var i = 1; i < Math.Min(nameSplit.Length, SPLIT_CAP); i++)
            {
                matches = FindByPartialFirstNameOrLastNameMatch(nameSplit[i], matches.AsQueryable()).ToList();
            }

            return matches.Distinct();
        }

        private IEnumerable<Contact> FindByPartialFirstNameOrLastNameMatch(string partialName,
            IQueryable<Contact> filterList)
        {
            return (from c in filterList
                    where c.FirstName.ToLowerInvariant().Contains(partialName)
                          || c.LastName.ToLowerInvariant().Contains(partialName)
                    select c);
        }

        #endregion
    }

    public interface IContactRepository : IRepository<Contact>
    {
        bool CanDelete(Contact contact);
        IEnumerable<Contact> FindByPartialNameMatch(string partialName);
    }
}
