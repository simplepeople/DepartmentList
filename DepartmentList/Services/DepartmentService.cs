using System;
using System.Collections.Generic;
using AutoMapper;
using DepartmentList.Domain.Entities;
using DepartmentList.Domain.EntityServices;
using DepartmentList.Dto;

namespace DepartmentList.Services
{
    public class DepartmentService
    {
        protected DepartmentEntityService DepartmentEntityService { get; set; }

        private static Func<DepartmentEntity, bool> GetFilter(string name, DateTime? creationDate) => entity =>
            (name == null || entity.Name.Contains(name)) && (creationDate == null || entity.CreationDate == creationDate);

        public IEnumerable<DepartmentDto> Get(int id, string name, DateTime? creationDate)
        {
            return Mapper.Map<IEnumerable<DepartmentDto>>(
                DepartmentEntityService.Get(id, GetFilter(name, creationDate)));
        }

        public IEnumerable<DepartmentDto> Get(string name, DateTime? creationDate)
        {
            return Mapper.Map<IEnumerable<DepartmentDto>>(DepartmentEntityService.Get(GetFilter(name, creationDate)));
        }

        public IEnumerable<DepartmentDto> GetImmediateChilds(int id)
        {
            return Mapper.Map<IEnumerable<DepartmentDto>>(DepartmentEntityService.GetImmediateChilds(id));
        }

        public IEnumerable<DepartmentDto> GetImmediateRootChilds()
        {
            return Mapper.Map<IEnumerable<DepartmentDto>>(DepartmentEntityService.GetImmediateRootChilds());
        }

        public DepartmentDto AddDepartment(DepartmentDto dto)
        {
            var departmentEntity = !dto.ParentId.HasValue
                ? DepartmentEntityService.AddDepartmentToRoot(dto.Name, dto.CreationDate)
                : DepartmentEntityService.AddDepartmentToParent(dto.ParentId.Value, dto.Name, dto.CreationDate);
            return Mapper.Map<DepartmentDto>(departmentEntity);
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