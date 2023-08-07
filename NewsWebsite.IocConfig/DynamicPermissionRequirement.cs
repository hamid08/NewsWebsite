using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using NewsWebsite.Services.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebsite.IocConfig
{
    public class DynamicPermissionRequirement : IAuthorizationRequirement
    {
    }

    public class DynamicPermissionsAuthorizationHandler : AuthorizationHandler<DynamicPermissionRequirement>
    {
        private readonly ISecurityTrimmingService _securityTrimmingService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DynamicPermissionsAuthorizationHandler(IHttpContextAccessor httpContextAccessor,ISecurityTrimmingService securityTrimmingService)
        {
            _securityTrimmingService = securityTrimmingService;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(
             AuthorizationHandlerContext context,
             DynamicPermissionRequirement requirement)
        {

            var routeData = _httpContextAccessor.HttpContext.GetRouteData();

            var areaName = routeData?.Values["area"]?.ToString();
            var area = string.IsNullOrWhiteSpace(areaName) ? string.Empty : areaName;

            var controllerName = routeData?.Values["controller"]?.ToString();
            var controller = string.IsNullOrWhiteSpace(controllerName) ? string.Empty : controllerName;

            var actionName = routeData?.Values["action"]?.ToString();
            var action = string.IsNullOrWhiteSpace(actionName) ? string.Empty : actionName;

            //var mvcContext = context.Resource as Endpoint;
            //if (mvcContext == null)
            //{
            //    return Task.CompletedTask;
            //}

            //var actionDescriptor = mvcContext.Metadata.OfType<ControllerActionDescriptor>().SingleOrDefault();

            //actionDescriptor.RouteValues.TryGetValue("area", out var areaName);
            //var area = string.IsNullOrWhiteSpace(areaName) ? string.Empty : areaName;

            //actionDescriptor.RouteValues.TryGetValue("controller", out var controllerName);
            //var controller = string.IsNullOrWhiteSpace(controllerName) ? string.Empty : controllerName;

            //actionDescriptor.RouteValues.TryGetValue("action", out var actionName);
            //var action = string.IsNullOrWhiteSpace(actionName) ? string.Empty : actionName;

            if (_securityTrimmingService.CanCurrentUserAccess(area, controller, action))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
