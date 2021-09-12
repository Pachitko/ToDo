using Core.Application.Features.Commands.CreateToDoItem;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System;

namespace ToDoApi.ModelBinders
{
    public class CreateToDoItemCommandModelBinder : IModelBinder
    {
        //private readonly IModelBinder fallbackBinder;
        //public CreateToDoItemCommandModelBinder(IModelBinder fallbackBinder)
        //{
        //    this.fallbackBinder = fallbackBinder;
        //}

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var userIdValue = bindingContext.ValueProvider.GetValue("userId");
            var toDoListValue = bindingContext.ValueProvider.GetValue("toDoListId");

            //if (userIdValue == ValueProviderResult.None || toDoListValue == ValueProviderResult.None)
            //    return fallbackBinder.BindModelAsync(bindingContext);
            Guid.TryParse(userIdValue.FirstValue, out Guid userId);
            Guid.TryParse(toDoListValue.FirstValue, out Guid toDoListId);

            //string title = bindingContext.HttpContext.

            bindingContext.Result = ModelBindingResult.Success(new CreateToDoItem.Command()
            { 
                UserId = userId, 
                ToDoListId = toDoListId 
            });
            return Task.CompletedTask;
        }
    }
}
