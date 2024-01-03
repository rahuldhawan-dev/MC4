using System.Collections.Generic;
using System.Linq;

namespace MapCall.Common.Model.Entities
{
    // TODO: This doesn't really belong in the Entities namespace. Dunno where to put it at the moment.

    /// <summary>
    /// This class contains all the logic for matching roles to a specific module/action/op center and 
    /// deals with wildcard matches and all that junk.
    /// </summary>
    public class RoleMatch
    {
        #region Properties

        /// <summary>
        /// Gets the role action used to filter matches.
        /// </summary>
        public RoleActions Action { get; private set; }

        /// <summary>
        /// Gets the role module used to filter matches.
        /// </summary>
        public RoleModules Module { get; private set; }

        /// <summary>
        /// Gets the operating center used to filter matches if one was supplied.
        /// </summary>
        public OperatingCenter OperatingCenter { get; private set; }

        /// <summary>
        /// Returns the IQueryable filter used to filter matches. If you originally
        /// passed in an non-IQueryable IEnumerable object to the constructor, this will
        /// not be usable for additional query filtering.
        /// </summary>
        public IQueryable<AggregateRole> Matches { get; private set; }

        /// <summary>
        /// Gets the distinct Operating Centers that are found in the matching roles.
        /// NOTE: You should always check HasWildCardMatch to see if a role exists
        /// that is valid for any operating center.
        /// </summary>
        public int[] OperatingCenters
        {
            get
            {
                return Matches.Where(x => x.OperatingCenter != null).Select(x => x.OperatingCenter.Id).Distinct()
                              .ToArray();
            }
        }

        /// <summary>
        /// Gets whether a matching role includes at least one that allows access to
        /// a module/action on any operating center.
        /// </summary>
        public bool HasWildCardMatch
        {
            get { return Matches.Any(r => r.OperatingCenter == null); }
        }

        /// <summary>
        /// Returns true if there are any matching roles.
        /// </summary>
        public bool CanAccessRole => Matches.Any();

        #endregion

        #region Constructor

        /// <summary>
        /// Use this constructor if you are not using RoleMatch to create a filter for use with other queries.
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="module"></param>
        /// <param name="action"></param>
        /// <param name="opCenter"></param>
        public RoleMatch(IEnumerable<AggregateRole> roles, RoleModules module, RoleActions action, OperatingCenter opCenter) :
            // NOTE: It is safe to use AsQueryable on a list/collection. It will not cause the list to somehow query the db again.
            // Unless you're using an IList that comes from NHibernate, in which case this just makes it query the database over
            // and over.
            this(roles.AsQueryable(), module, action, opCenter) { }

        /// <summary>
        /// Use this constructor if you are using the RoleMatch solely for creating additional IQueryable filters.
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="module"></param>
        /// <param name="action"></param>
        /// <param name="opCenter"></param>
        public RoleMatch(IQueryable<AggregateRole> roles, RoleModules module, RoleActions action, OperatingCenter opCenter)
        {
            const int userAdmin = (int)RoleActions.UserAdministrator;

            Action = action;
            Module = module;
            OperatingCenter = opCenter;

            // So we aren't casting over and over in a loop.
            var moduleId = (int)module;
            var actionId = (int)action;

            // HOW A ROLE IS SUPPOSED TO MATCH:
            //    Module MUST BE THE SAME as the one supplied to the constructor
            //    Action MUST match UNLESS 
            //        - The action supplied to the constructor is Read(any action can read)
            //        - The action of the role is UserAdmin(is allowed access for any action)
            //    OperatingCenter MUST match UNLESS 
            //        - One isn't supplied to the constructor
            //        - One IS supplied to the constructor but a role has a null operating center(aka wildcard match)
            
            var roleMatches = roles.Where(x => x.Module.Id == moduleId);

            // Any module is considered a match for action if the action we're looking for is RoleActions.Read.
            if (action != RoleActions.Read)
            {
                roleMatches = roleMatches.Where(x => x.Action.Id == userAdmin || x.Action.Id == actionId);
            }

            if (opCenter != null)
            {
                roleMatches = roleMatches.Where(x => x.OperatingCenter == null /* valid for any operating center */ ||
                                                     x.OperatingCenter.Id == opCenter.Id);
            }

            // NOTE: Now that this class passes in IQueryable, rather than IEnumerable, each one of these properties now
            // does a seperate query to the database if the IQueryable is database-related(rather than a regular List/collection 
            // that has been passed in with .AsQueryable()).
            //
            // NOTE 2: Because this now handles IQueryable, DO NOT set any other properties in the constructor. Everything
            // must be lazy-loaded as to not create unnecessary queries to the database.
            Matches = roleMatches;
        }

        #endregion
    }
}
