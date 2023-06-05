﻿namespace Library.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}