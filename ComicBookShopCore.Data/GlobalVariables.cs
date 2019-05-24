using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ComicBookShopCore.Data
{
    public static class GlobalVariables
    {
        public static User LoggedUser { get; set; }

    }
}
