using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Saas.Api.Controllers
{
    
    public class BaseController : ControllerBase
    {
        protected Guid GetCurrentUserId()
    {
        return new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
    }
    }
}