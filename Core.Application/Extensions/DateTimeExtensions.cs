using System;

namespace Core.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static int? GetCurrentAge(this DateTimeOffset? dateTimeOffset, DateTimeOffset? dateOfDeath)
        {
            var dateToCalculateTo = DateTime.Now;

            if(dateOfDeath is not null)
            {
                dateToCalculateTo = dateOfDeath.Value.UtcDateTime;
            }

            var age = dateToCalculateTo.Year - dateTimeOffset?.Year;

            if(age is not null && dateTimeOffset?.AddYears(age.Value) > dateToCalculateTo)
            {
                age--;
            }

            return age;
        }
    }
}