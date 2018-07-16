using System.Linq;
using DepartmentList.Infrastructure.Extentions;
using FluentJsonNet;
using Newtonsoft.Json;

namespace DepartmentList
{
    public class JsonMapperConfig
    {
        public static void InitMapping()
        {
            var mappings = ReflectiveEnumerator.GetSubclasses<JsonMap>(true);
            JsonConvert.DefaultSettings = JsonMaps.GetDefaultSettings(mappings.ToArray());
        }
    }
}