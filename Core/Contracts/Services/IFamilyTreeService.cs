using Core.DTOs;
using System.Threading.Tasks;

namespace Core.Contracts.Services
{
    public interface IFamilyTreeService
    {
        Task<Person> GetAsync(int personId);
    }
}
