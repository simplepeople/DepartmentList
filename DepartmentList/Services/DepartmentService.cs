using System;
using System.Collections.Generic;
using AutoMapper;
using DepartmentList.Domain.Entities;
using DepartmentList.Domain.EntityServices;
using DepartmentList.Dto;
using DepartmentList.Infrastructure.Extensions;
using DepartmentList.Infrastructure.IoC;

namespace DepartmentList.Services
{
    public class DepartmentService : IDependency
    {
        protected DepartmentEntityService DepartmentEntityService { get; set; }

        private static Func<DepartmentEntity, bool> GetFilter(string name, DateTime? creationDate) => entity =>
            (name == null || entity.Name.Contains(name)) && (creationDate == null || entity.CreationDate == creationDate);

        public IEnumerable<DepartmentDto> Get(int id, string name, DateTime? creationDate)
        {
            return DepartmentEntityService.Get(id, GetFilter(name, creationDate)).Map();
        }

        public IEnumerable<DepartmentDto> Get(string name, DateTime? creationDate)
        {
            return DepartmentEntityService.Get(GetFilter(name, creationDate)).Map();
        }

        public IEnumerable<DepartmentDto> GetImmediateChilds(int id)
        {
            return DepartmentEntityService.GetImmediateChilds(id).Map();
        }

        public IEnumerable<DepartmentDto> GetImmediateRootChilds()
        {
            return DepartmentEntityService.GetImmediateRootChilds().Map();
        }

        public DepartmentDto AddDepartment(DepartmentDto dto)
        {
            var departmentEntity = !dto.ParentId.HasValue
                ? DepartmentEntityService.AddDepartmentToRoot(dto.Name, dto.CreationDate)
                : DepartmentEntityService.AddDepartmentToParent(dto.ParentId.Value, dto.Name, dto.CreationDate);
            return departmentEntity.Map();
        }

        public void Update(DepartmentDto dto)
        {
            DepartmentEntityService.Update(Mapper.Map<DepartmentEntity>(dto));
        }

        public void Remove(int id)
        {
            DepartmentEntityService.Remove(id);
        }
    }
}