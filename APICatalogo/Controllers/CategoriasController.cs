using APICatalogo.Models;
using APICatalogo.Repository;
//using APICatalogo.Services;  // Usado para o exemplo de Saudação no [FromServices]
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : Controller
{
    private readonly IUnitOfWork _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public CategoriasController(IUnitOfWork context, IConfiguration configuration, 
        ILogger<CategoriasController> logger)
    {
        _context = context;
         _configuration = configuration;
        _logger = logger;
    }

    /* Criei um Midware no appsettings.json chamado autor, injetei Iconfiguration
                       * p/ obter as informações de autor e connecionString*/
    //[HttpGet("autor")]
    //public string GetAutor()
    //{
    //    var autor = _configuration["autor"];
    //    var conexao = _configuration["ConnectionStrings:DefaultConnection"];
    //    return $"Autor: {autor} Conexao: {conexao}";  
    //}

    //EXEMPLO DE USO DE INTERFACE DE SERVICO (SERVICES) PARA SAUDAÇÃO - FINS DIDÁTICOS
    //[HttpGet("saudacao/{nome}")]
    //public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
    //{
    //    return meuServico.Saudacao(nome);
    //}

    [HttpGet("Produtos")]
    public ActionResult <IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        return _context.CategoriaRepository.GetCategoriasProdutos().ToList();
    }
         
    [HttpGet]
    public ActionResult <IEnumerable<Categoria>> Get()
    {
       //_logger.LogInformation("======================GET api/categorias =======================");

        return _context.CategoriaRepository.Get().ToList();
    }

    [HttpGet("{id:int}", Name="ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        //_logger.LogInformation($"======================GET api/categorias/id = {id} =======================");

        var categoria = _context.CategoriaRepository.GetById(c => c.CategoriaId == id);
        if (categoria == null)
        {
       //_logger.LogInformation($"======================GET api/categorias/id = {id} =======================");

            return NotFound("A categoria de código " + id + " não foi encontrada");
        }
        return categoria;
    }

    [HttpPost]
    public ActionResult Post ([FromBody]Categoria categoria)
    {
        _context.CategoriaRepository.Add(categoria);
        _context.Commit();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put (int id, [FromBody] Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return BadRequest();
        }

        _context.CategoriaRepository.Update(categoria);
        _context.Commit();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoria = _context.CategoriaRepository.GetById(c => c.CategoriaId == id);
        if ( categoria == null)
        {
            return NotFound("A Categoria de código " + id + " nõo foi encontrada!");
        }
        _context.CategoriaRepository.Delete(categoria);
        _context.Commit();

        return Ok(categoria);
    }

}
