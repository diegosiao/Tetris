using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Common
{
    public class StatementBoolean : StatementSql
    {
        public StatementBoolean() { _expressions = new List<StatementBoolean>(); }

        //private readonly StatementBoolean _this;

        private List<StatementBoolean> _expressions;

        public StatementBoolean(string statement)
        {
            _expressions = new List<StatementBoolean>();
        }

        public StatementBoolean And(StatementBoolean statetment)
        {
            _expressions.Add(statetment);
            return this;
        }

        public StatementBoolean Or(StatementBoolean statetment)
        {
            return this;
        }
        
        /// <summary>
        /// It means the end of a boolean logic chain with the addition of another
        /// <para>E.g.: ((true OR true) AND (false AND true))</para>
        /// </summary>
        /// <param name="statetment"></param>
        public StatementBoolean Plus(StatementBoolean statetment)
        {
            return this;
        }
    }
}
