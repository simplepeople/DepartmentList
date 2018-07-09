using System;
using System.Data.Entity;
using System.Data.Entity.Hierarchy;
using System.Linq;

namespace DepartmentList.DAL
{
    public class DepartmentEntityService
    {
        //todo переделать
        private static readonly DepartmentContext Context = new DepartmentContext();
        private static readonly DbSet<DepartmentEntity> Departments = Context.Departments;

        private DepartmentEntity GetRoot()
        {
            return Departments.First(x => x.Node == HierarchyId.GetRoot());
        }

        public DepartmentEntity Add(string name, DateTime creationDate)
        {
            var parent = GetRoot();
            return Add(parent, name, creationDate);
        }

        public DepartmentEntity Add(int parentId, string name, DateTime creationDate)
        {
            var parent = Departments.First(x => x.Id == parentId);
            return Add(parent, name, creationDate);
        }

        private static DepartmentEntity Add(DepartmentEntity parent, string name, DateTime creationDate)
        {
            parent.HasChilds = true;
            var left = Departments.Where(x => x.ParentId == parent.Id).OrderByDescending(x => x.Id).FirstOrDefault()?.Node;
            var entity = new DepartmentEntity
            {
                CreationDate = creationDate.Date,
                ParentId = parent.Id,
                HasChilds = false,
                Name = name,
                Node = parent.Node.GetDescendant(left, null)
            };
            Departments.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public void Update(int id, string name, DateTime creationDate)
        {
            var department = Departments.First(x => x.Id == id);
            department.Name = name;
            Context.SaveChanges();
        }

        public void Remove(int id)
        {
            var department = Departments.First(x => x.Id == id);
            var parent = Departments.First(x => x.Id == department.ParentId);
            bool hasManyChilds = Departments.Count(x => x.Node.GetAncestor(1) == parent.Node) > 1;
            if (!hasManyChilds)
                parent.HasChilds = false;
            Departments.Remove(department);
            Context.SaveChanges();
        }

        public IQueryable<DepartmentEntity> Get(int parentId, string name, DateTime? creationDate)
        {
            var parent = Departments.First(x => x.Id == parentId);
            return Get(parent, name, creationDate);
        }

        public IQueryable<DepartmentEntity> Get(string name, DateTime? creationDate)
        {
            var parent = GetRoot();
            return Get(parent, name, creationDate);
        }

        private IQueryable<DepartmentEntity> Get(DepartmentEntity parent, string name, DateTime? creationDate)
        {
            return Departments.Where(o => o.Node.IsDescendantOf(parent.Node) && (name == null || o.Name.Contains(name)) && (!creationDate.HasValue || o.CreationDate == creationDate.Value));
        }

        public IQueryable<DepartmentEntity> Get(int parentId)
        {
            var parent = Departments.First(x => x.Id == parentId);
            return Departments.Where(x => x.Node.GetAncestor(1) == parent.Node);
        }

        public IQueryable<DepartmentEntity> Get()
        {
            return Departments.Where(x=>x.Node.GetAncestor(1) == HierarchyId.GetRoot());
        }
    }
}