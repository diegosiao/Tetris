using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Common.Objects
{
    internal class DbProcedureContext
    {
        public List<DbProcedureParameter> Parameters { get; set; }

        public List<DbProcedureVariable> Variables { get; set; }       
    }
}
