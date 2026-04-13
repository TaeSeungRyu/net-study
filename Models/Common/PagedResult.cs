namespace MemberApi.Models.Common
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

        public int Page { get; set; }

        public int Size { get; set; }

        public long TotalCount { get; set; }

        public int TotalPages => Size <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)Size);

        public bool HasPrevious => Page > 1;

        public bool HasNext => Page < TotalPages;
    }
}
