using RoomBooking.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomBooking.Core.Contracts
{
  public interface IBookingRepository
  {
    Task<IEnumerable<Booking>> GetAllAsync();
        Task<IEnumerable<Booking>> GetByRoomsAsync(int roomId);


        Task AddRangeAsync(IEnumerable<Booking> bookings);
    }
}