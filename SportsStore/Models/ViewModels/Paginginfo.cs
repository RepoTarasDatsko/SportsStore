using System;

namespace SportsStore.Models.ViewModels
{
    public class Paginginfo
    {
        public int Totalitems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)Totalitems / ItemsPerPage);
    }
}

    