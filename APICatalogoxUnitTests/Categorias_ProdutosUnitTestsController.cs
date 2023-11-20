using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using APICatalogo.TestMokControllers;
using APICatalogoxUnitTests.TestMokControllers;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogoxUnitTests;

// Esta classe ira testar todo Controllador de CategoriasController.
public class CategoriasUnitTestsController
{
    private IUnitOfWork repository;
    private IMapper mapper;

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
        var config = new MapperConfiguration(cfg =>
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
    Testar se o valor retornado é igual a um BadRequest - (400).*/
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
    Testar se o valor retornado é igual a um NotFound - (404).retornar */
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
     tem que retornar um CreatedAtRouteResult - (201)e persistir no Banco de Dados*/
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
        // O Retorno terá que ser um CreatedAtRouteResult - (201).
        Assert.IsType<CreatedAtRouteResult>(data);
    }

    /* Este método Put tera que Alterar uma categoria
     além de e persistir o dado no Banco de dados*/
    [Fact] // Informa que é um código de teste unitário.  
    public async void Put_Categoria_Update_ValidData_Return_OkResult()
    {
        //Arrange  
        var controller = new CategoriasMockController(repository, mapper);
        var catId = 26;

        //Act
        var existingPost = await controller.GetById(catId);
        var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

        var catDto = new CategoriaDTO();
        catDto.CategoriaId = catId;
        catDto.Nome = "Categoria Atualizada - Testes 2000";
        catDto.ImagemUrl = "Testando123";

        var updateData = controller.Put(catId, catDto);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        // O Retorna um 200-Ok, altera e persiste o registro especificado no Banco de Dados.
        Assert.IsType<OkResult>(updateData);
    }

    /* Este método Delete tera que Excluir uma categoria,
     retornar o Objeto excluido, além de epersistir o dado no Banco de dados*/
    [Fact] // Informa que é um código de teste unitário.  
    public async void Delete_Categoria_Return_OkResult()
    {
        //Arrange  
        var controller = new CategoriasMockController(repository, mapper);
        var catId = 26;

        //Act
        var data = await controller.Delete(catId);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        // O Retorno terá que ser um 200-Ok e alterar o registro especivicado no (var catId).
        Assert.IsType<CategoriaDTO>(data.Value);
    }




    //**********************************************************************************************************************//
    //**********************************************************************************************************************//



    // ===================================================================================================
    // AQUI COMEÇA TESTES DO PRODUTOCONTROLLER  
    // ===================================================================================================


    // Inicio dos testes Unitário:
    // Casos de teste:
    //=====================================================================================================

    // ===================================================================================================
    // TESTES DE CONSULTAS
    // ===================================================================================================

    /* Testar o método GET do Controlador de CategoriasController:
       Testar se o valor retornado é igual a uma LISTA de objeto Produto.*/
    [Fact]  // Informa que é um código de teste unitário.  
    public async void GetProdutos_Return_OkResult()
    {
        //Arrange  
        var controller = new ProdutosMockController(repository, mapper);
        ProdutosParameters param = new ProdutosParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        //Act
        var data = await controller.Get(param);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<ProdutoDTO>(data.Value.First());
    }

    //=====================================================================================================

    /* Testar o método GET (FORÇAR UM BADREQUEST) do Controlador de ProdutosController:
    Testar se o valor retornado é igual a um BadRequest - (400).*/
    [Fact] // Informa que é um código de teste unitário.  
    public async void GetProdutos_Return_BadRequestResult()
    {
        //Arrange  
        var controller = new ProdutosMockController(repository, mapper);
        ProdutosParameters param = new ProdutosParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        //Act
        var data = await controller.GetBadProd(param);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<BadRequestResult>(data.Result);
    }

    //=====================================================================================================

