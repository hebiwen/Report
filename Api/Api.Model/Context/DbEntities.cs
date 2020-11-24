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
        #endregion

    }
}
