using System;

namespace Tetris.Core.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TetrisOutputParameterAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TetrisInputOutputParameterAttribute : Attribute
    {
    }
}
