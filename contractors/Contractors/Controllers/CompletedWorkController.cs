using System.Linq;
using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;

namespace Contractors.Controllers
{
    public class CompletedWorkController : ControllerBaseWithValidation<IMeterChangeOutRepository, MeterChangeOut>
    {
        public CompletedWorkController(ControllerBaseWithPersistenceArguments<IMeterChangeOutRepository, MeterChangeOut, ContractorUser> args) : base(args) { }

        [HttpGet]
        public ActionResult Search(SearchCompletedWork search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Index(SearchCompletedWork search)
        {
            search.EnablePaging = false;
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search));
                f.Excel(() => {
                    var results = Repository.Search(search)
                        .OrderBy(co => co.DateStatusChanged).ThenBy(co => co
                            .Documents.OrderBy(d => d.CreatedAt).LastOrDefault()
                            ?.CreatedAt);
                    return !results.Any()
                        ? RedirectToAction("Search")
                        : (ActionResult)new ExcelResult(
                                $"{results.First().CalledInByContractorMeterCrew} - {search.DateStatusChanged}")
                            .AddSheet(results.Select(x => new {
                                ServiceStreetAddress =
                                x.ServiceStreetAddressCombined,
                                x.ServiceCity,
                                x.DateStatusChanged,
                                FirstDocumentCreated =
                                x.Documents.OrderBy(d => d.CreatedAt).FirstOrDefault()
                                    ?.CreatedAt,
                                LastDocumentCreated =
                                x.Documents.OrderBy(d => d.CreatedAt).LastOrDefault()
                                    ?.CreatedAt
                            }));
                });
            });
        }
    }
}
