using System;
using System.Linq.Expressions;
using Tinder_Dating_API.Entites;

namespace Tinder_Dating_API.Models.Requests
{
    public class SpecParams
    {
        private int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 6;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string CurrentUserName { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public DateTime MaxDob { get; set; }
        public DateTime MinDob { get; set; }
    }
}
