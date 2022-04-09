using System;

namespace Tinder_Dating_API.Entites
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public AppUser() { }

        public AppUser(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}
