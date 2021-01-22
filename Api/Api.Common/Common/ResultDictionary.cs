using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Common
{
    public class ResultDictionary
    {
        /// <summary>
        /// 行业资讯
        /// </summary>
        public const string hyzx = "B6FF03FC-D90A-41D2-B6F5-467EA1EC9C69";
        /// <summary>
        /// 市场分析报告
        /// </summary>
        public const string scfxbg = "08A59B9F-4D74-4973-82FA-550EFD968EDB";
        /// <summary>
        /// 境内分析报告
        /// </summary>
        public const string jnfxbg = "5F52C877-FDA6-4809-BD58-B868C1EF949B";
        /// <summary>
        /// 境外分析报告
        /// </summary>
        public const string jwfxbg = "4E954640-960E-4335-9E20-7639CE5D21D2";
        /// <summary>
        /// 技术研究报告
        /// </summary>
        public const string jsyjbg = "F4CF54B9-56DB-46A5-BBA6-44A1D778FC8C";
        /// <summary>
        /// 中文技术报告
        /// </summary>
        public const string zwjsbg = "09C0FC4F-74A3-4C63-A7EA-1717B18A1ED3";
        /// <summary>
        /// 外文技术报告
        /// </summary>
        public const string wwjsbg = "D4CE0372-9E37-4F59-9465-CF344B6E63C7";
        /// <summary>
        /// 投资分析报告
        /// </summary>
        public const string tzfxbg = "928F8375-96BA-4B8A-94CF-325E77A2AF46";
        /// <summary>
        /// 政策环境报告
        /// </summary>
        public const string zchjbg = "80EFBF4F-D625-4EFD-9B2D-BE5374E57DA6";
        /// <summary>
        /// 综合分析报告
        /// </summary>
        public const string zhfxbg = "FF691863-1344-4810-85B7-C6C937ED6F1E";

        public static string ConvetCategory(Guid? cID)
        {
            string result = string.Empty;
            if (cID.HasValue) {
                switch (cID.Value.ToString())
                {
                    case   hyzx: result = "行业资讯"; break;
                    case scfxbg: result = "市场分析报告"; break;
                    case jnfxbg: result = "境内分析报告"; break;
                    case jwfxbg: result = "境外分析报告"; break;
                    case jsyjbg: result = "技术研究报告"; break;
                    case zwjsbg: result = "中文技术报告"; break;
                    case wwjsbg: result = "外文技术报告"; break;
                    case tzfxbg: result = "投资分析报告"; break;
                    case zchjbg: result = "政策环境报告"; break;
                    case zhfxbg: result = "综合分析报告"; break;
                    default: break;
                }
            }
            return result;
        }

    }
}
