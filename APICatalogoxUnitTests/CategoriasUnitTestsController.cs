using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using APICatalogo.TestMokControllers;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogoxUnitTests;

// Esta classe ira testar todo Controllador de CategoriasController.
public class CategoriasUnitTestsController
{
    private  IUnitOfWork repository;
    private  IMapper mapper;

    // Definir e trabalhar com uma Instancia do meu Contexto.
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }

    public static string connectionString =
        "Server=HOME;Database=CatalogoDB;User ID=sa;Password=Paradoxo22;TrustServerCertificate=True";

    /* Construtor Estático - Inicializa qualquer dado estático e
       échamado automaticamente antes que a instancia seja criada.
       ou seja, a primeira coisa que vai acontecer nessa classe é 
       a leitura deste construtor.*/
    static CategoriasUnitTestsController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
             .UseSqlServer(connectionString)
             .Options;
    }
    // Consttrutor de Instância
    public CategoriasUnitTestsController()
    {
        var config = new MapperConfiguration(cfg=>
        {
            cfg.AddProfile(new MappingProfile());
        }); 

        // Instancia do AutoMapper:
        mapper = config.CreateMapper();

        // Meu Contexto.
        var context = new AppDbContext(dbContextOptions);

        // Se eu fosse usar a Classe de Mock (DBUnitTestsMockInitialize).
        // DBUnitTestsMockInitialize db = new DBUnitTestsMockInitialize();
        // db.Seed(context);

        // Como estou usando meu banco e Contexto Original, fica:
        // Instancia do Repository...
        repository = new UnitOfWork(context);        
    }

    // Inicio dos testes Unitário:
    // Casos de teste:
    //=====================================================================================================

    // ===================================================================================================
    // TESTES DE CONSULTAS
    // ===================================================================================================

    /* Testar o método GET do Controlador de CategoriasController:
       Testar se o valor retornado é igual a uma LISTA de objeto Categria.*/

    [Fact]  // Informa que é um código de teste unitário.  
    public async void GetCategorias_Return_OkResult()
    {
        //Arrange  
        var controller = new CategoriasMockController(repository, mapper);
        CategoriasParameters param = new CategoriasParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        //Act
        var data = await controller.Get(param);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<CategoriaDTO>(data.Value.First());
    }

    //=====================================================================================================

    /* Testar o método GET (FORÇAR UM BADREQUEST) do Controlador de CategoriasController:
   Testar se o valor retornado é igual a umaLISTA de objeto Categria.*/

    [Fact] // Informa que é um código de teste unitário.  
    public async void GetCategorias_Return_BadRequestResult()
    {
        //Arrange  
        var controller = new CategoriasMockController(repository, mapper);
        CategoriasParameters parameter = new CategoriasParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        //Act
        var data = await controller.GetBad(parameter);                 

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<BadRequestResult>(data.Result);
    }


    //=====================================================================================================

    /* Testar o método GET  do Controlador de CategoriasController:
    retornar alguns dados que tenho no Banco de ados.*/

    [Fact] // Informa que é um código de teste unitário.  
    public async void GetCategorias_Return_MatchResult()
    {
        //Arrange  
        var controller = new CategoriasMockController(repository, mapper);
        CategoriasParameters parameter = new CategoriasParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        //Act
        var data = await controller.GetBd(parameter);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<List<CategoriaDTO>>(data.Value);
        // Avbaixo atribuo uma lista de categorias no objeto categorias e vou ter uma lista de objetos.
        var cat = data.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

        Assert.Equal("Bebidas", cat[0].Nome); // Índice [0] => primeiro registro do meu Db.
        Assert.Equal("bebidas.jpg", cat[0].ImagemUrl); // Índice [0] => primeiro registro do meu Db.

        Assert.Equal("Sobremesas", cat[2].Nome); // Índice [2] => terceiro registro do meu Db.
        Assert.Equal("sobremesas.jpg", cat[2].ImagemUrl); // Índice [2] => terceiro registro do meu Db.
    }


    //=====================================================================================================

    /* Testar o método GET  do Controlador de CategoriasController:
    retornar Categoria por um Id - retornatr um ObjetoDTO por Id.*/

    [Fact] // Informa que é um código de teste unitário.  
    public async void GetCategoriaById_Return_OkResult()
    {
        //Arrange  
        var controller = new CategoriasMockController(repository, mapper);
        var catId = 2;

        //Act
        var data = await controller.GetById(catId);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<CategoriaDTO>(data.Value);
    }

    //=====================================================================================================

    /* Testar o método GET  do Controlador de CategoriasController:
    retornar Categoria por um Id - retornatr um ObjetoDTO por Id.*/

    [Fact] // Informa que é um código de teste unitário.  
    public async void GetCategoriaById_Return_NotFoundResult()
    {
        //Arrange  
        var controller = new CategoriasMockController(repository, mapper);
        var catId = 9999;

        //Act
        var data = await controller.GetById(catId);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<NotFoundResult>(data.Result);
    }

    /// ===================================================================================================
    // TESTES DE MANIPULAÇÃ - POST / PUT / DELETE   
    // ===================================================================================================

    /* Testar o método post  do Controlador de CategoriasController:
     tem que retornar um CreatedAtRouteResult.*/

    [Fact] // Informa que é um código de teste unitário.  
    public async void Post_Categoria_AddValidData_Return_CreatedResult()
    {
        //Arrange  
        var controller = new CategoriasMockController(repository, mapper);

        // Vai ser incluido esse objeto categoria - Vai ser incluido na base de dados.
        var categ = new CategoriaDTO()
        { Nome = "Teste_inclusao", ImagemUrl = "testcatInclusao.jpg" };

        //Act
        var data = await controller.Post(categ);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        // O Retorno terá que ser um CreatedAtRouteResult.
        Assert.IsType<CreatedAtRouteResult>(data);
    }

    /* Este método Delete tera que Excluir uma categoria,
     retornar o Objeto excluido, além de persistir o dado no Banco de dados*/
    [Fact] // Informa que é um código de teste unitário.  
    public async void Deletet_Categoria_Return_OkResult()
    {
        //Arrange  
        var controller = new CategoriasMockController(repository, mapper);
        var catId = 18;
       
        //Act
        var data = await controller.Delete(catId);
        

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<CategoriaDTO>(data.Value);
    }
}
