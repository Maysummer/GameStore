namespace GameStore.Api.Dtos
{
    public record class UpdateGameDTO (string Name, string Genre, decimal Price, DateOnly ReleaseDate);
}
