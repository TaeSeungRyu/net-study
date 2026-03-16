namespace MemberApi.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();

        public int Page { get; set; }

        public int Size { get; set; }

        public long TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}