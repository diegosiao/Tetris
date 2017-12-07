using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Common
{
    public class StatementInsert : StatementSql
    {
        public StatementInsert() : base()
        {

        }

        public StatementInsert(StatementSql statetment)
        {
            Sentence = statetment.Sentence;
            Parameters = statetment.Parameters;
        }

        public StatementInsert SetWithId(string variable)
        {
            Sentence += "\r\nSELECT @@IDENTITY INTO @" + variable + ";\r\n";
            return this;
        }

        public StatementInsert SetWithRowsAffected(string variable)
        {
            return this;
        }

        public override string ToString()
        {
            return Sentence;
        }
    }
}
