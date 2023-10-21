namespace APICatalogo.DTOs
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
       
        public string Nome { get; set; }
        
        public string Descricao { get; set; }
                   
        public decimal Preco { get; set; }
                                   
        public string ImagemUrl { get; set; }

        // Refinamento de que Categoria poderá receber várioas produtos:
        // Propriedades de navegação  
        public int CategoriaId { get; set; }
        
    }
}
