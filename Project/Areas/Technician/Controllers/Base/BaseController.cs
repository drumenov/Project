using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Constants;

namespace Project.Areas.Technician.Controllers.Base
{
    [Area(StringConstants.AreaNameTechnician)]
    [Authorize(Roles = StringConstants.UserRolesThatAreAuthorisedToUseTheTechnicianArea)]
    public class BaseController : Controller
    {
    }
}
