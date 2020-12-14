using Microsoft.EntityFrameworkCore;
using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Persistence
{
  public class CustomerRepository : ICustomerRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public CustomerRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
      => await _dbContext.Customers
          .OrderBy(customers => customers.LastName)
          .ToListAsync();


       public async Task<Customer> GetById(int id)
        {
            return await _dbContext.Customers.FindAsync(id);
        }

        public void Update(Customer cus)
        {
            _dbContext.Customers.Update(cus);
        }

    }
}
