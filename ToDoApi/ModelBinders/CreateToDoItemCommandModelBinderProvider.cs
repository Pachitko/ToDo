using Core.Application.Features.Commands.CreateToDoItem;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace ToDoApi.ModelBinders
{
    public class CreateToDoItemCommandModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(CreateToDoItem.Command))
            {
                return new BinderTypeModelBinder(typeof(CreateToDoItemCommandModelBinder));
            }

            return null;
        }
    }
}