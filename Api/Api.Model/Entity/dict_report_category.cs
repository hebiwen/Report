using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    public class dict_report_category
    {
        [Key]
        public Guid rec_id { get; set; }
        public string x_name { get; set; }
        public Guid x_parent { get; set; }
        public int x_no { get; set; }
    }
}
