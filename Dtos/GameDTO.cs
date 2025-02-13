namespace GameStore.Dtos;

public record class GameDTO(int Id, string Name, string Genre, decimal Price, DateOnly ReleaseDate);
