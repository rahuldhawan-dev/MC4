using System.Web.Mvc;
using MMSINC.Authentication;
using MMSINC.Metadata;

namespace MMSINC.Testing
{
    public class FakeCrudController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index() => null;

        [HttpGet]
        public virtual ActionResult Show() => null;

        [HttpGet]
        public virtual ActionResult New() => null;

        [HttpPost]
        public virtual ActionResult Create() => null;

        [HttpGet]
        public virtual ActionResult Edit() => null;

        [HttpPost]
        public virtual ActionResult Update() => null;

        [HttpPost]
        public virtual ActionResult Delete() => null;
    }

    public class FakeCrudWithSearchController : FakeCrudController
    {
        [HttpGet]
        public virtual ActionResult Search() => null;
    }

    public class FakeAuthenticationController : Controller
    {
        [AllowAnonymous]
        public virtual ActionResult Anonymous() => null;

        // No attribute needed
        public virtual ActionResult LoggedIn() => null;

        [RequiresAdmin]
        public virtual ActionResult AdminOnly() => null;
    }

    [AllowAnonymous]
    public class FakeAnonymousController : Controller
    {
        public virtual ActionResult Action() => null;
    }
}
