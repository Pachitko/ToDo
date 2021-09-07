namespace Core.Application.Helpers
{
    public interface IHasPageParameters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
