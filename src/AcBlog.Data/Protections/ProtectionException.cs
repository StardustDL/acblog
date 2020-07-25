using System;

namespace AcBlog.Data.Protections
{
    [Serializable]
    public class ProtectionException : Exception
    {
        public ProtectionException() { }
        public ProtectionException(string message) : base(message) { }
        public ProtectionException(string message, Exception inner) : base(message, inner) { }
        protected ProtectionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
