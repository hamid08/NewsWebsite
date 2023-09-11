using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Data.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebsite.ViewComponents
{
    public class SiteInfoComponent: ViewComponent
    {
        private readonly IUnitOfWork _uw;
        public SiteInfoComponent(IUnitOfWork uw)
        {
            _uw = uw;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var visitCount = await _uw._Context.SiteVisits.CountAsync();

            return View(visitCount);
        }
    }

}
