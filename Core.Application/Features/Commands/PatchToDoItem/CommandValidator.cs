using System.Collections.Generic;
using Core.Domain.Entities;
using FluentValidation;
using System.Linq;
using System;

namespace Core.Application.Features.Commands.PatchToDoItem
{
    public partial class PatchToDoItem
    {
        public class CommandValidator : AbstractValidator<PatchToDoItem.Command>
        {
            private static readonly List<string> _mutableToDoItemProps = new()
            {
                nameof(ToDoItem.Title),
                nameof(ToDoItem.IsCompleted),
                nameof(ToDoItem.IsImportant),
                nameof(ToDoItem.DueDate),
                nameof(ToDoItem.Recurrence)
            };

            public CommandValidator()
            {
                RuleForEach(c => c.jsonPatchDocument.Operations)
                    .ChildRules(opertaion =>
                    {
                        opertaion.RuleFor(operation => operation.path).Must(path =>
                        {
                            if (string.IsNullOrWhiteSpace(path))
                            {
                                return false;
                            }
                            else
                            {
                                var target = path.TrimStart('/').Split('/').First();
                                return _mutableToDoItemProps.Any(prop => prop.Equals(target, StringComparison.OrdinalIgnoreCase));
                            }
                        })
                            .WithMessage(opertaion => $"The property at path '{opertaion.path}' is immutable or does not exist.");
                    })
                        .OverridePropertyName(command => command.jsonPatchDocument)
                        .WithMessage("qwerty");
            }
        }
    }
}