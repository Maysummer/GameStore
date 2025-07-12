
using GameStore.Api.Dtos;
using GameStore.Api.DTOs;

var builder = WebApplication.CreateBuilder(args);
const string GetGameEndpointName= "GetGame";

List<GameDTO> games = [
    new (1, "Street Fighter II", "Fighting", 19.9M, new DateOnly(1992, 7, 15)),
    new (2, "Final Fantasy XIV", "Roleplaying", 59.9M, new DateOnly(2010, 9, 30)),
    new (3, "FIFA 23", "Sports", 69.99M, new DateOnly(2022, 9, 27))
];

var app = builder.Build();

// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => 
{
    GameDTO? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
}).WithName(GetGameEndpointName);

// POST /games
app.MapPost("games", (CreateGameDTO newGame) =>
{
    GameDTO game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);

    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// PUT /games/1
app.MapPut("games/{id}", (int id, UpdateGameDTO updatedGame) =>
{
    var gameIndex = games.FindIndex(game => game.Id == id);
    if (gameIndex == -1)
    {
        return Results.NotFound();
	}
    games[gameIndex] = new GameDTO(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate);

    return Results.NoContent();
});

// DELETE /games/1
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.MapGet("/", () => "Hello World!");

app.Run();
