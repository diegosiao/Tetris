using System.Collections.Generic;

namespace Tetris.Common
{
    public class TetrisResult
    {
        public bool Succeded { get; set; }

        public List<TetrisMessage> Messages { get; set; }

        internal Dictionary<string, object> OutputParameters { get; set; }

        public object Return { get; set; }

        public T GetOutput<T>(string name)
        {
            return default(T);
        }

        public object GetOutput(string name)
        {
            return null;
        }
    }
}
