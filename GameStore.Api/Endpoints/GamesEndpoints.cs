using GameStore.Api.Dtos;
using GameStore.Api.DTOs;

namespace GameStore.Api.Endpoints
{
	public static class GamesEndpoints
	{
		const string GetGameEndpointName = "GetGame";

		private readonly static List<GameDTO> games = [
			new (1, "Street Fighter II", "Fighting", 19.9M, new DateOnly(1992, 7, 15)),
			new (2, "Final Fantasy XIV", "Roleplaying", 59.9M, new DateOnly(2010, 9, 30)),
			new (3, "FIFA 23", "Sports", 69.99M, new DateOnly(2022, 9, 27))
		];

		public static WebApplication MapGamesEnpoints(this WebApplication app)
		{
			var group = app.MapGroup("games"); //this is so we dont need to add games to every endpoint

			// GET /games
			group.MapGet("/", () => games);

			// GET /games/1
			group.MapGet("/{id}", (int id) =>
			{
				GameDTO? game = games.Find(game => game.Id == id);
				return game is null ? Results.NotFound() : Results.Ok(game);
			}).WithName(GetGameEndpointName);

			// POST /games
			group.MapPost("/", (CreateGameDTO newGame) =>
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
			group.MapPut("/{id}", (int id, UpdateGameDTO updatedGame) =>
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
			group.MapDelete("/{id}", (int id) =>
			{
				games.RemoveAll(game => game.Id == id);

				return Results.NoContent();
			});

			return app;
		}
	}
}
