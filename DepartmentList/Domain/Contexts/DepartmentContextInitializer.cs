using System;
using System.Data.Entity;
using System.Data.Entity.Hierarchy;
using DepartmentList.Domain.Entities;

namespace DepartmentList.Domain.Contexts
{
    public class DepartmentContextInitializer : CreateDatabaseIfNotExists<DepartmentContext>
    {
        protected override void Seed(DepartmentContext context)
        {
            context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Departments] ADD [ParentDepartmentNode] AS ([Node].[GetAncestor]((1))) PERSISTED");
            context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Departments] ADD CONSTRAINT [UK_DepartmentNode] UNIQUE NONCLUSTERED (Node)");
            context.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[Departments] WITH CHECK ADD CONSTRAINT [DepartmentParentDepartmentNodeNodeFK] " +
                "FOREIGN KEY([ParentDepartmentNode]) REFERENCES [dbo].[Departments] ([Node])");
            context.Departments.Add(new DepartmentEntity { Name = "Root", Node = new HierarchyId("/"), HasChilds = true, CreationDate = DateTime.Now.Date});
            context.Departments.Add(new DepartmentEntity { Name = "ИТ", Node = new HierarchyId("/1/"), ParentId = 1, HasChilds = true, CreationDate = DateTime.Now.Date.AddDays(-5)});
            context.Departments.Add(new DepartmentEntity { Name = "Кадры", Node = new HierarchyId("/2/"), ParentId = 1, HasChilds = true, CreationDate = DateTime.Now.Date.AddDays(-10)});
            context.Departments.Add(new DepartmentEntity { Name = "Разработка", Node = new HierarchyId("/1/1/"), ParentId = 2, HasChilds = true, CreationDate = DateTime.Now.Date.AddDays(-4)});
            context.Departments.Add(new DepartmentEntity { Name = "Внутренняя разработка", Node = new HierarchyId("/1/1/1/"), ParentId = 4, HasChilds = false, CreationDate = DateTime.Now.Date.AddDays(-1)});
            context.Departments.Add(new DepartmentEntity { Name = "Поиск сотрудников", Node = new HierarchyId("/2/1/"), ParentId = 3, HasChilds = false, CreationDate = DateTime.Now.Date.AddDays(2)});
            context.Departments.Add(new DepartmentEntity { Name = "Поддержка", Node = new HierarchyId("/1/2/"), ParentId  = 2, HasChilds = false, CreationDate = DateTime.Now.Date.AddDays(3)});
        }
    }
}