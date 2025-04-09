using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PiwkoMozna.Models{
public class RecenzjaModel
{
    [Key]
    public int ReviewID { get; set; }

    [Display(Name = "UżytkownikID")]
    [Required]
    public int UserID { get; set; }

    [Display(Name = "Piwo")]
    [Required]
    public required string BeerName { get; set; }

    [Display(Name = "Ocena")]
    [Required]
    public int Rating { get; set; }

    [Display(Name = "Komentarz")]
    public required string Comment { get; set; }

    [Display(Name = "Data Recenzji")]
    public DateTime ReviewDate { get; set; }

    // Relacja wiele do jednego z Użytkownik
    [ForeignKey("UserID")]
    public UzytkownikModel? UzytkownikModel { get; set; }

    // Relacja wiele do jednego z Piwo
    [ForeignKey("BeerName")]
    public PiwoModel? PiwoModel { get; set; }
}
}
