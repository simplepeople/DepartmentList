using System.Data.Entity;
using DepartmentList.Domain.Entities;

namespace DepartmentList.Domain.Contexts
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

        public static void InitDb()
        {
            using (var context = new DepartmentContext())
            {
                context.Database.Initialize(true);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentEntity>().HasKey(x => x.Id).ToTable("Departments");
            base.OnModelCreating(modelBuilder);
        }
    }
}