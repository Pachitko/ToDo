namespace Core.Application.Abstractions
{
    public interface IPropertyChecker
    {
        bool TypeHasProperties<T>(string fields);
    }
}
