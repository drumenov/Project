using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Constants;

namespace Project.Areas.Customer.Controllers.Base
{
    [Area(StringConstants.AreaNameCustomer)]
    [Authorize(Roles = StringConstants.UserRolesThatAreAuthorisedToUseTheCustomerArea)]
    public class BaseController : Controller
    {
    }
}
