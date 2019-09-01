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
            if (this.IncomingRelations == null)
            {
                this.IncomingRelations = new List<Relation>();
            }

            if (parent == null)
            {
                throw new Exception("Parent cannot be null.");
            }

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
            if (this.IncomingRelations == null)
            {
                throw new Exception("Incoming relations are not loaded!");
            }

            return this.IncomingRelations.Any(r =>
                r.PersonFromId == parent.Id &&
                r.RelationTypeId == RelationTypeEnum.Child
            );
        }

        public void Update(DTOs.Person personDto)
        {
            this.Name = personDto.Name;
            this.Surname = personDto.Surname;
            this.Age = personDto.Age;
        }
    }
}
