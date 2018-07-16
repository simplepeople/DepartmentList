using AutoMapper;
using DepartmentList.Infrastructure.Extensions;

namespace DepartmentList
{
    public class AutomapperConfig
    {
        public static void InitMapping()
        {
            Mapper.Initialize(x =>
            {
                foreach (var profile in ReflectiveEnumerator.GetInstanceOfSubclasses<Profile>(true))
                    x.AddProfile(profile);
            });
        }
    }
}