using NewsWebsite.ViewModels.DynamicAccess;
using System.Collections.Generic;

namespace NewsWebsite.Services.Contracts
{
    public interface IMvcActionsDiscoveryService
    {
        ICollection<ControllerViewModel> GetAllSecuredControllerActionsWithPolicy(string policyName);
    }
}
