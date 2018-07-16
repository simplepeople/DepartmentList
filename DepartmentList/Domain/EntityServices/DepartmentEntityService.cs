using System;
using System.Data.Entity.Hierarchy;
using System.Linq;
using DepartmentList.Domain.Contexts;
using DepartmentList.Domain.Entities;

namespace DepartmentList.Domain.EntityServices
{
    public class DepartmentEntityService: EntityServiceBase<DepartmentContext, DepartmentEntity>
    {
        private DepartmentEntity GetRoot()
        {
            return Entities.First(x => x.Node == HierarchyId.GetRoot());
        }

        public DepartmentEntity AddDepartmentToRoot(string name, DateTime creationDate)
        {
            var parent = GetRoot();
            return AddDepartment(parent, name, creationDate);
        }

        public DepartmentEntity AddDepartmentToParent(int parentId, string name, DateTime creationDate)
        {
            var parent = Entities.First(x => x.Id == parentId);
            return AddDepartment(parent, name, creationDate);
        }

        private DepartmentEntity AddDepartment(DepartmentEntity parent, string name, DateTime creationDate)
        {
            parent.HasChilds = true;
            var left = Entities.Where(x => x.ParentId == parent.Id).OrderByDescending(x => x.Id).FirstOrDefault()?.Node;
            var entity = new DepartmentEntity
            {
                CreationDate = creationDate.Date,
                ParentId = parent.Id,
                HasChilds = false,
                Name = name,
                Node = parent.Node.GetDescendant(left, null)
            };
            Entities.Add(entity);
            Save();
            return entity;
        }

        public void Remove(int id)
        {
            var department = Entities.First(x => x.Id == id);
            var parent = Entities.First(x => x.Id == department.ParentId);
            bool hasManyChilds = Entities.Count(x => x.Node.GetAncestor(1) == parent.Node) > 1;
            if (!hasManyChilds)
                parent.HasChilds = false;
            Entities.Remove(department);
            Save();
        }

        public IQueryable<DepartmentEntity> Get(int parentId, Func<DepartmentEntity, bool> filter)
        {
            var parent = Entities.First(x => x.Id == parentId);
            return Get(parent, filter);
        }

        public IQueryable<DepartmentEntity> Get(Func<DepartmentEntity, bool> filter)
        {
            var parent = GetRoot();
            return Get(parent, filter);
        }

        private IQueryable<DepartmentEntity> Get(DepartmentEntity parent, Func<DepartmentEntity, bool> filter)
        {
            return Entities.Where(x => x.Node.IsDescendantOf(parent.Node) && filter(x));
        }

        public IQueryable<DepartmentEntity> GetImmediateChilds(int parentId)
        {
            var parent = Entities.First(x => x.Id == parentId);
            return Entities.Where(x => x.Node.GetAncestor(1) == parent.Node);
        }

        public IQueryable<DepartmentEntity> GetImmediateRootChilds()
        {
            return Entities.Where(x => x.Node.GetAncestor(1) == HierarchyId.GetRoot());
        }
    }
}