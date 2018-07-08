using System.Data.Entity.Hierarchy;
using DepartmentList.Models;

namespace DepartmentList.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DepartmentList.Models.DepartmentContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DepartmentList.Models.DepartmentContext context)
        {
            
        }
    }
}
