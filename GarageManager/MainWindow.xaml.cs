using GarageManager.Model;
using GarageManager.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tetris.Common;

namespace GarageManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DbEntity<Customer>.Save(new Customer() { Name = "Diego Morais", Phone = "99135-7242", Email = "diegosiao@gmail.com" });
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            DbEntity<Customer>.Save(new Customer() { Id = 1, Name = "Diego Morais de Medeiros", Phone = "99135-7242", Email = "diegosiao@gmail.com" });
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DbEntity<Customer>.Delete(new Customer() { Id = 2 });
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var customer = DbEntity<Customer>.GetById(3);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            //var command = new CustomerAddCar(new Customer(), new Car());
            //command.Create(DbEngineManager.GetEngine(new Customer()));

            //var result = command.Execute();
        }
    }
}
