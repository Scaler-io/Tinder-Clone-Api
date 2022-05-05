using System.Runtime.Serialization;

namespace Tinder_Dating_API.Models.Constants
{
    public class SortParams
    {
        public const string CreatedAsc = "CreatedAsc";
        public const string CreatedDesc = "CreatedDesc";
        public const string LastActiveAsc = "LastActiveAsc";
        public const string LastActiveDesc = "LastActiveDesc";
    }

    public class ApplicationRoles
    {
        public const string Administrator = "Administrator";
        public const string Moderator = "Moderator";
        public const string Member = "Member";
    }

    public class ApiAccess
    {
        public const string RequireAdminRole = "RequireAdminRole";
        public const string ModeratePhotoRole = "ModeratePhotoRole";
    }
}
