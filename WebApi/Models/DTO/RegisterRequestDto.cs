using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }
        [Required]
        public string[]? Roles { get; set; }
    }
}
