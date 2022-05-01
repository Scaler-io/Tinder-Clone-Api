using System;

namespace Tinder_Dating_API.Extensions.Utilities
{
    public static class CalculateUserAgeExtension
    {
        public static int CalulateUserAge(this DateTime dob)
        {
            var today = DateTime.Now;
            var age = today.AddYears(-dob.Year).Year;

            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
