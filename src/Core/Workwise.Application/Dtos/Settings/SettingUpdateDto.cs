namespace Workwise.Application.Dtos
{
    public record SettingUpdateDto
    {
        public string Id { get; init; } = null!;
        public string Key { get; init; } = null!;
        public string Value { get; init; } = null!;
    }
}
