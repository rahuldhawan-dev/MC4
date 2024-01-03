using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using MMSINC.Common;
using MapCall.Common;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Interface;
using Permits.Data.Client.Entities;
using Permits.Data.Client.Repositories;
using StructureMap;

namespace MapCall.Modules.Data
{
    /// <summary>
    /// Webservice for linking to permits.mapcall.net
    /// Provide client-side webservices for mapcall to consume that hook into
    /// permits.mapcall.net's token protected webservices.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class Permits : WebService
    {
        #region Constants

        public struct Urls
        {
            public const string LOCALHOST = "http://localhost:64051";
            public const string PRODUCTION = "https://permits.mapcall.net/";
            public const string STAGING = "https://permits.mapcall.info";
        }
        public const string RSA = "132,40,142,22,40,212,165,4,114,145,37,213,125,175,108,247,134,162,2,144,219,196,74,184,208,111,44,207,119,168,178,73,14,29,209,44,183,28,200,24,251,116,106,221,231,180,15,28,99,167,5,207,25,18,175,78,105,130,84,171,202,6,187,0,46,6,177,118,117,62,26,113,9,173,123,105,202,165,169,127,2,115,158,116,43,121,121,168,161,48,75,30,2,158,4,12,149,191,199,107,201,47,62,162,41,140,86,64,43,170,157,202,203,5,165,127,218,137,89,114,121,31,254,213,0,185,7,1";

        #endregion

        #region Fields

        private IUserRepository _userRepository;
        private User _currentUser;

        #endregion

        #region Properties

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ??
                       (_userRepository = DependencyResolver.Current.GetService<IUserRepository>());
            }
        }

        public User CurrentUser
        {
            get { return _currentUser ?? (_currentUser = LoadCurrentUser()); }
        }

        #endregion

        #region Private Methods

        private User LoadCurrentUser()
        {
            return UserRepository.GetUserByUserName(User.Identity.Name);
        }

        #endregion

        #region Exposed Methods

        public static Uri GetBaseUri ()
        { 
            if (HttpApplicationBase.IsProduction)
                return new Uri(Urls.PRODUCTION);
            if (HttpApplicationBase.IsStaging)
                return new Uri(Urls.STAGING);
            return new Uri(Urls.LOCALHOST);
        }

        #endregion

        #region Web Methods

        [WebMethod]
        public State GetStateId(string name)
        {
            // go call up a webservice on permits to see if the state name exists and return the result
            var repo = new StateRepository(CurrentUser.DefaultOperatingCenter.PermitsUserName);
            return repo.Search(new NameValueCollection {{"name", name}}).FirstOrDefault();
        }

        [WebMethod]
        public County GetCountyId(string name, int stateId)
        {
            // go call up a webservice on permits to see if the county name exists
            // and return the result
            var repo = new CountyRepository(CurrentUser.DefaultOperatingCenter.PermitsUserName);
            return repo.Search(new NameValueCollection {{"name", name}, {"stateId", stateId.ToString()}}).FirstOrDefault();
        }

        [WebMethod]
        public Municipality GetMunicipalityId(string name, int countyId)
        {
            // go call up a webservice on permits to see if the municipality name exists
            // and return the result
            var repo = new MunicipalityRepository(CurrentUser.DefaultOperatingCenter.PermitsUserName);
            return repo.Search(new NameValueCollection {{"name", name}, {"countyId", countyId.ToString()}}).FirstOrDefault();
        }

        #endregion
    }
}
