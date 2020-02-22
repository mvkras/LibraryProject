using System;

namespace Diplom.Krasnov__WindowsForms.Models
{
    public class TakenBook
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int BookId { get; set; }
        public int UserId { get; set; }

        public virtual Book Book { get; set; }
        public virtual User User { get; set; }

        public override string ToString()
        {
            return Book.Title;
        }
    }
}
