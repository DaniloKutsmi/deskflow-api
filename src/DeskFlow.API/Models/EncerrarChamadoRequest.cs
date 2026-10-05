using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models
{
    public class EncerrarChamadoRequest
    {
        [Required]
        public string Solucao { get; set; }
    }
}