using System;
using GameStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

public class GameStoreContext : DbContext
{
    public GameStoreContext(DbContextOptions options) : base(options) { }

    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();
}
