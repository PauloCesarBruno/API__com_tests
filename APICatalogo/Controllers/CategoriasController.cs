﻿using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
//using APICatalogo.Services;  // Usado para o exemplo de Saudação no [FromServices]
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : Controller
{
    private readonly IUnitOfWork _context;
   //private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork context, /*IConfiguration configuration,*/ 
        ILogger<CategoriasController> logger, IMapper mapper)
    {
        _context = context;
        // _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
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
    public ActionResult <IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
    {
        var categorias = _context.CategoriaRepository.GetCategoriasProdutos().ToList();

        var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

        return categoriaDTO;
    }
         
    [HttpGet]
    public ActionResult <IEnumerable<CategoriaDTO>> Get()
    {
        //_logger.LogInformation("======================GET api/categorias =======================");

        var categorias = _context.CategoriaRepository.Get().ToList();

        var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

        return Ok(categoriaDTO);
    }

    [HttpGet("{id:int}", Name="ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        //_logger.LogInformation($"======================GET api/categorias/id = {id} =======================");

        var categoria = _context.CategoriaRepository.GetById(c => c.CategoriaId == id);
        if (categoria == null)
        {
       //_logger.LogInformation($"======================GET api/categorias/id = {id} =======================");

            return NotFound("A categoria de código " + id + " não foi encontrada");
        }

        var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

        return categoriaDTO;
    }

    [HttpPost]
    public ActionResult Post ([FromBody]CategoriaDTO categoriaDto)
    {
        var categoria = _mapper.Map<Categoria>(categoriaDto);

        _context.CategoriaRepository.Add(categoria);
        _context.Commit();

        var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoria.CategoriaId }, categoriaDTO);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put (int id, [FromBody] CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
        {
            return BadRequest();
        }

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        _context.CategoriaRepository.Update(categoria);
        _context.Commit();

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public ActionResult <CategoriaDTO> Delete(int id)
    {
        var categoria = _context.CategoriaRepository.GetById(c => c.CategoriaId == id);

        if ( categoria == null)
        {
            return NotFound("A Categoria de código " + id + " nõo foi encontrada!");
        }
        _context.CategoriaRepository.Delete(categoria);
        _context.Commit();

        var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

        return categoriaDto;
    }

}
