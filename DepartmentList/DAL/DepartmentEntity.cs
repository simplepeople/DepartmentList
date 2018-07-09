using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Hierarchy;

namespace DepartmentList.DAL
{
    [Table("Departments")]
    public class DepartmentEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public HierarchyId Node { get; set; }
        public int ParentId { get; set; }
        public bool HasChilds { get; set; }
    }
}