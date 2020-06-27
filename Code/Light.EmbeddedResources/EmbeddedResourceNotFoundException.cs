using System;
using System.Runtime.Serialization;

namespace Light.EmbeddedResources
{
    /// <summary>
    /// Indicates that an embedded resource could not be found.
    /// </summary>
    [Serializable]
    public class EmbeddedResourceNotFoundException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EmbeddedResourceNotFoundException"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public EmbeddedResourceNotFoundException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

        /// <summary>
        /// This constructor is used for serialization purposes.
        /// </summary>
        protected EmbeddedResourceNotFoundException(SerializationInfo info, StreamingContext context): base(info, context) { }
    }
}