using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    [Table("RP_DZReport")]
    public class RP_DZReport
    {
        [Key]
        public int id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 定制报告分类(1:样本，2：大数据分析报告，3：深度研究报告，4：专题报告)
        /// </summary>
        public int Category { get; set; }
        public string hyfl { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? Price { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        /// <summary>
        /// 文件页数
        /// </summary>
        public int? FilePage { get; set; }
        /// <summary>
        /// 可预览页数
        /// </summary>
        public int? PreviewPage { get; set; }
        /// <summary>
        /// 前端缩略图
        /// </summary>
        public string Thumb { get; set; }
        public string Description { get; set; }
        public int? Seq { get; set; }
        public string Source { get; set; }
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
