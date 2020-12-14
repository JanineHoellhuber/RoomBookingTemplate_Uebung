using RoomBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace RoomBooking.ImportConsole
{
  public static class ImportController
  {
    /// <summary>
    /// Liest die Buchungen mit ihren Räumen und Kunden aus der
    /// csv-Datei ein.
    /// </summary>
    /// <returns></returns>
    public static async Task<IEnumerable<Booking>> ReadBookingsFromCsvAsync()
    {
      string[][] matrix = await MyFile.ReadStringMatrixFromCsvAsync("bookings.csv", true);
            var customer = matrix
                .Select(cus => new Customer
                {
                    LastName = cus[0],
                    FirstName = cus[1],
                    Iban = cus[2]
                }).GroupBy(line => line.FirstName + line.LastName + line.Iban).Select(s => s.First()).ToArray();

            var room = matrix
                .GroupBy(s => s[3])
                .Select(ro => new Room
                {
                    RoomNumber = ro.Key
                }).ToArray();

            var booking = matrix
                .Select(boo => new Booking
                {
                    Customer = customer.Single(c => c.FirstName == boo[1] && c.LastName == boo[0] && c.Iban == boo[2]),
                    Room = room.Single(r => r.RoomNumber == boo[3]),
                    To = boo[5],
                    From = boo[4]
                }).ToArray();

            return booking;
    }

  }
}
