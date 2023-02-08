using Autofac;
using Autofac.Extensions.DependencyInjection;
using chessAPI;
using chessAPI.business.interfaces;
using chessAPI.models.player;
using chessAPI.models.game;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Events;

//Serilog logger (https://github.com/serilog/serilog-aspnetcore)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("chessAPI starting");
    var builder = WebApplication.CreateBuilder(args);

    var connectionStrings = new connectionStrings();
    builder.Services.AddOptions();
    builder.Services.Configure<connectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
    builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

    // Two-stage initialization (https://github.com/serilog/serilog-aspnetcore)
    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom
             .Configuration(context.Configuration)
             .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning).ReadFrom
             .Services(services).Enrich
             .FromLogContext().WriteTo
             .Console());

    // Autofac como inyección de dependencias
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new chessAPI.dependencyInjection<int, int>()));
    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UseMiddleware(typeof(chessAPI.customMiddleware<int>));
    app.MapGet("/", () =>
    {
        return "hola mundo";
    });

    app.MapPost("player", 
    [AllowAnonymous] async(IPlayerBusiness<int> bs, clsNewPlayer newPlayer) => Results.Ok(await bs.addPlayer(newPlayer)));

    //Recuperacion (GET) de las entidades Game y Player
    app.MapGet("player/{idplayer}", 
    [AllowAnonymous] async(IPlayerBusiness<int> bs, int idplayer) => Results.Ok(await bs.getPlayer(idplayer)));

    app.MapGet("game/{idgame}", 
    [AllowAnonymous] async(IGameBusiness<int> bs, int idgame) => Results.Ok(await bs.getGame(idgame)));


    //Actualización (PUT) de las entidades Game y Player
    app.MapPut("player/{idplayer}",
    [AllowAnonymous] async(IPlayerBusiness<int> bs, int idplayer, clsPlayer<int> updatePlayer) => Results.Ok(await bs.updatePlayer(updatePlayer)));

    app.MapPut("game/{idgame}",
    [AllowAnonymous] async(IGameBusiness<int> bs, int idgame, clsGame<int> updateGame) => Results.Ok(await bs.updateGame(updateGame)));


    //Inserción (POST) de la entidad Game.
    app.MapPost("game", 
    [AllowAnonymous] async(IGameBusiness<int> bs, clsNewGame<int> newGame) => Results.Ok(await bs.addGame(newGame)));




    //1. Endpoint tipo POST donde un integrante de un equipo iniciaría una partida
    //Ya estan creado los teams, solo se debe iniciar el juego, en el ajedrez siempre inician las blancas.
    app.MapPost("game/{id}",
    [AllowAnonymous] async(IGameBusiness<int> bs, int grupoBlancos) => Results.Ok(await bs.InitGame(grupoBlancos,1)));

    //2. Endpoint tipo PUT donde un integrante de un 2do equipo se adhiere a la partida para participar.
    //Se sabe que les toca negras, entonces solo se envía el id del juego.
    app.MapPut("game/{id}",
    [AllowAnonymous] async(IGameBusiness<int> bs, int idGame, int grupoNegros) => Results.Ok(await bs.secondTeamGame(idGame,grupoNegros)));

    //Se debe crear Team para realizar validación de los jugadores

    //--Si al menos 1 de los jugadores del 2do equipo existe registrado en el 1er equipo, el API debe retornar un HTTP 400 (Bad Request)
    //--En ambos endpoints validar si el id del equipo existe, sino existe, devolver un HTTP 404 (not found)

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "chessAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
