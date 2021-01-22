using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    [Table("RP_Directory")]
    public class Directory
    {
        [Key]
        public int id { get; set; }
        public string Title { get; set; }
        public int? hyfl { get; set; }
        public int? Year { get; set; }
        public int? Times { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsHome { get; set; }
        public int? SendCount { get; set; }
        public DateTime? SendLastDate { get; set; }
        /// <summary>
        /// 多个邮箱以逗号隔开
        /// </summary>
        public string SendEmail { get; set; } 
        /// <summary>
        /// 多个关联报告ID以逗号隔开
        /// </summary>
        public string Report { get; set; }
        /// <summary>
        /// 多个关联会员ID以逗号隔开
        /// </summary>
        public string User { get; set; }
    }
}
