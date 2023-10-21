using APICatalogo.Filter;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController  : ControllerBase
{
    //Injeção de dependência:
    private readonly IUnitOfWork _uof;
    private readonly ILogger _logger;

    public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet("menorpreco")]
    public ActionResult<IEnumerable<Produto>> GetProdutosPreco()
    {
        return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        return _uof.ProdutoRepository.Get().ToList();
    }

    // Rota Nomeada para obter Status 201 no Post.
    [HttpGet("{id:int:min(1)}", Name="ObterProduto")] // Restrição de rota ->  [HttpGet("{id:int:min(1)}"
    public ActionResult<Produto> Get(int id)
    {  
        var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("O Produto de Código " + id + " não foi encontrado !");
        }
        return produto;
    }

    [HttpPost]
    public ActionResult Post([FromBody]Produto produto)
    {
        _uof.ProdutoRepository.Add(produto);
        _uof.Commit();

        /* Este recurso ao contrario do Return Ok(),
        que retorna um codigo 200 retornará um 201
        dizendo que o produto foi CRIADO*/
        return new CreatedAtRouteResult("ObterProduto",
               new { id = produto.ProdutoId}, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put (int id, [FromBody] Produto produto)
    {
        if(id != produto.ProdutoId)
        {
            return BadRequest();
        }


        _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto == null)
        {
            return NotFound("Produto de código " + id + " não foi localizado!");
        }
        _uof.ProdutoRepository.Delete(produto);
        _uof.Commit();
        return Ok(produto);
    }
}
