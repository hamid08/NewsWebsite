using NewsWebsite.Entities.identity;
using System.Collections.Generic;

namespace NewsWebsite.ViewModels.DynamicAccess
{
    public class DynamicAccessIndexViewModel
    {
        public string ActionIds { set; get; }
        public int UserId { set; get; }
        public User UserIncludeUserClaims { set; get; }
        public ICollection<ControllerViewModel> SecuredControllerActions { set; get; }
    }
}
