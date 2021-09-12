using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ToDoApi.ModelBinders
{
    public class RouteBodyValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            var valueProvider = new RouteBodyValueProvider(context.ActionContext.HttpContext);
            context.ValueProviders.Add(valueProvider);

            return Task.CompletedTask;
        }
    }
}
