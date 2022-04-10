using System;

namespace Tinder_Dating_API.Extensions.Utilities
{
    public static class CalculateUserAgeExtension
    {
        public static int CalulateUserAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
