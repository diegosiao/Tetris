using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Tetris.Common
{
    public class StatementSql : Statement
    {
        public string Sentence { get; set; }

        public List<DbParameter> Parameters { get; set; }

        public CommandType CommandType { get; set; }

        public StatementSql()
        {
            Parameters = new List<DbParameter>();
        }
    }
}
