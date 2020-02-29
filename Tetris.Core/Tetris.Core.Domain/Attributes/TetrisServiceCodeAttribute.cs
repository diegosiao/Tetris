using System;
using Tetris.Core.Exceptions;

namespace Tetris.Core.Domain.Attributes
{
    /// <summary>
    /// A seven characters code uniquely identifying the service
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TetrisServiceCodeAttribute : System.Attribute
    {
        /// <summary>
        /// A seven characters code uniquely identifying the service
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// A brief description of the purpose of this service
        /// </summary>
        public string Description { get; set; }

        /// <param name="code">A seven characters code uniquely identifying the service</param>
        public TetrisServiceCodeAttribute(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length != 7)
                throw new TetrisException("The service code must have seven aplha numeric characteres");

            Code = code;
        }

        /// <param name="code">A seven characters code uniquely identifying the service</param>
        public TetrisServiceCodeAttribute(string code, string description)
        {
            if (string.IsNullOrEmpty(code) || code.Length != 7)
                throw new TetrisException("The service code must have seven aplha numeric characteres");

            Code = code;
            Description = description;
        }
    }
}
