using System;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.HotelDbContext {

    public class MyHotelDbContext : DbContext {

        public static string DbConnectionString = "Server=localhost;Database=MyHotelDb;Trusted_Connection=True;";

        public MyHotelDbContext (DbContextOptions<MyHotelDbContext> options) : base (options) { }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Guest> Guests { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<Guest> ().HasData (
                new Guest ("Alper", DateTime.Now.AddDays (-10)) { Id = 1 }
            );
            modelBuilder.Entity<Guest> ().HasData (
                new Guest ("George", DateTime.Now.AddDays (-5)) { Id = 2 }
            );
            modelBuilder.Entity<Guest> ().HasData (
                new Guest ("Daft", DateTime.Now.AddDays (-1)) { Id = 3 }
            );

            modelBuilder.Entity<Room> ().HasData (
                new Room (101, "yellow-room", RoomStatus.Available) { Id = 1 }
            );
            modelBuilder.Entity<Room> ().HasData (
                new Room (102, "blue-room", RoomStatus.Available) { Id = 2 }
            );
            modelBuilder.Entity<Room> ().HasData (
                new Room (103, "white-room", RoomStatus.Unavailable) { Id = 3 }
            );
            modelBuilder.Entity<Room> ().HasData (
                new Room (104, "black-room", RoomStatus.Unavailable) { Id = 4 }
            );

            modelBuilder.Entity<Reservation> ().HasData (
                new Reservation (DateTime.Now.AddDays (-2), DateTime.Now.AddDays (3), 3, 1) { Id = 1 }
            );

            modelBuilder.Entity<Room> ().HasData (
                new Reservation (DateTime.Now.AddDays (-1), DateTime.Now.AddDays (4), 4, 2) { Id = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}