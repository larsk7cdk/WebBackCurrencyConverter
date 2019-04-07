using System.ComponentModel.DataAnnotations;

namespace WebBackCurrencyConverter.API.Models
{
    public class ExchangeRequest
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Indtast beløb som er mere end 0")]
        public double Amount { get; set; }

        [Required]
        [MinLength(3)]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Ugyldig fra valuta")]
        public string FromCurrencyCode { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Ugyldig til valuta")]
        public string ToCurrencyCode { get; set; }
    }
}