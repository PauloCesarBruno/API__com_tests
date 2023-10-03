using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        //EXEMPLO DE USO DE INTERFACE DE SERVICO (SERVICES) PARA SAUDAÃO - FINS DIDÁTICOS
        //[HttpGet("saudacao/{nome}")]
        //public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        //{
        //    return meuServico.Saudacao(nome);
        //}
             
        [HttpGet]
        public async Task <ActionResult <IEnumerable<Categoria>>> Get()
        {
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
            var categoria = await  _context.Categorias.AsNoTracking()
                .FirstOrDefaultAsync(c=> c.CategoriaId== id);
            if (categoria is null)
            {
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
