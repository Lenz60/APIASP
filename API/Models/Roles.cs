using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Roles
    {
        [Key]
        public string Role_Id { get; set; }
        public string Role_Name { get; set; }
    }
}
