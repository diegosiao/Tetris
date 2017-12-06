using GarageManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Common;

namespace GarageManager.ViewModels
{
    public class CustomersViewModel
    {
        public ObservableCollection<Customer> Customers { get; set; }
        
        public CustomersViewModel()
        {
            Customers = new ObservableCollection<Customer>(DbEntity<Customer>.GetAll());
        }
    }
}
