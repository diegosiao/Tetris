using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Common
{
    public class StatementBoolean : StatementSql
    {
        internal enum Operator { And, Or }

        internal Operator PredecessorOperator { get; set; }

        public StatementBoolean() { _expressions = new List<StatementBoolean>(); }

        //private readonly StatementBoolean _this;

        private List<StatementBoolean> _expressions;

        public StatementBoolean(string statement)
        {
            _expressions = new List<StatementBoolean>();
        }

        public StatementBoolean And(StatementBoolean statetment)
        {
            PredecessorOperator = Operator.And;
            _expressions.Add(statetment);
            return this;
        }

        public StatementBoolean Or(StatementBoolean statetment)
        {
            PredecessorOperator = Operator.Or;
            _expressions.Add(statetment);
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

        public override string ToString()
        {
            var code = new StringBuilder();

            code.AppendLine(Sentence);

            foreach (var item in _expressions)
                code.AppendLine((PredecessorOperator == Operator.And ? " AND " : " OR ") + item.Sentence);

            return code.ToString();
        }
    }
}
