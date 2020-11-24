using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Model
{
    public class ResultData
    {
        /// <summary>
        /// error code 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// return data JArr JObject String 
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// tip message
        /// </summary>
        public List<string> msg { get; set; }

        public ResultData()
        {
            code = 0;
            data = string.Empty;
            msg = new List<string>();
        }
    }
}
