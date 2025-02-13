using System;
using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
    const string GET_GAME_ROUTE_NAME = "GetGame";
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        group.MapGet("/", (GameStoreContext dbContext) =>
        {
            var games = dbContext.Games.Include(x => x.Genre).ToArray().Select(x => x.ToDto());
            return Results.Ok(games);
        });

        group.MapPost("/", (CreateGameDTO newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GET_GAME_ROUTE_NAME, new { id = game.Id }, game.ToDto());
        });

        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            var oldGame = dbContext.Games.Include(x => x.Genre).FirstOrDefault(x => x.Id == id);

            if (oldGame == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(oldGame.ToDto());
        }).WithName(GET_GAME_ROUTE_NAME);

        group.MapPut("/{id}", (int id, UpdateGameDTO newGame, GameStoreContext dbContext) =>
        {
            var oldGame = dbContext.Games.Find(id);

            if (oldGame == null)
            {
                return Results.NotFound();
            }

            oldGame.Name = newGame.Name;
            oldGame.GenreId = newGame.GenreId;
            oldGame.Price = newGame.Price;
            oldGame.ReleaseDate = newGame.ReleaseDate;

            dbContext.Games.Update(oldGame);
            dbContext.SaveChanges();

            return Results.Ok();
        });

        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            var oldGame = dbContext.Games.Find(id);

            if (oldGame == null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(oldGame);
            dbContext.SaveChanges();

            return Results.Ok();
        });

        return group;
    }

}
