namespace Core.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public DateTime? Expiration { get; set; }
        public int? SignUpId { get; set; }
        public SignUp SignUp { get; set; }
    }
}
