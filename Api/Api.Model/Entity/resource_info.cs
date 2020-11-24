using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    [Table("resource_info")]
    public class resource_info
    {
        [Key]
        public Guid rec_id { get; set; }
        public Guid create_uid { get; set; }
        public DateTime create_date { get; set; }
        public Guid write_uid { get; set; }
        public DateTime? write_date { get; set; }
        public Guid x_reportId { get; set; }
        public Guid x_zhongtuId { get; set; }
        public string x_title { get; set; }
        public string x_subtitle { get; set; }
        public string x_webtitle { get; set; }
        public string x_keywords { get; set; }
        public string x_author { get; set; }
        public string x_company { get; set; }
        public string x_remark { get; set; }
        public string x_content { get; set; }
        public string x_file { get; set; }
        public DateTime x_publishdate { get; set; }
        public string x_infosource { get; set; }
        public string x_language { get; set; }
        public string x_country { get; set; }
        public string x_area { get; set; }
        public int? x_page { get; set; }
        public string x_directory { get; set; }
        public int x_level { get; set; }
        public Guid x_watchId { get; set; }
        public Guid x_verifystate { get; set; }
        public bool is_valid { get; set; }
        public int docmain_id { get; set; }
        public Guid x_manager { get; set; }
        public bool is_main { get; set; }
        public string bj { get; set; }
    }
}
