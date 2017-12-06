using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Common.Mapping;

namespace Tetris.Common
{
    public abstract class DbProcedure
    {
        protected abstract string Schema { get; }

        protected abstract string Name { get; }

        protected void Parameters(params DbProcedureParameter[] parameters)
        {

        }

        protected void Variables(params DbProcedureVariable[] parameters)
        {

        }

        protected abstract void Body();

        public object Execute()
        {
            return null;
        }

        public void Create()
        {

        }

        public void Drop()
        {

        }

        protected void Declare(string Variable)
        {

        }

        protected void Set(object Value)
        {

        }

        protected object Warn()
        {
            return null;
        }

        protected InsertStatement Insert(object Object)
        {
            return null;
        }
        
        protected InsertStatement Insert(string Table, object args)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Object">The mapped object to have the primary key checked for existance</param>
        protected BooleanStatement NotExists(object Object)
        {
            var booleanStatement = new BooleanStatement();
            return booleanStatement;
        }

        protected BooleanStatement Exists(object Object)
        {
            return null;
        }

        protected IfStatement If(BooleanStatement statement)
        {
            return null;
        }

        protected void Return()
        {

        }
    }
}