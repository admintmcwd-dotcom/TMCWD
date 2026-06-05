using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public interface IApprovalHIstoryService
    {
        
        Task<ApprovalHistory> Save(int userId, int jobOrderId, ApprovalHistory history);
        Task<List<ApprovalHistory>> GetAll(int jobOrderId);

    }
}
