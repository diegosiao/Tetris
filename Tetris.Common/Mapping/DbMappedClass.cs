using System;

namespace Tetris.Mapping
{
    public class DbMappedClass : Attribute
    {
        public string Table { get; set; }

        public string DefaultOrderByColumn { get; set; }
    }
}
