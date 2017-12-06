using System;

namespace Tetris.Mapping
{
    public enum DefaultExpression { Timestamp }

    public class DbReadOnly : Attribute
    {
        public DefaultExpression Default { get; set; }
    }
}
