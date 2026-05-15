using Microsoft.AspNetCore.Mvc;
using TMCWD.Application.Models;
using TMCWD.CustomerSupport;

namespace TMCWD.Application.ViewComponents
{
    public class CustomerSelectionViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string valueContainerElement, string textContainerElement, string parentContainerId = "", bool isRefreshCustomerName = false)
        {
            CustomerSelectionViewModel model = new()
            {
                SelectedValueContainerId = valueContainerElement,
                SelectedNameContainerId = textContainerElement,
                IsRefreshCustomerName = isRefreshCustomerName,
                ParentContainerId = parentContainerId
            };

            return View("Default", model);
        }
    }
}