    /* Testar o método GET  do Controlador de ProdutosController:
    retornar alguns dados que tenho no Banco de ados.*/
    [Fact] // Informa que é um código de teste unitário.  
    public async void GetProdutos_Return_MatchResult()
    {
        //Arrange  
        var controller = new ProdutosMockController(repository, mapper);
        ProdutosParameters param = new ProdutosParameters()
        {
            PageNumber = 1,
            PageSize = 10
        };

        //Act
        var data = await controller.GetBd(param);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<List<ProdutoDTO>>(data.Value);
        // Avbaixo atribuo uma lista de categorias no objeto categorias e vou ter uma lista de objetos.
        var cat = data.Value.Should().BeAssignableTo<List<ProdutoDTO>>().Subject;

        Assert.Equal("Coca Cola Zero", cat[0].Nome); // Índice [0] => primeiro registro do meu Db.
        Assert.Equal("Refrigerante de Cola 350 ml", cat[0].Descricao); // Índice [0] => primeiro registro do meu Db.
        Assert.Equal("5,45", Convert.ToDecimal(cat[0].Preco).ToString()); // Índice [0] => primeiro registro do meu Db.
        Assert.Equal("cocacola.jpg", cat[0].ImagemUrl); // Índice [0] => primeiro registro do meu Db.                
        Assert.Equal("1", Convert.ToInt32(cat[0].CategoriaId).ToString()); // Índice [0] => primeiro registro do meu Db.  

        Assert.Equal("Pudim 100 g", cat[2].Nome); // Índice [2] => terceiro registro do meu Db.
        Assert.Equal("Pudim de Leite Condensado ", cat[2].Descricao); // Índice [2] => terceiro registro do meu Db.
        Assert.Equal("6,75", Convert.ToDecimal(cat[2].Preco).ToString()); // Índice [2] => terceiro registro do meu Db.
        Assert.Equal("pudim.jpg", cat[2].ImagemUrl); // Índice [2] => terceiro registro do meu Db.            
        Assert.Equal("3", Convert.ToInt32(cat[2].CategoriaId).ToString()); // Índice [2] => terceiro registro do meu Db.  
    }

    //=====================================================================================================

    /* Testar o método GET  do Controlador de ProdutosController:
    retornar Categoria por um Id - retornatr um ObjetoDTO por Id.*/
    [Fact] // Informa que é um código de teste unitário.  
    public async void GetProdutoById_Return_OkResult()
    {
        //Arrange  
        var controller = new ProdutosMockController(repository, mapper);
        var catId = 4;

        //Act
        var data = await controller.GetById(catId);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<ProdutoDTO>(data.Value);
    }

    //=====================================================================================================

    /* Testar o método GET  do Controlador de ProdutosController:
    Testar se o valor retornado é igual a um NotFound - (404).retornar */
    [Fact] // Informa que é um código de teste unitário.  
    public async void GetProdutoById_Return_NotFoundResult()
    {
        //Arrange  
        var controller = new ProdutosMockController(repository, mapper);
        var catId = 9999;

        //Act
        var data = await controller.GetById(catId);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<NotFoundResult>(data.Result);
    }

    /// ===================================================================================================
    // TESTES DE MANIPULAÇÃO - POST / PUT / DELETE   
    // ===================================================================================================

    /* Testar o método post  do Controlador de ProdutosController:
     tem que retornar um CreatedAtRouteResult - (201) e persistir no Banco de Dados*/
    [Fact] // Informa que é um código de teste unitário.  
    public async void Post_Produto_AddValidData_Return_CreatedResult()
    {
        //Arrange  
        var controller = new ProdutosMockController(repository, mapper);

        // Vai ser incluido esse objeto produto - Vai ser incluido na base de dados.
        var prod = new ProdutoDTO()
        { Nome = "Teste_inclusao", Descricao = "Testando", Preco = Convert.ToDecimal("9,99"), ImagemUrl = "testprodInclusao.jpg", CategoriaId = Convert.ToInt32("10") };

        //Act
        var data = await controller.Post(prod);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        // O Retorno terá que ser um CreatedAtRouteResult.
        Assert.IsType<CreatedAtRouteResult>(data);
    }

    /* Este método Put tera que Alterar um produto
     além de e persistir o dado no Banco de dados*/
    [Fact] // Informa que é um código de teste unitário.  
    public async void Put_Produto_Update_ValidData_Return_OkResult()
    {
        //Arrange  
        var controller = new ProdutosMockController(repository, mapper);
        var prodId = 21;

        //Act
        var existingPost = await controller.GetById(prodId);
        var result = existingPost.Value.Should().BeAssignableTo<ProdutoDTO>().Subject;

        var prodDto = new ProdutoDTO();
        prodDto.ProdutoId = prodId;
        prodDto.Nome = "Produto Atualizado - Testes 3000";
        prodDto.Descricao = "Atualizado 3000";
        prodDto.Preco = Convert.ToDecimal("99,99");
        prodDto.ImagemUrl = "Atualizado 3000.png";
        prodDto.CategoriaId = Convert.ToInt32("1");

        var updateData = controller.Put(prodId, prodDto);

        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        // O Retorna um 200-Ok, altera e persiste o registro especificado no Banco de Dados.
        Assert.IsType<OkResult>(updateData);
    }

    /* Este método Delete tera que Excluir um Produto,
     retornar o Objeto excluido, além de epersistir o dado no Banco de dados*/
    [Fact] // Informa que é um código de teste unitário.  
    public async void Deletet_Produto_Return_OkResult()
    {
        //Arrange  
        var controller = new ProdutosMockController(repository, mapper);
        var catId = 24;

        //Act
        var data = await controller.Delete(catId);


        //Assert  
        // Muita atenção para o "Assert", ele tem que seguir a lógica do teste desejado.
        Assert.IsType<ProdutoDTO>(data.Value);
    }
}
