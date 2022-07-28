namespace Api.Models
{
    public class LoginRequest
    {
        [Required]
        public string Identification { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
