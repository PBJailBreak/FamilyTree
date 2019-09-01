using Core.DTOs;
using System.Threading.Tasks;

namespace Core.Contracts.Services
{
    public interface IPeopleService
    {
        Task UpsertAsync(Person person);
        Task UpsertChildAsync(int parentId, Person child);
    }
}
