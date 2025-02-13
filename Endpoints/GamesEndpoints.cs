using System;
using GameStore.Dtos;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
    const string GET_GAME_ROUTE_NAME = "GetGame";

    private static readonly List<GameDTO> games = [
        new (
            1,
            "Street Fighter II",
            "Fighting",
            19.99M,
            new DateOnly(1992, 7, 15)
        ),
        new (
            2,
            "Final Fantasy XIV",
            "Roleplaying",
            59.99M,
            new DateOnly(2010, 9, 20)
        )
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        group.MapGet("/", () => games);

        group.MapPost("/", (CreateGameDTO newGame) =>
        {
            var game = new GameDTO(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);

            return Results.CreatedAtRoute(GET_GAME_ROUTE_NAME, new { id = game.Id }, game);
        });

        group.MapGet("/{id}", (int id) =>
        {
            var oldGame = games.Find(game => game.Id == id);

            if (oldGame == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(oldGame);
        }).WithName(GET_GAME_ROUTE_NAME);

        group.MapPut("/{id}", (int id, UpdateGameDTO newGame) =>
        {
            var oldGame = games.Find(game => game.Id == id);

            if (oldGame == null)
            {
                return Results.NotFound();
            }

            var game = new GameDTO(
                id,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Remove(oldGame);
            games.Add(game);

            return Results.Ok(game);
        });

        group.MapDelete("/{id}", (int id) =>
        {
            var oldGame = games.Find(game => game.Id == id);

            if (oldGame == null)
            {
                return Results.NotFound();
            }

            games.Remove(oldGame);

            return Results.Ok();
        });

        return group;
    }

}
