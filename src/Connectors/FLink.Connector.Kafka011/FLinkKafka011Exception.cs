using System;
using FLink.Core.Exceptions;

namespace FLink.Connector.Kafka011
{
    public class FLinkKafka011Exception : FLinkException
    {
        public FLinkKafka011ErrorCode ErrorCode { get; private set; }

        public FLinkKafka011Exception(FLinkKafka011ErrorCode errorCode, string message)
            : base(message) => ErrorCode = errorCode;

        public FLinkKafka011Exception(FLinkKafka011ErrorCode errorCode, string message, Exception exception)
            : base(message, exception) => ErrorCode = errorCode;
    }
}
