namespace Workwise.Application.Dtos
{
    public record PaginationDto<T>
    {
        public int Take { get; init; }
        public int? CategoryId { get; init; }
        public int Order { get; init; }
        public string? Search { get; init; }
        public int CurrentPage { get; init; }
        public double TotalPage { get; init; }
        public ICollection<T>? Items { get; init; }
        public T? Item { get; init; }
    }
}
