using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Shop.Model;

namespace Shop.Helpers
{
    public class FirebaseHelper
    {
        private static readonly string FirebaseAuthKey = "FreshFirebaseToken";

        public static async Task<bool> Login(string email, string password)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCXtTb_BIGOuCIZiBqqEZTzGZ8WtmqxEr4"));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                var serializedAuth = JsonConvert.SerializeObject(auth);
                Preferences.Set(FirebaseAuthKey, serializedAuth);
                return true;
            }
            catch (Exception ex)
            {
                // Обработка ошибок аутентификации
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static void Logout()
        {
            Preferences.Remove(FirebaseAuthKey);
        }

        public static bool IsUserLoggedIn()
        {
            return Preferences.ContainsKey(FirebaseAuthKey);
        }

        public static Firebase.Auth.FirebaseAuth GetUser()
        {
            if (IsUserLoggedIn())
            {
                var serializedAuth = Preferences.Get(FirebaseAuthKey, "");
                var auth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(serializedAuth);
                return auth;
            }
            return null;
        }
        public static async Task<bool> Register(string email, string password)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCXtTb_BIGOuCIZiBqqEZTzGZ8WtmqxEr4"));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

                // Сохранение токена аутентификации
                var serializedAuth = JsonConvert.SerializeObject(auth);
                Preferences.Set(FirebaseAuthKey, serializedAuth);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static async Task<UserProfile> GetUserProfile()
        {
            var auth = GetUser();
            if (auth != null)
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig("https://car-shop-fde53-default-rtdb.europe-west1.firebasedatabase.app/"));
                var freshAuth = await authProvider.RefreshAuthAsync(auth);
                if (freshAuth.User != null)
                {
                    return new UserProfile
                    {
                        Email = freshAuth.User.Email,
                        DisplayName = freshAuth.User.DisplayName,
                        PhotoUrl = freshAuth.User.PhotoUrl
                    };
                }
            }
            return null;
        }

    }
}