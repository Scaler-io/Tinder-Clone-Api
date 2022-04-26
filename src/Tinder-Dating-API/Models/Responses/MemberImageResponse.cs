using System;

namespace Tinder_Dating_API.Models.Responses
{
    public class MemberImageResponse
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
    }
}
