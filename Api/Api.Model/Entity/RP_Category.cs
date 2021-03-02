using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    [Table("RP_Category")]
    public class RP_Category
    {
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public int? parentId { get; set; }
        public string parentName { get; set; }
        /// <summary>
        /// 1：报告分类，2：行业分类，3：中图分类
        /// </summary>
        public int? type { get; set; }
        public string typeName { get; set; }
        public string sort { get; set; }
    }
}
