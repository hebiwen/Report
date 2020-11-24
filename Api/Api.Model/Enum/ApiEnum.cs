using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    public class ApiEnum
    {
        public static string TransferResultCode(int value)
        {
            string result = string.Empty;
            switch (value)
            {
                case (int)ResultCode.Successed:
                    result = "Success";
                    break;
                case (int)ResultCode.Failed:
                    result = "Failed";
                    break;
            }
            return result;
        }

    }

    public enum ResultCode
    {
        Successed = 0,
        Failed = 101
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
