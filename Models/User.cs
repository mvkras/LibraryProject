using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom.Krasnov__WindowsForms.Models
{
    public enum Role { Admin, User }
    public class User
    {
        public int Id { get; set; }
        [StringLength(30)]
        [Index("Login", IsUnique = true)]
        public string Login { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
