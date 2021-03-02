using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    public class DbEntities : DbContext
    {
        public DbEntities() : base("DefaultConnection") { }

        #region Entities
        public DbSet<resource_info> resource_info { get; set; }

        public DbSet<dict_report_category> dict_report_category { get; set; }

        public DbSet<dict_zhongtu_category> dict_zhongtu_category { get; set; }

        public DbSet<dict_industry_category> dict_industry_category { get; set; }

        public DbSet<rel_resource_industry> rel_resource_industry { get; set; }

        public DbSet<sys_user> sys_user { get; set; }
        #endregion

        #region NewEntites
        public DbSet<RP_Report> Report { get; set; }

        public DbSet<RP_Directory> Directory { get; set; }

        public DbSet<RP_Category> Category { get; set; }
        #endregion

        #region VEntities
        public DbSet<V_Report> V_Report { get; set; }
        #endregion

    }
}
