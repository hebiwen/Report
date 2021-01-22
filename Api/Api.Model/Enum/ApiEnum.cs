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
                default: result = "未审核";
                    break;
            }
            return result;
        }
    }

    public enum ResultCode
    {
        Successed = 0,
        Faild = 101
    }

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

}
