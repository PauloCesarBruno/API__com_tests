using APICatalogo.Context;
using APICatalogo.Models;
//using APICatalogo.Services;  // Usado para õ exemplo de Saudação no [FromServices]
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, IConfiguration configuration, 
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
             
        [HttpGet]
        public async Task <ActionResult <IEnumerable<Categoria>>> Get()
        {
           //_logger.LogInformation("======================GET api/categorias =======================");

            var categorias = await  _context.Categorias.AsNoTracking().ToListAsync();
            if (categorias is null)
            {
                return NotFound("Categorias não encontradas !");
            }
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public async Task <ActionResult<Categoria>> Get(int id)
        {
           //_logger.LogInformation($"======================GET api/categorias/id = {id} =======================");

            var categoria = await  _context.Categorias.AsNoTracking()
                .FirstOrDefaultAsync(c=> c.CategoriaId== id);
            if (categoria is null)
            {
           //_logger.LogInformation($"======================GET api/categorias/id = {id} =======================");

                return NotFound("A categoria de código " + id + " não foi encontrada");
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post (Categoria categoria)
        {
            if (categoria is null)
            {
                return BadRequest();
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put (int id , Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c=> c.CategoriaId == id);
            if( categoria is null)
            {
                return NotFound("A Categoria de código " + id + " nõo foi encontrada!");
            }
            _context.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }

    }
}
