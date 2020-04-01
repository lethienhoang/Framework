using Framework.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.ServiceBus.Infrastructure
{
    public class SBQEventEnvelopeMessageRequest : IQuery<int>
    {
        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
