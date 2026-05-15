using Microsoft.AspNetCore.Mvc;
using TMCWD.CustomerSupport;

namespace TMCWD.Application.ViewComponents
{
    public class CustomerSelectionViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string selectedValueContainerElement)
        {
            return View("Default", selectedValueContainerElement);
        }
    }
}
