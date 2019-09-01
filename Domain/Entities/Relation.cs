using System;

namespace Core.Entities
{
    public class Relation : BaseEntity
    {
        public int Id { get; set; }
        public RelationTypeEnum RelationTypeId { get; set; }
        public int PersonFromId { get; set; }
        public int PersonToId { get; set; }
        public DateTime? TerminationDate { get; set; }

        public virtual Person PersonFrom { get; set; }
        public virtual Person PersonTo { get; set; }
        public virtual RelationType RelationType { get; set; }
    }
}
