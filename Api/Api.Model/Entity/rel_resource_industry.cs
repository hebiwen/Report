using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    [Table("rel_resource_industry")]
    public class rel_resource_industry
    {
        [Key]
        public Guid rec_id { get; set; }
        public Guid resource_id { get; set; }
        public Guid industry_id { get; set; }
        
    }
}
