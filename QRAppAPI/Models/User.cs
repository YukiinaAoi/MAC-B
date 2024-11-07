namespace QRAppAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public string Name { get; set; }
        public string IDNumber { get; set; }
        public byte[] ImageData { get; set; }
    }
}
