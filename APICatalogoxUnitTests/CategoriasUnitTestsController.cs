using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using APICatalogo.TestMokControllers;
using AutoMapper;
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

    // Inicio dos testes Unitários=============================================
    // Casos de teste:
    //=========================================================================

    /* Testar o método GET do Controlador de CategoriasController:
       Testar se o valor retornado é igual a umaLISTA de objeto Categria.*/
    [Fact]    
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
        Assert.IsType<CategoriaDTO>(data.Value.First());
    }

}
