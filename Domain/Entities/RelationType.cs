using System.Collections.Generic;

namespace Core.Entities
{
    public class RelationType
    {
        public RelationTypeEnum Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Relation> Relations { get; set; }
    }
}
