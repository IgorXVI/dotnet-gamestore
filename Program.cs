using GameStore.Data;
using GameStore.Endpoints;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);

SQLitePCL.raw.SetProvider(new SQLite3Provider_e_sqlite3());

var connStr = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connStr);

var app = builder.Build();

app.MapGamesEndpoints();

await app.MigrateDb();

app.Run();
