using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketToCode.Core.Models;
namespace TicketToCode.Core.Data
{
    public class ApplicationDBContext:DbContext
    {
        //Gör att vi kan skicka in en databasanslutning
        public ApplicationDBContext(DbContextOptions <ApplicationDBContext> options):base(options)
        { }

        //Skapa dbset för varje modellklass,EF-core kommer skapa en tabell i databasen baserat på modellen och inkluderar alla public properties som kolumner. 
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;



    }
}
