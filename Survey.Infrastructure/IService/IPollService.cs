using Survey.Domain.Entities;
using Survey.Infrastructure.DTOs.Poll.Requests;
using Survey.Infrastructure.DTOs.Poll.Responses;

namespace Survey.Infrastructure.IService;

public interface IPollService
{
    Task<IEnumerable<PollResponse?>> GetAllAsync();
    Task<PollResponse?> GetByIdAsync(int id);
    Task<int> AddAsync(PollRequest entity);
    Task<PollResponse> UpdateAsync(int id,PollRequest entity);
    Task<int> DeleteAsync(int id);
}
