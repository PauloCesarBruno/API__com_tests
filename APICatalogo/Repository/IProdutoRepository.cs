using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    //Contrato para paginação:
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);


    IEnumerable<Produto> GetProdutosPorPreco();
}
