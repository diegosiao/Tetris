using Tetris.Common.Mapping;

namespace GarageManager.Model
{
    public class Person
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
