using System;
using System.Runtime.Serialization;

namespace Tetris.Core.Exceptions
{
    [Serializable]
    internal class TetrisDbException : Exception
    {
        public TetrisDbException()
        {
        }

        public TetrisDbException(string message) : base(message)
        {
        }

        public TetrisDbException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TetrisDbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}