using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integrity
{
    public sealed class Evidence
    {
        public Evidence(Type originator, object subject)
        {
            Originator = originator;
            Subject = subject;
        }

        public Type Originator { get; private set; }

        public object Subject { get; private set; }
    }
}