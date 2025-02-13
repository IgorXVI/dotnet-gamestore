using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos;

public record class UpdateGameDTO(
    [Required][StringLength(50)] string Name,
    [Required][Range(1, int.MaxValue)] int GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);
