using Core.Contracts.Services;
using Core.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Core.Mappers;
using Core.Contracts.DataAcess;
using Core.Exceptions;

namespace Infrastructure.Services
{
    public class FamilyTreeService : IFamilyTreeService
    {
        private readonly IUnitOfWork uow;

        public FamilyTreeService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<Core.DTOs.Person> GetAsync(int personId)
        {
            var personRepository = this.uow.GetRepository<Person>();
            var person = await personRepository.GetAsync
            (
                x => x.Id == personId,
                x => x.Include(p => p.OutgoingRelations)
            ) ?? throw new EntityNotFoundException($"A person with id: {personId} was not found.");

            if (person.OutgoingRelations == null) return person.ToDto();

            var childrenIds = person.OutgoingRelations.ToList().Where(r => r.RelationTypeId == RelationTypeEnum.Child).Select(r => r.PersonToId);
            var children = await Task.WhenAll(childrenIds.Select(async id => await this.GetAsync(id)));

            return person.ToDto(children);
        }
    }
}
