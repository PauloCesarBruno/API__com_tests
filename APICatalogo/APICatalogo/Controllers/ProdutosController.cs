using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController  : ControllerBase
    {
        //Injeção de dependência:
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        //Exemplo de varias rotas para um mesmo endpoint
        [HttpGet("Primeiro")]          
        [HttpGet("/Primeiro")] // A barra invertida ignora o -> [Route("api/[controller]")]
        public ActionResult<Produto> Get2()
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault();
            if (produto is null)
            {
                return NotFound("Produto não encontrado !");
            }
            return produto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.AsNoTracking().ToList();
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados !");
            }
            return produtos;
        }

        // Rota Nomeada para obter Status 201 no Post.
        [HttpGet("{id:int:min(1)}", Name="ObterProduto")] // Restrição de rota ->  [HttpGet("{id:int:min(1)}"
        public ActionResult<Produto> Get(int id)
        {  
            var produtos =_context.Produtos.AsNoTracking().ToList().FirstOrDefault (p=> p.ProdutoId == id);
            if (produtos is null)
            {
                return NotFound("O Produto de Código " + id + " não foi encontrado !");
            }
            return produtos;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                return BadRequest();
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            /* Este recurso ao contrario do Return Ok(),
            que retorna um codigo 200 retornará um 201
            dizendo que o produto foi CRIADO*/
            return new CreatedAtRouteResult("ObterProduto",
                   new { id = produto.ProdutoId}, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put (int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            
            if(produto is null)
            {
                return NotFound("Produto de código " + id + " não foi localizado!");
            }
            
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return Ok(produto);
        }
    }
}
