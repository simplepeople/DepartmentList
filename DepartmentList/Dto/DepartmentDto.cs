using System;
using Newtonsoft.Json;

namespace DepartmentList.Dto
{
    public class DepartmentDto
    {
        //todo перенести в маппинг
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("hasChildren")]
        public bool HasChilds { get; set; }
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
    }
}