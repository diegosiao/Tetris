using System;

namespace Tetris
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
