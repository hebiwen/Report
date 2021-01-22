using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Common
{
    public class ResultData
    {
        /// <summary>
        /// page num
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        /// page total
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// error code 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// return data JArr JObject String 
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// tip message (List<string> will be convert to string)
        /// </summary>
        public string msg { get; set; }

        public ResultData()
        {
            pageIndex = 1;
            total = 0;
            code = 0;
            data = string.Empty;
            msg = string.Empty;
        }
    }
}
