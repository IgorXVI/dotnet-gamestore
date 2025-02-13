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

        group.MapGet("/", async (GameStoreContext dbContext) =>
        {
            var games = await dbContext.Games.Include(x => x.Genre).Select(x => x.ToDto()).AsNoTracking().ToListAsync();
            return Results.Ok(games);
        });

        group.MapPost("/", async (CreateGameDTO newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = await dbContext.Genres.FindAsync(newGame.GenreId);

            await dbContext.Games.AddAsync(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GET_GAME_ROUTE_NAME, new { id = game.Id }, game.ToDto());
        });

        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var oldGame = await dbContext.Games.Include(x => x.Genre).FirstOrDefaultAsync(x => x.Id == id);

            if (oldGame == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(oldGame.ToGameDetailsDto());
        }).WithName(GET_GAME_ROUTE_NAME);

        group.MapPut("/{id}", async (int id, UpdateGameDTO newGame, GameStoreContext dbContext) =>
        {
            var oldGame = await dbContext.Games.FindAsync(id);

            if (oldGame == null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(oldGame).CurrentValues.SetValues(newGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.Ok();
        });

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var oldGame = await dbContext.Games.FindAsync(id);

            if (oldGame == null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(oldGame);
            await dbContext.SaveChangesAsync();

            return Results.Ok();
        });

        return group;
    }

}
