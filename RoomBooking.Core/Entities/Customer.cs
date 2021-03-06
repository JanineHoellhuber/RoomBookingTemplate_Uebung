﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Core.Entities
{
    public class Customer : EntityObject
    {
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Iban { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public string FullName => $"{LastName} {FirstName}";
        public Customer()
        {
            Bookings = new List<Booking>();
        }
    }
}
