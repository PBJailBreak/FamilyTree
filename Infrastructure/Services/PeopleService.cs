using System.Threading.Tasks;
using Core.Contracts.DataAcess;
using Core.Contracts.Services;
using Core.Entities;
using Core.Exceptions;
using Core.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IUnitOfWork uow;
        private readonly IAgeService ageService;

        public PeopleService(IUnitOfWork uow, IAgeService ageService)
        {
            this.uow = uow;
            this.ageService = ageService;
        }

        public async Task UpsertAsync(Core.DTOs.Person personDto)
        {
            var repository = this.uow.GetRepository<Person>();
            var person = await repository.GetAsync(x => x.Id == personDto.Id);

            if (person == null)
            {
                await repository.InsertAsync(personDto.ToEntity());
            }
            else
            {
                person.Update(personDto);
            }

            await this.uow.CommitAsync();
        }

        public async Task UpsertChildAsync(int parentId, Core.DTOs.Person childDto)
        {
            var repository = this.uow.GetRepository<Person>();
            var parent = await repository.GetAsync(x => x.Id == parentId, x => x.Include(p => p.OutgoingRelations))
                ?? throw new EntityNotFoundException($"Parent with id: {parentId} was not found.");

            if (childDto.Age == 0)
            {
                childDto.Age = await this.ageService.GetRandomAgeAsync();
            }

            // if new child
            if (childDto.Id == 0)
            {
                await repository.InsertAsync(childDto.ToEntity().AddParent(parent));
            }
            else
            {
                var child = await repository.GetAsync(x => x.Id == childDto.Id, x => x.Include(p => p.IncomingRelations))
                    ?? throw new EntityNotFoundException($"Child with id: {childDto.Id} was not found.");

                child.Update(childDto);

                if (!child.HasParent(parent))
                {
                    child.AddParent(parent);
                }
            }

            await this.uow.CommitAsync();
        }
    }
}
