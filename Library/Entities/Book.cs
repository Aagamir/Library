using Library.Enums;

namespace Library.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public BookState State { get; set; }
        public DateTime RentTime { get; set; }
        public int? UserId { get; set; }
    }
}