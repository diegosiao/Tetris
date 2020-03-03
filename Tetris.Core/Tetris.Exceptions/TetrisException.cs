using System;
using System.Runtime.Serialization;

namespace Tetris.Exceptions
{
    /// <summary>
    /// General error in internal operations
    /// </summary>
    [Serializable]
    public class TetrisException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public TetrisException()
        {
        }

        /// <summary>
        /// General error in internal operations
        /// </summary>
        /// <param name="message"></param>
        public TetrisException(string message) : base(message)
        {
        }

        /// <summary>
        /// General error in internal operations
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TetrisException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// General error in internal operations
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected TetrisException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
