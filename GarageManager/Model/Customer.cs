using System;
using System.Collections.Generic;
using Tetris.Mapping;

namespace GarageManager.Model
{
    [DbMappedClass(Table = "Customers")]
    public class Customer : Person
    { 
        public List<Car> Cars { get; set; }

        [DbReadOnly]
        public DateTime Creation { get; set; }
    }
}
