using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Common
{
    public class CodeValueItem
    {
        public int Code { get; set; }
        public string Value { get; set; }


        public static List<CodeValueItem> lstStatus = new List<CodeValueItem>() {
                    new CodeValueItem { Code = 0, Value = "未审核" },
                    new CodeValueItem { Code = 1, Value = "初审通过" },
                    new CodeValueItem { Code = 2, Value = "初审失败" },
                    new CodeValueItem { Code = 3, Value = "终审通过" },
                    new CodeValueItem { Code = 4, Value = "终审失败" }
        };

        public static List<CodeValueItem> lstBgfl = new List<CodeValueItem>() {
            new CodeValueItem { Code = 10,Value = "行业资讯" },
            new CodeValueItem { Code = 11,Value = "市场分析报告" },
            new CodeValueItem { Code = 12,Value = "境内分析报告" },
            new CodeValueItem { Code = 13,Value = "境外分析报告" },
            new CodeValueItem { Code = 14,Value = "技术研究报告" },
            new CodeValueItem { Code = 15,Value = "中文技术报告" },
            new CodeValueItem { Code = 16,Value = "外文技术报告" },
            new CodeValueItem { Code = 17,Value = "投资分析报告" },
            new CodeValueItem { Code = 18,Value = "政策环境报告" },
            new CodeValueItem { Code = 19,Value = "综合分析报告" }
        };

    }
}
