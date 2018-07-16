using AutoMapper;
using DepartmentList.Domain.Entities;
using DepartmentList.Dto;

namespace DepartmentList.Infrastructure.Mapping.Auto
{
    public class DepartmentMap : Profile
    {
        public DepartmentMap()
        {
            CreateMap<DepartmentEntity, DepartmentDto>().ReverseMap();
        }
    }
}