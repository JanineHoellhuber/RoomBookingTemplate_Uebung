using Microsoft.EntityFrameworkCore;
using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Persistence
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _dbContext.Bookings
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Booking>> GetByRoomsAsync(int roomId)
        {
            return await _dbContext.Bookings.Where(b => b.Room.Id == roomId)
                .ToArrayAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Booking> bookings)
        {
             await _dbContext.Bookings.AddRangeAsync(bookings);
        }


    }
}