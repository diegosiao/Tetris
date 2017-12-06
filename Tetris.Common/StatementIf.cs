using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Common
{
    public class StatementIf : Statement
    {
        private static StatementIf _this;
        
        public StatementIf()
        {
            _this = this;
        }

        public StatementIf Then(params object[] args)
        {
            return _this;
        }

        public StatementIf ElseIf()
        {
            return _this;
        }

        public StatementIf Else(object obj)
        {
            return _this;
        }

        internal void Add(StatementBoolean statement)
        {
            
        }
    }
}
