using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.ViewModels.DynamicAccess;

namespace NewsWebsite.Areas.Admin.Controllers
{
    [DisplayName("داشبورد")]

    public class DashboardController : BaseController
    {
        [HttpGet, DisplayName("مشاهده")]
        [Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult Index()
        {
            return View();
        }
    }
}