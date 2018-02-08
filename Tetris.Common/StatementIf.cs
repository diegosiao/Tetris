using System.Collections.Generic;
using System.Text;

namespace Tetris.Common
{
    public class StatementIf : Statement
    {
        private List<Statement> _then_expressions;

        private StatementBoolean boolean;

        public StatementIf()
        {

        }

        public StatementIf(StatementBoolean boolean)
        {
            this.boolean = boolean;
            _then_expressions = new List<Statement>();
        }

        public StatementIf Then(params Statement[] args)
        {
            _then_expressions.AddRange(args);
            return this;
        }

        public StatementIf ElseIf(StatementBoolean statement)
        {

            return this;
        }

        public StatementIf Else(object obj)
        {

            return this;
        }

        internal void Add(StatementBoolean statement)
        {
            
        }

        public override string ToString()
        {
            var block = new StringBuilder();

            block
                .AppendLine("IF " + boolean.ToString() + " THEN ")
                .AppendLine("BEGIN");

            foreach (var item in _then_expressions)
                block.AppendLine(item.ToString());

            block.AppendLine("END");

            return block.ToString();
        }
    }
}
