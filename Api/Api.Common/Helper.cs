using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Model;

using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace Api.Common
{
    public static class Helper
    {
        public static string ToJson(this object o)
        {
            try
            {
                if (o == null)
                {
                    return "[]";
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                string str = js.Serialize(o);
                str = Regex.Replace(str, @"\\/Date\((\d+)\)\\/", match =>
                {
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    time = time.ToLocalTime();
                    return time.ToString("yyyy-MM-dd HH:mm");
                });
                return str.Replace("\\/Date(-62135596800000)\\/", "");
            }
            catch (Exception)
            {
                return "[]";
            }
        }

        public static T ToDeserialize<T>(this String strJson)
        {
            try
            {
                if (string.IsNullOrEmpty(strJson))
                {
                    return default(T);
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Deserialize<T>(strJson);
            }
            catch (Exception)
            {
                return default(T);
            }
        }


        public static bool isNumber(string value)
        {
            Regex regExp = new Regex(@"^([+-]?)\d*\.?\d+$");
            return regExp.IsMatch(value);
        }

        public static bool isInteger(string value)
        {
            Regex regExp = new Regex(@"^-?[1-9]\d*$");
            return regExp.IsMatch(value);
        }

        public static bool isDecimal(string value)
        {
            Regex regExp = new Regex(@"^([0-9]\d{0,10}|10000000000)(\.\d{0,4})?$");
            return regExp.IsMatch(value);
        }

        public static bool isDateTime(string value)
        {
            try
            {
                DateTime test = Convert.ToDateTime(value);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool isEmail(string value)
        {
            Regex regExp = new Regex(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$");
            return regExp.IsMatch(value);
        }

        public static bool isLetterOnly(string value)
        {
            Regex regExp = new Regex(@"^[A-Za-z]+$");
            return regExp.IsMatch(value);
        }

        public static bool isSafeCharsOnly(string value)
        {
            return !Regex.IsMatch(value, @"[<|>|\}|\{|$|#|\*|\'|\t|\r|\n]");
        }

        public static bool TextValueCheck(string Value, int MinLength, int MaxLength, string emptyMess, string tooLongMess, string invalidMess, string isExisted ,DataValidateTypes DataType, out List<string> MessageList)
        {
            bool result = true;
            MessageList = new List<string>();
            List<string> messList = new List<string>();
            string mess = string.Empty;

            if (Value.Length == 0 && MinLength > 0)
            {
                mess = emptyMess;
                messList.Add(mess);
                result = false;
            }
            if (Value.Length > MaxLength)
            {
                mess = tooLongMess;
                messList.Add(mess);
                result = false;
            }

            if (Value.Length > 0 && MinLength >= 0)
            {
                if (!CheckValue(DataType, Value))
                {
                    mess = invalidMess;
                    messList.Add(mess);
                    result = false;
                }
            }
            MessageList.AddRange(messList);
            return result;
        }


        public static bool CheckValue(DataValidateTypes validateType, string value)
        {
            bool result = false;
            switch (validateType)
            {
                case DataValidateTypes.AnyValue:
                    result = true;
                    break;
                case DataValidateTypes.DateTime:
                    result = isDateTime(value);
                    break;
                case DataValidateTypes.Email:
                    result = isEmail(value);
                    break;
                case DataValidateTypes.Integer:
                    result = isInteger(value);
                    break;
                case DataValidateTypes.LetterOnly:
                    result = isLetterOnly(value);
                    break;
                case DataValidateTypes.Number:
                    result = isNumber(value);
                    break;
                case DataValidateTypes.SafeCharsOnly:
                    result = isSafeCharsOnly(value);
                    break;
                case DataValidateTypes.Decimal:
                    result = isDecimal(value);
                    break;
            }

            return result;
        }

    }
}
