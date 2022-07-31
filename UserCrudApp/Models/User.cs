namespace UserCrudApp.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ProfileImage ProfileImage { get; set; }

        public List<Todo> Todos { get; set; } = new List<Todo>();
    }
}
