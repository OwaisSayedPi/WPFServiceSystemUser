using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSystemUser
{
    public class OutResponse<T>
    {
        public int ResCode { get; set; }
        public String ResMessage { get; set; }
        public T ResData { get; set; }
        public OutResponse()
        {
            ResCode = 100;
            ResMessage = "Failed";
        }
    }
}
