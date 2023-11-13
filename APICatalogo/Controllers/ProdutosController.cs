using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    //Injeção de dependência:
    private readonly IUnitOfWork _uof;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger, IMapper mapper)
    {
        _uof = uof;
        _logger = logger;
        _mapper = mapper;
    }
   
    [HttpGet("menorpreco")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
    {
        var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
        var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

        return produtosDto;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>>
           Get([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);

        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
        return produtosDto;
    }

    // Rota Nomeada para obter Status 201 no Post.
    [HttpGet("{id}", Name = "ObterProduto")] // Restrição de rota ->  [HttpGet("{id:int:min(1)}"
    public async Task <ActionResult<ProdutoDTO>> Get(int id)
    {
        var produto = await _uof.ProdutoRepository
                            .GetById(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("O Produto de Código " + id + " não foi encontrado !");
        }
        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        return produtoDTO;
    }


    [HttpPost] 
    public async Task <ActionResult> Post([FromBody] ProdutoDTO produtoDto)
    {
        var produto = _mapper.Map<Produto>(produtoDto);

        _uof.ProdutoRepository.Add(produto);
         await _uof.Commit();

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        /* Este recurso ao contrario do Return Ok(),
        que retorna um codigo 200 retornará um 201
        dizendo que o produto foi CRIADO*/
        return new CreatedAtRouteResult("ObterProduto",
               new { id = produto.ProdutoId }, produtoDTO);
    }

    [HttpPut("{id}")]
    public async Task <ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
        {
            return BadRequest();
        }

        var produto = _mapper.Map<Produto>(produtoDto);

        _uof.ProdutoRepository.Update(produto);
        await _uof.Commit();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task <ActionResult<ProdutoDTO>> Delete(int id)
    {
        var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto == null)
        {
            return NotFound("Produto de código " + id + " não foi localizado!");
        }
        _uof.ProdutoRepository.Delete(produto);
        await _uof.Commit();

        var produtoDto = _mapper.Map<ProdutoDTO>(produto);

        return produtoDto;
    }
}
