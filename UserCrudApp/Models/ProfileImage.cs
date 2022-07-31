namespace UserCrudApp.Models
{
    public class ProfileImage
    {
        public int Id { get; set; }
        public string FileName { get; set; }

        public IFormFile Image { get; set; }
    }
}
