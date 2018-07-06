using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;

namespace DepartmentList.Controllers
{
    public class Department
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("hasChildren")]
        public bool HasChildDepartments { get; set; }
        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        public Department(int id, int parentId, bool hasChildDepartments = false)
        {
            Id = id;
            ParentId = parentId;
            Name = id.ToString();
            CreationDate = DateTime.Now.AddDays(-id);
            HasChildDepartments = hasChildDepartments;
        }
    }

    public class DepartmentController : ApiController
    {
        [HttpGet]
        public IEnumerable<Department> Get([FromUri] string name = null, [FromUri] DateTime? startDate = null,
            [FromUri] DateTime? endDate = null)
        {
            return Get(0, name, startDate, endDate);
        }

        [HttpGet]
        public IEnumerable<Department> Get(int id, [FromUri] string name = null, [FromUri] DateTime? startDate = null,
            [FromUri]DateTime? endDate = null)
        {
            IEnumerable<Department> result;
            switch (id)
            {
                case 0:
                    result = Zero;
                    break;
                case 1:
                    result = First;
                    break;
                case 2:
                    result = Second;
                    break;
                default:
                    result = new List<Department>();
                    break;
            }

            return result.Where(x =>
                (String.IsNullOrWhiteSpace(name) || x.Name.Contains(name)) &&
                (!startDate.HasValue || startDate <= x.CreationDate) &&
                (!endDate.HasValue || x.CreationDate >= endDate));
        }

        private static IEnumerable<Department> Full => Zero.Union(First.Union(Second));

        private static IEnumerable<Department> Zero => new[]
            {new Department(1, 0, true), new Department(2, 0, true), new Department(3, 0)};

        private static IEnumerable<Department> First => new[]
        {
            new Department(4, 1), new Department(5, 1),
        };

        private static IEnumerable<Department> Second => new[]
            {new Department(6, 2)};


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
        }
    }
}