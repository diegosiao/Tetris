namespace Tetris.Common
{
    public static class DbEngineManager
    {
        public static DbEngine GetEngine(object Object)
        {
            return new DbEngineSqlServer("Server=(local);Database=GarageManager;User Id=sa; Password=mila0811; ");
        }
    }
}
