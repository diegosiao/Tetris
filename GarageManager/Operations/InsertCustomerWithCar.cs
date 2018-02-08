using GarageManager.Model;
using Tetris.Common;

namespace GarageManager.Operations
{
    public class InsertCustomerWithCar : DbProcedure
    {
        protected override string Schema => "dbo";

        protected override string Name => "usp_customer_and_car_save";

        public Customer Customer { get; set; }

        public Car Car { get; set; }

        protected override void Body()
        {
            Declare("vCustomerCars");

            If(Exists(Customer, "WHERE Id IN (SELECT CustomerId FROM CustomersBanned)")).Then(
                Warn("Sorry, you have been banned from our company."),
                Return()
            );

            If(Exists(Customer).And(NotExists(Car))).Then(
                
            ).
            Else(
                Warn("Only pre-existing customers can add a car")
            );
        }
    }
}