using System.ComponentModel.DataAnnotations;
namespace PiwkoMozna.Models{

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

}
