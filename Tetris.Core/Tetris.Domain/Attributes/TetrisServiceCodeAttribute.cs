using System;
using Tetris.Exceptions;

namespace Tetris
{
    /// <summary>
    /// A seven characters code that uniquely identify the service
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TetrisServiceCodeAttribute : System.Attribute
    {
        /// <summary>
        /// A seven characters code that uniquely identify the service
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// A brief description of the purpose of the associated service
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">A seven characters code that uniquely identifying the service</param>
        public TetrisServiceCodeAttribute(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length != 7)
                throw new TetrisException("The service code must have seven aplha numeric characteres");

            Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">A seven characters code that uniquely identifying the service</param>
        /// <param name="description">The description of the associated Web Action</param>
        public TetrisServiceCodeAttribute(string code, string description)
        {
            if (string.IsNullOrEmpty(code) || code.Length != 7)
                throw new TetrisException("The service code must have seven aplha numeric characteres");

            Code = code;
            Description = description;
        }
    }
}
