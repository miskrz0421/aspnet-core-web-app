using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PiwkoMozna.Models{
public class UzytkownikModel
{
    [Key]
    public int UserID { get; set; }

    [Display(Name = "Email")]
    [Required]
    public required string? Email { get; set; }

    [Display(Name = "Hasło")]
    [Required]
    public required string Password { get; set; }

    [Display(Name = "Admin")]
    [Required]
    public bool IsAdmin { get; set; }

    [Display(Name = "Token")]
    [Required]
    public required string Token { get; set; }

}
}
