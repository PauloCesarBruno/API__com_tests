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
        [StringLength(100)]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [StringLength(200)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [StringLength(500)]
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
