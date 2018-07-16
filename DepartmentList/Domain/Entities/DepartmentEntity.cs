using System;
using System.Data.Entity.Hierarchy;

namespace DepartmentList.Domain.Entities
{
    public class DepartmentEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public HierarchyId Node { get; set; }
        public int ParentId { get; set; }
        public bool HasChilds { get; set; }
    }
}