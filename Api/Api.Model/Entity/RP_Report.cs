using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{ 
    [Table("RP_Report")]
    public class RP_Report
    {
        [Key]
        public int id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string WebTitle { get; set; }
        public string KeyWords { get; set; }
        public string Abstract { get; set; }
        public string Directory { get; set; }
        public string Content { get; set; }
        public int? Status { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int? FilePage { get; set; }
        public int? bgfl { get; set; }
        public string zdgzfl { get; set; }
        public string ztfl { get; set; }
        /// <summary>
        /// 多个行业以逗号隔开
        /// </summary>
        public string hyfl { get; set; }
        public int? Level { get; set; }
        public string Author { get; set; }
        public string Company { get; set; }
        public string Source { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public bool IsMain { get; set; }
        public DateTime? PublishDate { get; set; } 
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? VerifyFirstDate {get;set;}
        public DateTime? VerifyLastDate { get; set; }
        /// <summary>
        /// 旧表docmain 中主键ID
        /// </summary>
        public int docmain_id { get; set; }
        /// <summary>
        /// 旧表Resource_Info 中主键ID
        /// </summary>
        public Guid? rec_id { get; set; }
    }
}
