namespace diploma_server.Account
{
    public class User
    {
        public string Username { get; set; } // Уникальный идентификатор
        public string Password { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public string? Token { get; set; }
    }
}
