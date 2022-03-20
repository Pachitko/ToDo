using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection;
using System.Linq;
using System;

namespace Core.Application.Extensions
{
    public static class JsonPatchDocumentExtensions
    {
        public static void ApplyToSafely<T>(this JsonPatchDocument<T> patchDoc, T objectToApplyTo,
                                     ModelStateDictionary modelState) 
            where T : class
        {
            if (patchDoc == null) throw new ArgumentNullException(nameof(patchDoc));
            if (objectToApplyTo == null) throw new ArgumentNullException(nameof(objectToApplyTo));
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));

            patchDoc.ApplyToSafely(objectToApplyTo: objectToApplyTo, modelState: modelState, prefix: string.Empty);
        }

        public static void ApplyToSafely<T>(this JsonPatchDocument<T> patchDoc, T objectToApplyTo,
            ModelStateDictionary modelState, string prefix)
            where T : class
        {
            if (patchDoc == null) throw new ArgumentNullException(nameof(patchDoc));
            if (objectToApplyTo == null) throw new ArgumentNullException(nameof(objectToApplyTo));
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));

            var attrs = BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance;
            var properties = typeof(T).GetProperties(attrs).Select(p => p.Name).ToList();

            foreach (var op in patchDoc.Operations)
            {
                if (!string.IsNullOrWhiteSpace(op.path))
                {
                    var segments = op.path.TrimStart('/').Split('/');
                    var target = segments.First();
                    if (!properties.Contains(target, StringComparer.OrdinalIgnoreCase))
                    {
                        var key = string.IsNullOrEmpty(prefix) ? target : prefix + "." + target;
                        modelState.TryAddModelError(key, $"The property at path '{op.path}' is immutable or does not exist.");
                        return;
                    }
                }
            }

            patchDoc.ApplyTo(objectToApplyTo: objectToApplyTo);
        }
    }
}
