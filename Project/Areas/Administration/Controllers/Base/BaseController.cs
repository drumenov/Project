using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common;

namespace Project.Areas.Administration.Controllers.Base
{
    [Area(StringConstants.AreaNameAdministration)]
    [Authorize(Roles = StringConstants.AdminUserRole)]
    public class BaseController : Controller
    {
    }
}
