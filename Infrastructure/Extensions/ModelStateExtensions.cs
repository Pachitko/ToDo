using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Core.Application.Responses;

namespace Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary AddModelErrors(this ModelStateDictionary modelState, 
            IEnumerable<IdentityError> errorsToAdd)
        {
            foreach (var error in errorsToAdd)
                modelState.AddModelError(error.Code, error.Description);

            return modelState;
        }

        public static ModelStateDictionary AddModelErrors(this ModelStateDictionary modelState,
          IEnumerable<ResponseError> responseErrors)
        {
            foreach (var error in responseErrors)
                modelState.AddModelError(error.Name, error.Description);

            return modelState;
        }
    }
}