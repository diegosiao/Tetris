using System;

namespace Tetris.Exceptions
{
    /// <summary>
    /// Wrong or missing configuration
    /// </summary>
    [Serializable]
    public class TetrisConfigurationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public TetrisConfigurationException(string message) : base(message)
        {
        }
    }
}
