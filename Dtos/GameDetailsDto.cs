namespace GameStore.Dtos;

public record class GameDetailsDto(int Id, string Name, string Genre, int GenreId, decimal Price, DateOnly ReleaseDate);
