using Tetris.Mapping;

namespace GarageManager.Model
{
    public class Person
    {
        [DbPrimaryKey]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
