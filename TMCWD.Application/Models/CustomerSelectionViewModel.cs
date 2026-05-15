namespace TMCWD.Application.Models
{
    public class CustomerSelectionViewModel
    {
        public string SelectedValueContainerId { get; set; } = String.Empty;

        public string SelectedNameContainerId { get; set; } = String.Empty;

        public string ParentContainerId { get; set; } = String.Empty;

        public bool IsRefreshCustomerName { get; set; }
    }
}
