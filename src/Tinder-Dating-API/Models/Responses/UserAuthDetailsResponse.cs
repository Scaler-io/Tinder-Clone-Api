using System;
using System.Collections.Generic;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.Models.Responses
{
    public class UserAuthDetailsResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
