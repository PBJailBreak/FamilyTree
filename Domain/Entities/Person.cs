using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Entities
{
    public class Person : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public virtual ICollection<Relation> IncomingRelations { get; set; }
        public virtual ICollection<Relation> OutgoingRelations { get; set; }

        public Person AddParent(Person parent)
        {
            this.CheckIfIncomingRelationsAreLoaded();

            this.IncomingRelations.Add(
                    new Relation
                    {
                        PersonFrom = parent,
                        RelationTypeId = RelationTypeEnum.Child
                    }
                );

            return this;
        }

        public bool HasParent(Person parent)
        {
            this.CheckIfIncomingRelationsAreLoaded();

            return this.IncomingRelations.Any(r =>
                r.PersonFromId == parent.Id &&
                r.RelationTypeId == RelationTypeEnum.Child
            );
        }

        public void CheckIfIncomingRelationsAreLoaded()
        {
            if (this.IncomingRelations == null)
            {
                throw new Exception("Incoming relations are not loaded!");
            }
        }

        public void Update(DTOs.Person personDto)
        {
            this.Name = personDto.Name;
            this.Surname = personDto.Surname;
            this.Age = personDto.Age;
        }
    }
}
