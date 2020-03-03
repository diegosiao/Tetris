using System;
using System.Runtime.Serialization;

namespace Tetris.Exceptions
{
    [Serializable]
    internal class TetrisDatabaseException : Exception
    {
        public TetrisDatabaseException()
        {
        }

        public TetrisDatabaseException(string message) : base(message)
        {
        }

        public TetrisDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TetrisDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}