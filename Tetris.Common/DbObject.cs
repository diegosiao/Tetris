namespace Tetris.Common
{
    public abstract class DbObject
    {
        abstract protected string Schema { get; }

        abstract protected string Name { get; }
    }
}