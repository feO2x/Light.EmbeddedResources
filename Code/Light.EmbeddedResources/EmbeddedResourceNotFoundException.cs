using System;
using System.Runtime.Serialization;

namespace Light.EmbeddedResources
{
    [Serializable]
    public class EmbeddedResourceNotFoundException : ArgumentException
    {
        public EmbeddedResourceNotFoundException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

        protected EmbeddedResourceNotFoundException(SerializationInfo info, StreamingContext context): base(info, context) { }
    }
}