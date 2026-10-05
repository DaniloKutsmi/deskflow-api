using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.Entities
{
    public class Interacao
    {
        [Key]
        public int Id { get; set; }

        public int ChamadoId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Autor { get; set; }

        [Required]
        public string Mensagem { get; set; }

        public DateTime DataRegistro { get; set; }
    }
}