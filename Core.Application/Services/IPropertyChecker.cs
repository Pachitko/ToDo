namespace Core.Application.Services
{
    public interface IPropertyChecker
    {
        bool TypeHasProperties<T>(string fields);
    }
}
