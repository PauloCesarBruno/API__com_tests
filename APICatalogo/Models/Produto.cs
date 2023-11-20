using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto  : IValidatableObject // Ao colocar esta classe como herança, tenho que implementar...
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [StringLength(30, ErrorMessage = "O Nome deve ter entre 05 e 20 caracteres", MinimumLength = 5)]
        [PrimeiraLetraMaiuscula] // Validador Customizado
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [StringLength(100, ErrorMessage = "A Descrição deve ter entre 15 e 100 caracteres", MinimumLength = 15)]
        [PrimeiraLetraMaiuscula] // Validador Customizado
        public string Descricao { get; set; }


        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, 10000, ErrorMessage = "O Proço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [StringLength(100, ErrorMessage = "A ImagemURL deve ter entre 15 e 100 caracteres", MinimumLength = 15)]
        public string ImagemUrl { get; set; }
       
        public float Estoque { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]        
        public DateTime DataCadastro { get; set; }

        // Refinamento de que Categoria poderá receber várioas produtos:
        // Propriedades de navegação  
        public int CategoriaId { get; set; }
        [JsonIgnore] // Ignora a obrigação de preenchimento no POST e PUT 
        [Required(ErrorMessage = "Campo Obrigatório!")]
        public Categoria Categoria { get; set; }

        // Implementação após herança de "IValidatableObject" - é uma outra técnica
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if (!string.IsNullOrEmpty(this.Nome))
            //{
            //    var primeiraLeetra = this.Nome.ToString();
            //    if (primeiraLeetra != primeiraLeetra.ToUpper())
            //    {
            //        yield return new
            //            ValidationResult("A primeira letra do Nome deve ser Maiuscula.",
            //            new[]
            //            { nameof(this.Nome) }
            //            );
            //    }
            //}

            if (this.Estoque <= 0)
            {
                yield return new
                       ValidationResult("O Estoque deve ser maior do que zero.",
                       new[]
                       { nameof(this.Estoque) }
                       );
            }
        }
    }
}

/* OBS nesta classe existem 2 formas de validação personalizada: Uma eu uso a Pasta validations para validar
 se a primeira letra do nome é maiuscula e a outra eu uso a validação personalizada dentro desta classe mesmo
fazendo a Hrança de "IValidatableObject", implementando e validando que o Esoque deve ser maior que zero logo
acima o exemplo. Então existem essas duas formas de validação personalizada.*/
