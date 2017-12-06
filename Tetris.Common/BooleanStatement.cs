using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Common
{
    public class BooleanStatement
    {
        public BooleanStatement() { }

        public BooleanStatement(string statement)
        {
            
        }

        public BooleanStatement And(BooleanStatement statetment)
        {
            return this;
        }

        public BooleanStatement Or(BooleanStatement statetment)
        {
            return this;
        }
        
        /// <summary>
        /// It means the end of a boolean logic chain with the addition of another
        /// <para>E.g.: ((true OR true) AND (false AND true))</para>
        /// </summary>
        /// <param name="statetment"></param>
        public BooleanStatement Plus(BooleanStatement statetment)
        {
            return this;
        }
    }
}
