using System;
using System.Runtime.Serialization;

namespace Tetris.Core.Exceptions
{
	public class TetrisException : Exception
    {
        public TetrisException()
        {
        }

        public TetrisException(string message) : base(message)
        {
        }

        public TetrisException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TetrisException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
