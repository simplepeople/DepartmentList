using System.Data.Entity;
using System.Data.Entity.Hierarchy;

namespace DepartmentList.Models
{
    public class DepartmentContextInitializer : DropCreateDatabaseAlways<DepartmentContext>
    {
        protected override void Seed(DepartmentContext context)
        {
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Departments] ADD [ParentDepartmentNode] AS ([Node].[GetAncestor]((1))) PERSISTED");
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Departments] ADD CONSTRAINT [UK_DepartmentNode] UNIQUE NONCLUSTERED (Node)");
            context.Database.ExecuteSqlCommand(
                "ALTER TABLE [dbo].[Departments] WITH CHECK ADD CONSTRAINT [DepartmentParentDepartmentNodeNodeFK] " +
                "FOREIGN KEY([ParentDepartmentNode]) REFERENCES [dbo].[Departments] ([Node])");
            context.Departments.Add(new DepartmentEntity { Name = "Root", Node = new HierarchyId("/") });
            context.Departments.Add(new DepartmentEntity { Name = "Dep1", Node = new HierarchyId("/1/") });
            context.Departments.Add(new DepartmentEntity { Name = "Dep2", Node = new HierarchyId("/2/") });
            context.Departments.Add(new DepartmentEntity { Name = "Dep3", Node = new HierarchyId("/1/1/") });
            context.Departments.Add(new DepartmentEntity { Name = "Dep4", Node = new HierarchyId("/1/1/1/") });
            context.Departments.Add(new DepartmentEntity { Name = "Dep5", Node = new HierarchyId("/2/1/") });
            context.Departments.Add(new DepartmentEntity { Name = "Dep6", Node = new HierarchyId("/1/2/") });
            context.SaveChanges();
        }
    }
}