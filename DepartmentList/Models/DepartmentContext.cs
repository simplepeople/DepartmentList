using System.Data.Entity;
using System.Data.Entity.Hierarchy;

namespace DepartmentList.Models
{
    public class DepartmentContext : DbContext
    {
        public DbSet<DepartmentEntity> Departments { get; set; }

        static DepartmentContext()
        {
            Database.SetInitializer(new DepartmentContextInitializer());
        }

        public DepartmentContext() : base("DepartmentContext")
        {
            
        }
    }
}