using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Models.Entities
{
    public class Chamado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Titulo { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        [MaxLength(10)]
        public string Prioridade { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        [Required]
        [MaxLength(150)]
        public string SolicitanteNome { get; set; }

        public DateTime DataAbertura { get; set; }

        public DateTime? DataFechamento { get; set; }

        public string? Solucao { get; set; }

        // Chave estrangeira para Categoria
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }
    }
}