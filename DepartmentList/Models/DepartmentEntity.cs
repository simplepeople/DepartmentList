using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Hierarchy;
using System.Linq;

namespace DepartmentList.Models
{
    [Table("Departments")]
    public class DepartmentEntity
    {
        [Key]
        public int Id { get; set; }
        //public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        //public bool HasChildDepartments { get; set; }
        //public int ParentId { get; set; }
        public HierarchyId Node { get; set; }

        public IQueryable<DepartmentEntity> GetSubordinates(DepartmentContext context)
        {
            return context.Departments.Where(o => Node == o.Node.GetAncestor(1));
        }
    }
}