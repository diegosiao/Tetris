using GarageManager.Model;
using Tetris.Common;

namespace GarageManager.Operations
{
    public class InsertCustomerWithCar : DbProcedure
    {
        protected override string Schema => "dbo";

        protected override string Name => "usp_insert_new_customer";

        private readonly Customer Customer;

        public readonly Car Car;

        public InsertCustomerWithCar(Customer customer, Car car) : base(DbEngineManager.GetEngine(new Customer()))
        {
            Customer = customer;
            Car = car;

            Parameters(new DbProcedureParameter[] { });
            Variables(new DbProcedureVariable[] { });
        }

        #region Execution context variables
        const string IdCustomer = "IdCustomer";
        const string IdCar = "IdCustomer";
        #endregion

        protected override void Body()
        {
            If(NotExists(Customer).And(NotExists(Car))).Then(
                /*Insert(Customer).SetWithId(IdCustomer),
                Insert(Car).SetWithId(IdCar),
                Insert("Customers_Cars", new { id_customer = IdCustomer, id_car = IdCar })*/
            )
            .Else(
                Warn()
            );
        }
    }
}
