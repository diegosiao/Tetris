using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Common
{
    public class IfStatement : BooleanStatement
    {
        private static IfStatement _this;
        
        private IfStatement()
        {
            _this = new IfStatement();
        }

        public IfStatement Then()
        {
            return _this;
        }

        public IfStatement Then(params object[] args)
        {
            return _this;
        }

        public IfStatement ElseIf()
        {
            return _this;
        }

        public IfStatement Else(object obj)
        {
            return _this;
        }

        internal void Add(IfStatement @if)
        {
            
        }
    }
}
