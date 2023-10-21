namespace APICatalogo.DTOs;

public class CategoriaDTO
{
    public int CategoriaId { get; set; }
   
    public string Nome { get; set; }

    public string ImagemUrl { get; set; }

    //Categoria vai ter uma coleção de Produtos
    //Propriedade de navegação
    public ICollection<ProdutoDTO> Produtos { get; set; }
}
