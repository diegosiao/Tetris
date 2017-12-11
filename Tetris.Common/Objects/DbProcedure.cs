﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Mapping;

namespace Tetris.Common
{
    public abstract class DbProcedure : DbObject
    {
        private readonly DbEngine _engine;

        internal StringBuilder _body { get; set; }

        private List<Statement> _blocks { get; set; }

        public DbProcedure(DbEngine engine)
        {
            _engine = engine;
            _blocks = new List<Statement>();
        }

        protected void Parameters(params DbProcedureParameter[] parameters)
        {

        }

        protected void Variables(params DbProcedureVariable[] parameters)
        {

        }

        protected abstract void Body();

        public TetrisResult Execute()
        {
            _engine.ExecuteReadingStatement(null);
            return null;
        }

        public object Execute<T>()
        {
            _engine.ExecuteReadingStatement(null);
            return null;
        }

        public void Create(DbEngine engine)
        {
            Body();

            var statement = new StatementSql();
            statement.Sentence = engine.SqlProcedureCreation(this);

            engine.ExecuteStatement(statement);
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

        protected StatementInsert Insert(object Object)
        {
            var insert = new StatementInsert(_engine.GetInsertStatement(Object));
            return insert;
        }
        
        protected StatementInsert Insert(string Table, object args)
        {
            return null;
        }

        protected StatementBoolean NotExists(object Object)
        {
            var booleanStatement = new StatementBoolean();
            booleanStatement.Sentence = $@"NOT EXISTS(SELECT * FROM { MappingUtils.GetTable(Object) } WHERE { MappingUtils.GetPrimaryKeyName(Object) } = @{ MappingUtils.GetPrimaryKeyName(Object) })";
            booleanStatement.Parameters.Add(_engine.GetParameter("@" + MappingUtils.GetPrimaryKeyName(Object), MappingUtils.GetIdValue(Object)));
                        
            return booleanStatement;
        }

        protected StatementBoolean Exists(object Object)
        {
            return null;
        }

        protected StatementIf If(StatementBoolean statement)
        {
            var @if = new StatementIf(statement);
            @if.Add(statement);

            _blocks.Add(@if);

            return @if;
        }

        protected void Return()
        {

        }

        public override string ToString()
        {
            _body = new StringBuilder();

            foreach (var block in _blocks)
                _body.AppendLine(block.ToString());

            return _body.ToString();
        }
    }
}