using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities {
    
    public class Room {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [StringLength (200)]
        public string Name { get; set; }
        public RoomStatus Status { get; set; }
        //public bool AllowedSmoking { get; set; }
        public Room () {

        }

        public Room (int number, string name, RoomStatus status/*, bool allowedSmoking*/) {
            Number = number;
            Name = name;
            Status = status;
            //AllowedSmoking = allowedSmoking;
        }
    }
}