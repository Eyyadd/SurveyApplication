using Survey.Domain.Entities;
using Survey.Infrastructure.Abstractions;
using Survey.Infrastructure.DTOs.Poll.Requests;
using Survey.Infrastructure.DTOs.Poll.Responses;

namespace Survey.Infrastructure.IService;

public interface IPollService
{
    Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<PollResponse>> GetByIdAsync(int id , CancellationToken cancellationToken);
    Task<Result> AddAsync(PollRequest entity, CancellationToken cancellationToken);
    Task<Result<PollResponse>> UpdateAsync(int id,PollRequest entity, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(int id ,CancellationToken cancellationToken);
    Task<Result> ToggleIsPublished(int id, CancellationToken cancellationToken);
}
