using DepartmentList.Domain.Contexts;

namespace DepartmentList.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DepartmentContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DepartmentContext context)
        {
            
        }
    }
}
