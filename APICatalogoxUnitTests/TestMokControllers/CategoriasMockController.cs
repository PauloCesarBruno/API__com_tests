using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.TestMokControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasMockController : ControllerBase
    {
        private readonly IUnitOfWork _context;            
        private readonly IMapper _mapper;

        public CategoriasMockController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;               
            _mapper = mapper;
        }
            
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>>
                Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _context.CategoriaRepository.
                                GetCategorias(categoriasParameters);
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }
    }
}
