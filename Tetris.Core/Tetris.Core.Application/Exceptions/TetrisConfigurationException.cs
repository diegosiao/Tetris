using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Core.Tetris.Core.Application.Exceptions
{
    public class TetrisConfigurationException : Exception
    {
        public TetrisConfigurationException(string message) : base(message)
        {
        }
    }
}
