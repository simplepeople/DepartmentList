using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DepartmentList.DAL;
using DepartmentList.Models;

namespace DepartmentList.Controllers
{
    public class DepartmentController : ApiController
    {
        //todo переписать на IoC
        private IEnumerable<DepartmentDto> G(Func<DepartmentEntityService, IQueryable<DepartmentEntity>> f)
        {
            return f(new DepartmentEntityService()).AsEnumerable().Select(Convert);
        }
        //todo mapper
        private static DepartmentDto Convert(DepartmentEntity entity)
        {
            return new DepartmentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreationDate = entity.CreationDate,
                ParentId = entity.ParentId,
                HasChildDepartments = entity.HasChilds
            };
        }
        
        /// <summary>
        /// Дай мне всех в поиске у конкретного родителя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="creationDate"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<DepartmentDto> Get(int id, [FromUri]string name, [FromUri]DateTime? creationDate)
        {
            return G(x => x.Get(id, name, creationDate));
        }

        /// <summary>
        /// Дай мне всех в поиске
        /// </summary>
        /// <param name="name"></param>
        /// <param name="creationDate"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<DepartmentDto> Get([FromUri]string name, [FromUri] DateTime? creationDate = null)
        {
            return G(x=>x.Get(name, creationDate));
        }

        /// <summary>
        /// дай мне всех прямых потомков указанного уровня
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<DepartmentDto> Get(int id)
        {
            return G(x=>x.Get(id));
        }

        /// <summary>
        /// дай мне всех прямых потомков корня
        /// </summary>
        [HttpGet]
        public IEnumerable<DepartmentDto> Get()
        {
            return G(x => x.Get());
        }

        //todo убрать копипасту
        [HttpPost]
        public DepartmentDto Post(DepartmentDto dto)
        {
            return Convert(!dto.ParentId.HasValue
                ? new DepartmentEntityService().Add(dto.Name, dto.CreationDate)
                : new DepartmentEntityService().Add(dto.ParentId.Value, dto.Name, dto.CreationDate));
        }

        [HttpPut]
        public void Put(DepartmentDto dto)
        {
            new DepartmentEntityService().Update(dto.Id, dto.Name, dto.CreationDate);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            new DepartmentEntityService().Remove(id);
        }
    }
}