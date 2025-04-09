using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PiwkoMozna.Models{
public class PiwoModel
{
    [Key]
    [Display(Name = "Nazwa")]
    [Required]
    public required string? BeerName { get; set; }

    [Display(Name = "Browar")]
    [Required]
    public required string BreweryName { get; set; }

    [Display(Name = "Styl")]
    [Required]
    public required string Style { get; set; }

    [Display(Name = "Alkohol (%)")]
    public double ABV { get; set; }

    [Display(Name = "Średnia Ocena")]
    public double AverageRating { get; set; }

    // Relacja wiele do jednego z Browar
    [ForeignKey("BreweryName")]
    public BrowarModel? BrowarModel { get; set; }

}
}
