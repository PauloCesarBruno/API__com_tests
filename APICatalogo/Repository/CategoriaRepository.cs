using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext contexto) : base(contexto)
    {
    }

    public IEnumerable<Categoria> GetCategoriasProdutos()
    {
        return Get().Include(x => x.Produtos);
    }

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
    {

        return PagedList<Categoria>.ToPagedList(Get().OrderBy(oc => oc.Nome),
            categoriasParameters.PageNumber, categoriasParameters.PageSize);
    }
}
