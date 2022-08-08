using System.ComponentModel.DataAnnotations.Schema;

namespace UserCrudApp.Models
{
    public class Todo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
