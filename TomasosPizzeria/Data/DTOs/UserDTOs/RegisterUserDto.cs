namespace TomasosPizzeria.Data.DTOs.UserDTOs
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }  // Se convierte internamente a hash

        public bool IsAdmin { get; set; }
    }

}
