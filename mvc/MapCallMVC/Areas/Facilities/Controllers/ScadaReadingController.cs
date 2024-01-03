using System;
using System.Linq;
using System.Web.Mvc;
using Historian.Data.Client.Repositories;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class ScadaReadingController : ControllerBaseWithPersistence<ScadaTagName, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionEquipment;

        #endregion

        #region Exposed Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchScadaReading search)
        {
            if (!search.TagName.HasValue)
            {
                return RedirectToAction("Search", search);
            }

            if ((search.TagNameObj = Repository.Find(search.TagName.Value)) != null &&
                (search.Results = _container.GetInstance<IRawDataRepository>().FindByTagName(search.TagNameObj.TagName, search.UseRaw, search.StartDate, search.EndDate)).Any())
            {
                return this.RespondTo(f => {
                    f.View(() => View(search));
                    f.Excel(() => this.Excel(search.Results));
                });
            }

            DisplayErrorMessage($"No results found for tag name with id {search.TagName}.");
            return RedirectToAction("Search", search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchScadaReading search)
        {
            this.AddDynamicDropDownData<ScadaTagName, ScadaTagNameDisplayItem>(t => t.Id, t => t.Display, "TagName");

            return View("Search");
        }

        #endregion

        //public ScadaReadingController(IRawDataRepository repository, IRepository<ScadaTagName> tagNameRepository) : base()
        //{
        //    _repository = repository;
        //    _tagNameRepository = tagNameRepository;
        //}
        public ScadaReadingController(ControllerBaseWithPersistenceArguments<IRepository<ScadaTagName>, ScadaTagName, User> args) : base(args) {}
    }
}