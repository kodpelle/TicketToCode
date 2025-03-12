using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketToCode.Core.Models
{
    class Booking
    {
        //Primärnyckeln 
        public int Id { get; set; }

        //ForeignKeys (Sätter relationer till User och Event)
        public int UserId { get; set; }
        public int EventId { get; set; }

        // Navigationsproperties(Gör det enkelt att använda relationerna i C#-koden) 
        public User User { get; set; } = null!;
        public Event Event { get; set; } = null!;

        // Antalet bokade biljetter
        public int TicketCount { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    }
}
