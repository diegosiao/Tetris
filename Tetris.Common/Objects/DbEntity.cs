using System;
using System.Collections.Generic;
using System.Data.Common;
using Tetris.Mapping;

namespace Tetris.Common
{
    public abstract class DbEntity<T> : DbObject
    {
        public static void Save(T Object, DbTransaction Transaction = null)
        {
            if (Object == null)
                return;

            var id = MappingUtils.GetIdValue(Object);

            if (id == null || Activator.CreateInstance(id.GetType()).Equals(id))
                Insert(Object);
            else
                Update(Object, id);
        }

        private static void Insert(T Object)
        {
            var engine = DbEngineManager.GetEngine(Object);
            var statement = engine.GetInsertStatement(Object);
            engine.ExecuteStatement(statement);
        }

        private static void Update(T Object, object id)
        {
            var engine = DbEngineManager.GetEngine(Object);
            var statement = engine.GetUpdateStatement(Object);
            engine.ExecuteStatement(statement);
        }

        public static void Delete(T Object)
        {
            if (Object == null)
                return;

            var engine = DbEngineManager.GetEngine(Object);
            var statement = engine.GetDeleteStatement(Object.GetType(), MappingUtils.GetPrimaryKeyName(Object), MappingUtils.GetIdValue(Object));
            engine.ExecuteStatement(statement);
        }

        public static T GetById(object Id)
        {
            T result = Activator.CreateInstance<T>();

            var engine = DbEngineManager.GetEngine(result);
            var statement = engine.GetByIdStatement(result.GetType(), MappingUtils.GetPrimaryKeyName(result), Id);

            var reader = engine.ExecuteReadingStatement(statement);

            if (reader.Read())
            {
                foreach (var property in result.GetType().GetProperties())
                {
                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        if(property.Name.ToLower().Equals(reader.GetName(i).ToLower()))
                        {
                            property.SetValue(result, reader.GetValue(i));
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static List<T> GetAll()
        {
            return null;
        }
    }
}