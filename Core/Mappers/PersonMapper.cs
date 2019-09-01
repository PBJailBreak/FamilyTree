using System.Collections.Generic;

namespace Core.Mappers
{
    public static class PersonMapper
    {
        public static Core.Entities.Person ToEntity(this Core.DTOs.Person person)
        {
            return new Core.Entities.Person
            {
                Name = person.Name,
                Surname = person.Surname,
                Age = person.Age
            };
        }

        public static Core.DTOs.Person ToDto(this Core.Entities.Person person, IEnumerable<Core.DTOs.Person> children = null)
        {
            return new Core.DTOs.Person
            {
                Id = person.Id,
                Name = person.Name,
                Surname = person.Surname,
                Age = person.Age,

                Children = children
            };
        }
    }
}
