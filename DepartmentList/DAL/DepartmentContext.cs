using System.Data.Entity;

namespace DepartmentList.DAL
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