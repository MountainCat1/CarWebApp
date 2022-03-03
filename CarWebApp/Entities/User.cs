namespace CarWebApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Guest = 0,
        Admin = 1,
        User = 2
    }
}