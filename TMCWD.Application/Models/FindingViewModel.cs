namespace TMCWD.Application.Models
{
    public class FindingViewModel
    {

        public FindingViewModel() { }

        public int JobOrderId { get; set; }

        public int RequestId { get; set; }

        public string Details { get; set; } = string.Empty;

        public List<IFormFile> FindingFiles { get; set; } = new();

    }
}
