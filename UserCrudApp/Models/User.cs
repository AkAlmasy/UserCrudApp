namespace UserCrudApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProfileImagePath { get; set; }

        public List<Todo> Todos { get; set; } = new List<Todo>();
    }
}
