using System;
using Tetris.Common;
using Tetris.Mapping;

namespace GarageManager.Model
{
    [DbMappedClass(Table = "Cars")]
    public class Car
    {
        [DbPrimaryKey]
        public long Id { get; set; }

        public string Description { get; set; }

        public string PlatesNumber { get; set; }

        [DbReadOnly]
        public DateTime Creation { get; set; }
    }
}
