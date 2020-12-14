using RoomBooking.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Core.Contracts
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();

        Task<Customer> GetById(int id);

        void Update(Customer cus);
        
    }
}
