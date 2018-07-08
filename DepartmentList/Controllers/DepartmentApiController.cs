using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using DepartmentList.Models;

namespace DepartmentList.Controllers
{
    public class DepartmentController : ApiController
    {
        [HttpGet]
        public DepartmentDto Get(int id)
        {
            return null;
        }

        [HttpGet]
        public IEnumerable<DepartmentDto> Get(int parentId, string name, DateTime creationDate)
        {
            return null;
        }

        public IEnumerable<DepartmentDto> Get(string name, DateTime creationDate)
        {
            return Get(0, name, creationDate);
        }

        //[HttpGet]
        //public IEnumerable<Department> Get([FromUri] string name = null, [FromUri] DateTime? startDate = null,
        //    [FromUri] DateTime? endDate = null)
        //{
        //    return Get(0, name, startDate, endDate);
        //}

        //[HttpGet]
        //public IEnumerable<Department> Get(int id, [FromUri] string name = null, [FromUri] DateTime? startDate = null,
        //    [FromUri]DateTime? endDate = null)
        //{
        //    IEnumerable<Department> result = new List<Department>();
        //    //todo
        //    return result.Where(x =>
        //        (String.IsNullOrWhiteSpace(name) || x.Name.Contains(name)) &&
        //        (!startDate.HasValue || startDate <= x.CreationDate) &&
        //        (!endDate.HasValue || x.CreationDate >= endDate));
        //}


        // POST api/<controller>
        [HttpPost]
        public void Post(string name, DateTime creationDate, int parentId)
        {
        }

        // PUT api/<controller>/5
        [HttpPut]
        public void Put(int id, [FromUri]string name, [FromUri]DateTime creationDate)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public void Delete(int id)
        {
            using (var context = new DepartmentContext())
            {
                Database.SetInitializer(new DepartmentContextInitializer());
                context.Database.Initialize(true);
            }
        }
    }
}