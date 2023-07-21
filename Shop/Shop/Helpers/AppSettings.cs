using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Helpers
{
    public static class AppSettings
    {
        public static string UserId
        {
            get => Preferences.Get(nameof(UserId), null);
            set => Preferences.Set(nameof(UserId), value);
        }
        public static bool IsLoggedIn
        {
            get => Microsoft.Maui.Storage.Preferences.Get(nameof(IsLoggedIn), false);
            set => Microsoft.Maui.Storage.Preferences.Set(nameof(IsLoggedIn), value);
        }
    }
}
