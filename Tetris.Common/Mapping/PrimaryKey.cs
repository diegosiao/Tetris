﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Common.Mapping
{
    public class PrimaryKey : Attribute
    {
        public string Name { get; set; }
    }
}
