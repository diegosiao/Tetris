using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Mapping
{
    public class MappedClass : Attribute
    {
        public string Table { get; set; }

        public string DefaultOrderByColumn { get; set; }
    }
}
