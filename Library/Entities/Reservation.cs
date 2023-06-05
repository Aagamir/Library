namespace Library.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime RentedFrom { get; set; }
        public DateTime RentedUntil { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
    }
}