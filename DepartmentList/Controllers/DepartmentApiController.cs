using System;
using System.Collections.Generic;
using System.Web.Http;
using DepartmentList.Domain;
using DepartmentList.Dto;
using DepartmentList.Services;

namespace DepartmentList.Controllers
{
    public class DepartmentController : ApiController
    {
        public DepartmentService DepartmentService { get; set; }
        
        [HttpGet]
        public IEnumerable<DepartmentDto> Get(int id, [FromUri]string name, [FromUri] DateTime? creationDate)
        {
            return DepartmentService.Get(id, name, creationDate);
        }

        [HttpGet]
        public IEnumerable<DepartmentDto> Get([FromUri]string name, [FromUri] DateTime? creationDate = null)
        {
            return DepartmentService.Get(name, creationDate);
        }

        [HttpGet]
        public IEnumerable<DepartmentDto> Get(int id)
        {
            return DepartmentService.GetImmediateChilds(id);
        }

        [HttpGet]
        public IEnumerable<DepartmentDto> Get()
        {
            return DepartmentService.GetImmediateRootChilds();
        }

        [HttpPost]
        public DepartmentDto Post(DepartmentDto dto)
        {
            return DepartmentService.AddDepartment(dto);
        }

        [HttpPut]
        public void Put(DepartmentDto dto)
        {
            DepartmentService.Update(dto);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            DepartmentService.Remove(id);
        }
    }
}