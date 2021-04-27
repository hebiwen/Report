using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    public class ApiEnum
    {
        public static string TransferResultCode(ResultCode code)
        {
            string result = string.Empty;
            switch (code)
            {
                case ResultCode.Successed:
                    result = "操作完成.";
                    break;
                case ResultCode.Faild:
                    result = "操作失败.";
                    break;
            }
            return result;
        }

        public static string TransferReportStatus(int? status)
        {
            string result = string.Empty;
            switch (status)
            {
                case (int)ReportStatus.NotAudit: result = "未审核";
                    break;
                case (int)ReportStatus.FirstAuditSuccess: result = "初审通过";
                    break;
                case (int)ReportStatus.FirstAuditFaild: result = "初审失败";
                    break;
                case (int)ReportStatus.LastAuditSuccess: result = "终审通过";
                    break;
                case (int)ReportStatus.LastAuditFaild: result = "终审失败";
                    break;
                default: result = null;
                    break;
            }
            return result;
        }
        /// <summary>
        /// 转换类别的Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string TransferCategory(int? type) {
            string result = string.Empty;
            switch (type) {
                case (int)Category.BGFL: result = "报告分类";
                    break;
                case (int)Category.HYFL:result = "行业分类";
                    break;
                case (int)Category.ZTFL:result = "中图分类";
                    break;
                default: result = null;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 转换定制报告类别
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string TransferDZCategory(int? type)
        {
            string result = string.Empty;
            switch (type)
            {
                case (int)DZCategory.Sample:
                    result = "样本";
                    break;
                case (int)DZCategory.BigData:
                    result = "大数据分析报告";
                    break;
                case (int)DZCategory.Deep:
                    result = "深度研究报告";
                    break;
                case (int)DZCategory.Subject:
                    result = "专题报告";
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 转换报告级别
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string TransferLevel(int? level) {
            string result = string.Empty;
            switch (level) {
                case (int)ReportLevel.Level1: result = "一级";
                    break;
                case (int)ReportLevel.Level2: result = "二级";
                    break;
                case (int)ReportLevel.Level3: result = "三级";
                    break;
                default:result = null;
                    break;
            }
            return result;
        }

    }

    /// <summary>
    /// 返回结果(成功：1，失败：0)
    /// </summary>
    public enum ResultCode
    {
        Successed = 1,
        Faild = 0
    }

    /// <summary>
    /// 行业报告状态
    /// </summary>
    public enum ReportStatus
    {
        NotAudit = 0,
        FirstAuditSuccess = 1,
        FirstAuditFaild = 2,
        LastAuditSuccess = 3,
        LastAuditFaild = 4
    }

    public enum DataValidateTypes
    {
        AnyValue,
        Number,
        Integer,
        DateTime,
        Email,
        LetterOnly,
        SafeCharsOnly,
        Decimal
    }

    /// <summary>
    /// 行业报告级别
    /// </summary>
    public enum ReportLevel
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3
    }

    /// <summary>
    /// 分类
    /// </summary>
    public enum Category
    {
        BGFL = 1,
        HYFL = 2,
        ZTFL = 3
    }

    /// <summary>
    /// 定制报告分类
    /// </summary>
    public enum DZCategory
    {
        Sample = 1,
        BigData = 2,
        Deep = 3,
        Subject = 4
    }
}
