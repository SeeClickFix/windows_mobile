using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    [DataContract]
    public abstract class ResponseBase
    {
        [IgnoreDataMember]
        public ResponseError Error { get; set; }
    }

    public class ResponseError
    {
        public ResponseError(object result, Exception exception)
        {
            this.Result = result;
            this.Exception = exception;
        }

        [DataMember(Name = "error")]
        public object Result { get; private set; }

        public Exception Exception { get; private set; }
    }
}
