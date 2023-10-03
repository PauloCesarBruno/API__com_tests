using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [StringLength(30, ErrorMessage = "O nome deve ter entre 05 e 20 caracteres", MinimumLength = 5)]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [StringLength(100, ErrorMessage = "A Descrição deve ter entre 15 e 100 caracteres", MinimumLength = 15)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, 10000, ErrorMessage = "O Proço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [StringLength(100, ErrorMessage = "A ImagemURL deve ter entre 15 e 100 caracteres", MinimumLength = 15)]
        public string? ImagemUrl { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        public float Estoque { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        public DateTime DataCadastro { get; set; }

        // Refinamento de que Categoria poderá receber várioas produtos:
        // Propriedades de navegação  
        public int CategoriaId { get; set; }
        [JsonIgnore] // Ignora a obrigação de preenchimento no POST e PUT 
        public Categoria? Categoria { get; set; }
    }
}
