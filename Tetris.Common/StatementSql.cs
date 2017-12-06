using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Common
{
    public class StatementSql
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
