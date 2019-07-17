using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Entities;
using Hotel.HotelDbContext;

namespace Respository {

    public class ReservationRepository {
        public readonly MyHotelDbContext _myHotelDbContext;

        public ReservationRepository (MyHotelDbContext myHotelDbContext) {
            _myHotelDbContext = myHotelDbContext;
        }

        public async Task<List<T>> GetAll<T> () {
            return await _myHotelDbContext
                .Reservations
                .Include (x => x.Room)
                .Include (x => x.Guest)
                .ProjectTo<T> ()
                .ToListAsync ();
        }

        public async Task<IEnumerable<Reservation>> GetAll () {
            return await _myHotelDbContext
                .Reservations
                .Include (x => x.Room)
                .Include (x => x.Guest)
                .ToListAsync ();
        }
    }
}