using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallApi.Controllers
{
    public abstract class ApiControllerBaseWithPersistence<TRepository, TEntity, TUser> : ControllerBaseWithPersistence<TRepository, TEntity, TUser>
        where TRepository : class, IRepository<TEntity>
        where TUser : IAdministratedUser
        where TEntity : class
    {
        #region Constructors

        protected ApiControllerBaseWithPersistence(ControllerBaseWithPersistenceArguments<TRepository, TEntity, TUser> args) : base(args) { }

        #endregion

        #region Public Methods

        protected ActionResult GetError()
        {
            var result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            Response.StatusCode = 400;
            var allErrors = new Dictionary<string, List<string>>();
            (from pair in ModelState
             from error in pair.Value.Errors
             select (pair, error)).Each(x => {
                if (allErrors.TryGetValue(x.pair.Key, out var e))
                {
                    e.Add(x.error.ErrorMessage);
                }
                else
                {
                    allErrors.Add(string.IsNullOrEmpty(x.pair.Key) ? "Misc" : x.pair.Key,
                        new List<string> { x.error.ErrorMessage });
                }
            });
            result.Data = new {
                Errors = allErrors
            };
            return result;
        }

        #endregion
    }
}
