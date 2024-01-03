using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using NHibernate;

namespace Contractors.Controllers
{
    [RequiresAdmin]
    public class CrewController : ControllerBaseWithValidation<Crew>
    {
        // GET: /Crew/
        [HttpGet]
        public ActionResult Index()
        {
            var user = _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser;
            NHibernateUtil.Initialize(user.Contractor);
            NHibernateUtil.Initialize(user.Contractor.Crews);
            ViewData["Contractor"] = user.Contractor;
            return View(user.Contractor.Crews);
        }

        // GET: /Crew/Show/5
        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        // GET: /Crew/New
        [HttpGet]
        public ActionResult New()
        {
            return ActionHelper.DoNew(_container.GetInstance<CreateCrew>());
        } 

        // POST: /Crew/Create
        [HttpPost]
        public ActionResult Create(CreateCrew model)
        {
            return ActionHelper.DoCreate(model);
        }

        // GET: /Crew/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditCrew>(id);
        }

        // POST: /Crew/Update/5
        [HttpPost]
        public ActionResult Update(EditCrew model)
        {
            return ActionHelper.DoUpdate(model);
        }

        public CrewController(ControllerBaseWithPersistenceArguments<IRepository<Crew>, Crew, ContractorUser> args) : base(args) {}
    }
}
