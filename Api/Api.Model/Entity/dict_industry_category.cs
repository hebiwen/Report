using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    [Table("dict_industry_category")]
    public class dict_industry_category
    {
        [Key]
        public Guid rec_id { get; set; }
        public string x_name { get; set; }
        public int x_no { get; set; }
    }
}
