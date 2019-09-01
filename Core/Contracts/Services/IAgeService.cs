using System.Threading.Tasks;

namespace Core.Contracts.Services
{
    public interface IAgeService
    {
        Task<int> GetRandomAgeAsync();
    }
}
