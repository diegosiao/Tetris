using System;
using System.Collections.Generic;
using Tetris.Mapping;

namespace GarageManager.Model
{
    [MappedClass(Table = "Customers", DefaultOrderByColumn = nameof(Name))]
    public class Customer : Person
    { 
        public string Phone { get; set; }

        public string Email { get; set; }

        public List<Car> Cars { get; set; }

        [DbReadOnly]
        public DateTime Creation { get; set; }
    }
}
