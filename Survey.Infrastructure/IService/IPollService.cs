using Survey.Domain.Entities;
using Survey.Infrastructure.DTOs.Poll.Requests;
using Survey.Infrastructure.DTOs.Poll.Responses;

namespace Survey.Infrastructure.IService;

public interface IPollService
{
    Task<IEnumerable<PollResponse?>> GetAllAsync(CancellationToken cancellationToken);
    Task<PollResponse?> GetByIdAsync(int id , CancellationToken cancellationToken);
    Task<int> AddAsync(PollRequest entity, CancellationToken cancellationToken);
    Task<PollResponse> UpdateAsync(int id,PollRequest entity, CancellationToken cancellationToken);
    Task<int> DeleteAsync(int id ,CancellationToken cancellationToken);
    Task<bool> ToggleIsPublished(int id, CancellationToken cancellationToken);
}
