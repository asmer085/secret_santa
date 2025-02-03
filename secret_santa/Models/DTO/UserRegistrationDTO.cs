namespace secret_santa.Models.DTO;

public class UserRegistrationDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}