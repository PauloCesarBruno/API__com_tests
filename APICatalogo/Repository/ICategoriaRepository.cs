using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface ICategoriaRepository : IRepository<Categoria>
{
    //Contrato para paginação:
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);


    IEnumerable<Categoria> GetCategoriasProdutos();
}
