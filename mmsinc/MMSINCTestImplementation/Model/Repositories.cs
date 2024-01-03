using MMSINC.Data.Linq;

namespace MMSINCTestImplementation.Model
{
    public abstract class MMSINCTestImplementationRepository<TEntity> : Repository<TEntity>
        where TEntity : class, new() { }

    public class EmployeeRepository : MMSINCTestImplementationRepository<Employee> { }

    public class AddressRepository : MMSINCTestImplementationRepository<Address> { }

    public class CityRepository : MMSINCTestImplementationRepository<City> { }

    public class StateRepository : MMSINCTestImplementationRepository<State> { }

    public class ContactRepository : MMSINCTestImplementationRepository<Contact> { }

    public class TerritoryRepository : MMSINCTestImplementationRepository<Territory> { }

    public class EmployeeTerritoryRepository : MMSINCTestImplementationRepository<EmployeeTerritory> { }

    public class OrderRepository : MMSINCTestImplementationRepository<Order> { }
}
