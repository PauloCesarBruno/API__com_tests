using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        public Categoria()
        {
            // Construtor para inicialização da classe Produto
            Produtos = new Collection<Produto>();
        }

        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório !")]
        [StringLength(100)]
        public string? Nome { get; set; }

        [Required(ErrorMessage ="Campo Obrigatório!")]
        [StringLength(500)]
        public string? ImagemUrl { get; set; }

        //Categoria vai ter uma coleção de Produtos
        //Propriedade de navegação
        public ICollection<Produto>? Produtos { get; set; }
    }
}
