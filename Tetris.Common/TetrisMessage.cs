namespace Tetris.Common
{
    public enum TetrisMessageType { Information, Warning, Error }

    public class TetrisMessage
    {
        public TetrisMessageType Level { get; set; }

        public string Message { get; set; }
    }
}
