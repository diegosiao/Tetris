using System;
using Tetris.Common.Mapping;

namespace GarageManager.Model
{
    [MappedClass(Table = "Cars")]
    public class Car
    {
        [PrimaryKey]
        public long Id { get; set; }

        public string Description { get; set; }

        public string PlatesNumber { get; set; }

        [DbReadOnly]
        public DateTime Creation { get; set; }
    }
}
