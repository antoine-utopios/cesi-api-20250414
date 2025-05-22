using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entities
{
    public class Cat
    {
        // [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Breed { get; set; }
    }
}
