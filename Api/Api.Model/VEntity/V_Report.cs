using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    public class V_Report
    {
        public Guid id { get; set; }
        public string Title { get; set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid Status { get; set; }
        public string StatusStr { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime PublishDate { get; set; } 
    }
}
