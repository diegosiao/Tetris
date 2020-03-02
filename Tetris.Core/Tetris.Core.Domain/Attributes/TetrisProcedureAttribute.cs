using System;
using System.Linq;

namespace Tetris.Core.Domain.Attributes
{
    /// <summary>
    /// Use to indicate that a class is a query or command stored procedure representation.
    /// <para>The class needs to inherit from TetrisCommandBase or TetrisQueryBase.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TetrisProcedureAttribute : Attribute
    {
        /// <summary>
        /// Name of the stored procedure associated with this class.
        /// </summary>
        internal string Procedure { get; private set; }

        /// <summary>
        /// Use this property to override the default TetrisCommandBase or TetrisQueryBase connection string associated.
        /// </summary>
        internal string ConnectionStringKey { get; private set; }

        /// <summary>
        /// <para>Specify if the procedure have an 'outputs' varchar output parameter.</para>
        /// <para>Format expected for outputs:</para>
        /// <para>param1=error description&amp;param2=[w]warning description&amp;[i]general info description</para>
        /// <para>Errors will implicitly set 'Succeded' to false on execution results.</para>
        /// </summary>
        /// <value><c>true</c> if without the procedure does not contain errors varchar parameter; otherwise, <c>false</c>.</value>
        internal bool AddOutputsParam { get; private set; }

        /// <summary>
        /// Specify if the procedure has a 'sessionid varchar(36)' output parameter.
        /// </summary>
        internal bool AddSessionIdParam { get; private set; }

        /// <summary>
        /// If the result type is multi collection, is used to desribe in the root object the names of the collections returned.
        /// </summary>
        internal string[] ResultNames { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public TetrisQueryResultType ResultType { get; set; }

        /// <summary>
        /// Describes the database command or query database procedure associated with it.
        /// </summary>
        /// <param name="procedure">The name of the procedure associated.</param>
        /// <param name="connectionStringKey">If not one of the default specified in the configuration file 'ConnectionsString' section: 'queries' and 'commands'.</param>
        /// <param name="addOutputsParam">If the procedure contains a 'outputs' output text parameter.</param>
        /// <param name="addSessionIdParam">If the procedure declares a 'sessionid' text input parameter.</param>
        /// <param name="asObject">If the result should be treated as the a root object.</param>
        /// <param name="resultNames">
        ///     <para>If the result or results of the query procedure should be treated as properties of a root object.</para> 
        ///     <para>IMPORTANT: Is a comma separated value.</para>
        /// </param>
        public TetrisProcedureAttribute(
            string procedure, 
            string connectionStringKey = null, 
            bool addOutputsParam = true, 
            bool addSessionIdParam = true, 
            TetrisQueryResultType resultType = TetrisQueryResultType.Collection,
            string resultNames = null)
        {
            Procedure = procedure;
            ConnectionStringKey = connectionStringKey;
            AddOutputsParam = addOutputsParam;
            AddSessionIdParam = addSessionIdParam;
            ResultType = resultType;
            ResultNames = (resultNames ?? "")
                                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                                .ToArray();
        }
    }

    public enum TetrisQueryResultType 
    { 
        Collection,
        MultipleCollections,
        Single
    }
}