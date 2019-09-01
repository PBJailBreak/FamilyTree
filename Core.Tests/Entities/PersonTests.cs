using Core.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Tests.Entities
{
    public class PersonTests
    {
        private Person person;
        private Person parent;

        [SetUp]
        public void Setup()
        {
            this.person = new Person
            {
                Name = "Jonas",
                Surname = "Jonaitis",
                Age = 32
            };

            this.parent = new Person
            {
                Name = "Petras",
                Surname = "Petraitis",
                Age = 57
            };
        }

        [Test]
        public void AddParent_WhenPersonHasNullValueIncomingRelations_ShouldNotThrowAnyErrors()
        {
            Assert.DoesNotThrow(() => this.person.AddParent(this.parent));
        }

        [Test]
        public void AddParent_WhenParentIsNull_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => this.person.AddParent(null));
        }

        [Test]
        public void AddParent_ShouldAddTheSpecifiedParent()
        {
            this.person.AddParent(this.parent);

            Assert.IsTrue(this.person.IncomingRelations.ToList().ElementAt(0).PersonFrom == this.parent);
        }

        [Test]
        public void AddParent_ShouldAddIncomingRelationWithChildRelationType()
        {
            this.person.AddParent(this.parent);

            Assert.IsTrue(this.person.IncomingRelations.ToList().ElementAt(0).RelationTypeId == RelationTypeEnum.Child);
        }

        [Test]
        public void AddParent_ShouldReturnThePerson()
        {
            var returnedPerson = this.person.AddParent(this.parent);

            Assert.AreEqual(this.person, returnedPerson);
        }

        [Test]
        public void HasParent_ShouldThrowException_WhenIncomingRelationsAreNotLoaded()
        {
            Assert.Throws<Exception>(() => this.person.HasParent(null));
        }

        [Test]
        public void HasParent_ShouldReturnFalse_WhenParentIdDoesNotMatch()
        {
            this.person.IncomingRelations = new List<Relation>
            {
                new Relation
                {
                    PersonFromId = 42,
                    RelationTypeId = RelationTypeEnum.Child
                }
            };

            Assert.IsFalse(person.HasParent(parent));
        }

        [Test]
        public void HasParent_ShouldReturnFalse_WhenRelationTypeIsNotChild()
        {
            this.person.IncomingRelations = new List<Relation>
            {
                new Relation
                {
                    PersonFromId = this.parent.Id,
                    RelationTypeId = RelationTypeEnum.Sibling
                }
            };

            Assert.IsFalse(person.HasParent(parent));
        }

        [Test]
        public void HasParent_ShouldReturnTrue_WhenParentIdMatchesAndRelationTypeIsChild()
        {
            this.person.IncomingRelations = new List<Relation>
            {
                new Relation
                {
                    PersonFromId = this.parent.Id,
                    RelationTypeId = RelationTypeEnum.Child
                }
            };

            Assert.IsTrue(person.HasParent(parent));
        }

        [Test]
        public void Update_ShouldUpdateAllPropertiesToMatch()
        {
            var newName = "John";
            var newSurname = "Johnaitis";
            var newAge = 33;

            var personDto = new Core.DTOs.Person
            {
                Name = newName,
                Surname = newSurname,
                Age = newAge
            };

            this.person.Update(personDto);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(person.Name, newName);
                Assert.AreEqual(person.Surname, newSurname);
                Assert.AreEqual(person.Age, newAge);
            });
        }
    }
}
